using Application.Contracts.Features.LoadingDocument.Commands.UpdateLoadingDocument;
using Domain.Entities.Balances.Parameters;
using Domain.Entities.LoadingDocumentResources;
using Domain.Entities.LoadingDocumentResources.Parameters;
using Domain.Entities.Resources;
using MediatR;
using Persistence.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.LoadingDocument.Command.UpdateLoadingDocument;

file sealed class UpdateLoadingDocumentHandler : IRequestHandler<UpdateLoadingDocumentCommand>
{
    private readonly IAppDbContext _context;
    private List<Domain.Entities.MeasureUnits.MeasureUnit> _measureUnits = null!;
    private List<DomainResource> _resources = null!;
    private List<Domain.Entities.Balances.Balance> _balances = null!;


    public UpdateLoadingDocumentHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateLoadingDocumentCommand request, CancellationToken cancellationToken)
    {
        await LoadBalancesAsync(request, cancellationToken);

        var document = await _context.LoadingDocuments
            .Include(x => x.Resources)
            .ThenInclude(x => x.Balance)
            .SingleAsync(Domain.Entities.LoadingDocuments.LoadingDocument.Spec.ById(request.Dto.Id), cancellationToken);

        document.DocumentNumber = request.Dto.DocumentNumber;
        document.DateOnly = request.Dto.DateOnly;

        var newResources = AddNewResourcesAndThemDtos(request, document);
        DeleteResources(request, document);
        UpdateResources(request, newResources, document);

        await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task LoadBalancesAsync(UpdateLoadingDocumentCommand request, CancellationToken cancellationToken)
    {
        var resourcesList = request.Dto.DocumentResources.Select(x => x.ResourceId).ToArray();
        var measureUnitsList = request.Dto.DocumentResources.Select(x => x.MeasureUnitId).ToArray();

        _balances = await _context.Balances
            .Where(Domain.Entities.Balances.Balance.Spec.ByResourcesContains(resourcesList))
            .Where(Domain.Entities.Balances.Balance.Spec.ByMeasureUnitsContains(measureUnitsList))
            .ToListAsync(cancellationToken);

        _resources = await _context.Resources
            .Where(DomainResource.Spec.ByIdsList(resourcesList))
            .ToListAsync(cancellationToken);

        _measureUnits = await _context.MeasureUnits
            .Where(Domain.Entities.MeasureUnits.MeasureUnit.Spec.ByIdsList(measureUnitsList))
            .ToListAsync(cancellationToken);
    }

    private void UpdateResources(UpdateLoadingDocumentCommand request, List<UpdateLoadingDocumentRequestDto.DocumentResourceDto> newResources, Domain.Entities.LoadingDocuments.LoadingDocument document)
    {
        var changedResources = request.Dto.DocumentResources.Except(newResources).ToList();
        foreach (var documentResourceDto in changedResources)
        {
            var documentResource = document.Resources
                .Single(r => r.DomainResourceId == documentResourceDto.ResourceId
                             &&
                             r.MeasureUnitId == documentResourceDto.MeasureUnitId);

            documentResource.Balance.Count += documentResourceDto.Count - documentResource.Count;
            documentResource.Count = documentResourceDto.Count;
            _context.LoadingDocumentResources.Update(documentResource);
        }
    }

    private List<UpdateLoadingDocumentRequestDto.DocumentResourceDto> AddNewResourcesAndThemDtos(UpdateLoadingDocumentCommand request, Domain.Entities.LoadingDocuments.LoadingDocument loadingDocument)
    {
        var newResourceDtos = request.Dto.DocumentResources
            .Where(dto => !loadingDocument.Resources
                .Any(r =>
                    r.MeasureUnitId == dto.MeasureUnitId
                    &&
                    dto.ResourceId == r.DomainResourceId))
            .ToList();

        var newResources = newResourceDtos.Select(dr =>
        {
            var domainResource = _resources.Single(r => r.Id == dr.ResourceId);
            var measureUnit = _measureUnits.Single(mu => mu.Id == dr.MeasureUnitId);
            var balance = _balances
                              .SingleOrDefault(d => d.DomainResourceId == dr.ResourceId
                                                    && d.MeasureUnitId == dr.MeasureUnitId)
                          ?? new Domain.Entities.Balances.Balance(new CreateBalanceParameters
                          {
                              MeasureUnit = measureUnit,
                              DomainResource = domainResource
                          });

            return new LoadingDocumentResource(
                new CreateLoadingDocumentResourceParameters
                {
                    DomainResource = domainResource,
                    LoadingDocument = loadingDocument,
                    MeasureUnit = measureUnit,
                    Balance = balance,
                    Count = dr.Count
                });
        });

        _context.LoadingDocumentResources.AddRange(newResources);

        return newResourceDtos;
    }

    private void DeleteResources(UpdateLoadingDocumentCommand request, Domain.Entities.LoadingDocuments.LoadingDocument document)
    {
        var deletedResources = document.Resources
            .Where(r => !request.Dto.DocumentResources.Any(dto =>
                r.MeasureUnitId == dto.MeasureUnitId
                &&
                dto.ResourceId == r.DomainResourceId))
            .ToList();

        deletedResources.ForEach(r => r.Balance.Count -= r.Count);
        _context.LoadingDocumentResources.RemoveRange(deletedResources);
    }
}