using Application.Contracts.Features.LoadingDocument.Commands.UpdateLoadingDocument;
using Domain.Entities.Balances.Parameters;
using Domain.Entities.LoadingDocumentResources;
using Domain.Entities.LoadingDocumentResources.Parameters;
using MediatR;
using Persistence.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.LoadingDocument.Command.UpdateLoadingDocument;

file sealed class UpdateLoadingDocumentHandler : IRequestHandler<UpdateLoadingDocumentCommand>
{
    private readonly IAppDbContext _context;

    public UpdateLoadingDocumentHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateLoadingDocumentCommand request, CancellationToken cancellationToken)
    {
        var document = await _context.LoadingDocuments
            .Include(x => x.Resources)
            .ThenInclude(x => x.Balance)
            .SingleAsync(Domain.Entities.LoadingDocuments.LoadingDocument.Spec.ById(request.Dto.Id), cancellationToken);

        document.DocumentNumber = request.Dto.DocumentNumber;
        document.DateOnly = request.Dto.DateOnly;

        await LoadBalancesAsync(request, cancellationToken);

        var newResources = AddNewResourcesAndThemDtos(request, document);
        DeleteResources(request);
        UpdateResources(request, newResources, document);

        await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task LoadBalancesAsync(UpdateLoadingDocumentCommand request, CancellationToken cancellationToken)
    {
        await _context.Balances
            .Where(b => request.Dto.DocumentResources
                .Any(d => d.ResourceId == b.Id && d.MeasureUnitId == b.MeasureUnitId))
            .LoadAsync(cancellationToken);
    }

    private void UpdateResources(UpdateLoadingDocumentCommand request, List<UpdateLoadingDocumentRequestDto.DocumentResourceDto> newResources, Domain.Entities.LoadingDocuments.LoadingDocument document)
    {
        var changedResources = request.Dto.DocumentResources.Except(newResources);
        foreach (var documentResourceDto in changedResources)
        {
            var documentResource = document.Resources
                .Single(r => r.ResourceId == documentResourceDto.ResourceId
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
            .Where(dto => !_context.LoadingDocumentResources.Local
                .Any(r =>
                    r.MeasureUnitId == dto.MeasureUnitId
                    &&
                    dto.ResourceId == r.ResourceId))
            .ToList();

        var newResources = request.Dto.DocumentResources.Select(dr =>
        {
            var domainResource = _context.Resources.Local.Single(r => r.Id == dr.ResourceId);
            var measureUnit = _context.MeasureUnits.Local.Single(mu => mu.Id == dr.MeasureUnitId);
            var balance = _context.Balances.Local
                              .SingleOrDefault(d => d.ResourceId == dr.ResourceId
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

    private void DeleteResources(UpdateLoadingDocumentCommand request)
    {
        var deletedResources = _context.LoadingDocumentResources.Local
            .Where(r => !request.Dto.DocumentResources.Any(dto =>
                r.MeasureUnitId == dto.MeasureUnitId
                &&
                dto.ResourceId == r.ResourceId))
            .ToList();

        deletedResources.ForEach(r => r.Balance.Count -= r.Count);
        _context.LoadingDocumentResources.RemoveRange(deletedResources);
    }
}