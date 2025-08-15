using Application.Contracts.Features.DispatchDocuments.Commands.UpdateDispatchDocument;
using Domain.Entities.Balances;
using Domain.Entities.Balances.Parameters;
using Domain.Entities.DispatchDocumentResources;
using Domain.Entities.DispatchDocumentResources.Parameters;
using Domain.Entities.DispatchDocuments;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contracts;

namespace Application.Features.DispatchDocuments.Commands.UpdateDispatchDocument;

file sealed class UpdateDispatchDocumentHandler : IRequestHandler<UpdateDispatchDocumentCommand>
{
    private readonly IAppDbContext _context;

    public UpdateDispatchDocumentHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateDispatchDocumentCommand request, CancellationToken cancellationToken)
    {
        var document = await _context.DispatchDocuments
            .Include(x => x.DispatchDocumentResources)
            .ThenInclude(x => x.Balance)
            .SingleAsync(DispatchDocument.Spec.ById(request.Dto.Id), cancellationToken);

        await LoadBalancesAsync(request, cancellationToken);

        document.DocumentNumber = request.Dto.DocumentNumber;
        document.DateOnly = request.Dto.DateOnly;
        document.ClientId = request.Dto.ClientId;


        DeleteResources(request, document.State);

        document.SetState(request.Dto.State);
        var newResources = AddNewResourcesAndThemDtos(request, document);
        UpdateResources(request, newResources, document);

        await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task LoadBalancesAsync(UpdateDispatchDocumentCommand request, CancellationToken cancellationToken)
    {
        await _context.Balances
            .Include(x => x.DomainResource)
            .Include(x => x.MeasureUnit)
            .Where(Balance.Spec.ByResourcesContains(request.Dto.DocumentResources.Select(x => x.ResourceId)))
            .Where(Balance.Spec.ByMeasureUnitsContains(request.Dto.DocumentResources.Select(x => x.MeasureUnitId)))
            .LoadAsync(cancellationToken);
    }

    private void DeleteResources(UpdateDispatchDocumentCommand request, StateType state)
    {
        var deletedResources = _context.DispatchDocumentResources.Local
            .Where(r => !request.Dto.DocumentResources.Any(dto =>
                r.MeasureUnitId == dto.MeasureUnitId
                &&
                dto.ResourceId == r.DomainResourceId))
            .ToList();

        if (state == StateType.Actual)
        {
            deletedResources.ForEach(r => r.Balance.Count += r.Count);
        }

        _context.DispatchDocumentResources.RemoveRange(deletedResources);
    }

    private List<UpdateDispatchDocumentRequestDto.DocumentResourceDto> AddNewResourcesAndThemDtos(UpdateDispatchDocumentCommand request, Domain.Entities.DispatchDocuments.DispatchDocument document)
    {
        var newResourceDtos = request.Dto.DocumentResources
            .Where(dto => !_context.DispatchDocumentResources.Local
                .Any(r =>
                    r.MeasureUnitId == dto.MeasureUnitId
                    &&
                    dto.ResourceId == r.DomainResourceId))
            .ToList();

        var newResources = newResourceDtos.Select(dr =>
        {
            var domainResource = _context.Resources.Local.Single(r => r.Id == dr.ResourceId);
            var measureUnit = _context.MeasureUnits.Local.Single(mu => mu.Id == dr.MeasureUnitId);
            var balance = _context.Balances.Local
                              .SingleOrDefault(d => d.DomainResourceId == dr.ResourceId
                                                    && d.MeasureUnitId == dr.MeasureUnitId)
                          ?? new Balance(new CreateBalanceParameters
                          {
                              MeasureUnit = measureUnit,
                              DomainResource = domainResource
                          });

            return new DispatchDocumentResource(
                new CreateDispatchDocumentResourceParameters()
                {
                    DomainResource = domainResource,
                    DispatchDocument = document,
                    MeasureUnit = measureUnit,
                    Balance = balance,
                    Count = dr.Count
                });
        });

        _context.DispatchDocumentResources.AddRange(newResources);

        return newResourceDtos;
    }

    private void UpdateResources(UpdateDispatchDocumentCommand request, List<UpdateDispatchDocumentRequestDto.DocumentResourceDto> newResources, DispatchDocument document)
    {
        var changedResources = request.Dto.DocumentResources.Except(newResources);
        foreach (var documentResourceDto in changedResources)
        {
            var documentResource = document.DispatchDocumentResources
                .Single(r => r.DomainResourceId == documentResourceDto.ResourceId
                             &&
                             r.MeasureUnitId == documentResourceDto.MeasureUnitId);

            if (document.State == StateType.Actual)
            {
                documentResource.Balance.Count += documentResource.Count - documentResourceDto.Count;
            }

            documentResource.Count = documentResourceDto.Count;
            _context.DispatchDocumentResources.Update(documentResource);
        }
    }
}