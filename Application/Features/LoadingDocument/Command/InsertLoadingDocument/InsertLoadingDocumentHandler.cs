using Application.Contracts.Features.LoadingDocument.Commands.InsertLoadingDocument;
using Domain.Entities.Balances.Parameters;
using Domain.Entities.LoadingDocumentResources;
using Domain.Entities.LoadingDocumentResources.Parameters;
using Domain.Entities.LoadingDocuments.Parameters;
using Domain.Entities.Resources;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contracts;

namespace Application.Features.LoadingDocument.Command.InsertLoadingDocument;

file sealed class InsertLoadingDocumentHandler : IRequestHandler<InsertLoadingDocumentCommand, InsertLoadingDocumentResponseDto>
{
    private readonly IAppDbContext _context;

    public InsertLoadingDocumentHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<InsertLoadingDocumentResponseDto> Handle(InsertLoadingDocumentCommand request, CancellationToken cancellationToken)
    {
        await LoadExistingBalancesAsync(request, cancellationToken);

        var newDocument = CreateDocument(request);
        CreateDocumentResources(request, newDocument);

        await _context.SaveChangesAsync(cancellationToken);

        return new InsertLoadingDocumentResponseDto
        {
            Id = newDocument.Id
        };
    }

    private async Task LoadExistingBalancesAsync(InsertLoadingDocumentCommand request, CancellationToken cancellationToken)
    {
        await _context.Resources
            .Include(r => r.Balances)
            .Where(DomainResource.Spec.ByIdsList(request.Dto.DocumentResources.Select(dr => dr.ResourceId)))
            .LoadAsync(cancellationToken);

        await _context.MeasureUnits
            .Include(r => r.Balances)
            .Where(Domain.Entities.MeasureUnits.MeasureUnit.Spec.ByIdsList(request.Dto.DocumentResources.Select(dr => dr.MeasureUnitId)))
            .LoadAsync(cancellationToken);
    }

    private void UpdateBalances(InsertLoadingDocumentCommand request)
    {
        _context.Balances.Local
            .ToList()
            .ForEach(b => b.Count += request.Dto.DocumentResources
                .Where(d => d.ResourceId == b.ResourceId
                            && d.MeasureUnitId == b.MeasureUnitId)
                .Sum(d => d.Count));

        var balancesToCreate = request.Dto.DocumentResources
            .Where(d => !_context.Balances.Local.Any(b => d.ResourceId == b.ResourceId
                                                          && d.MeasureUnitId == b.MeasureUnitId))
            .ToList();

        var newBalances = balancesToCreate.Select(b => new Domain.Entities.Balances.Balance(new CreateBalanceParameters
        {
            MeasureUnit = _context.MeasureUnits.Local.Single(mu => mu.Id == b.MeasureUnitId),
            DomainResource = _context.Resources.Local.Single(r => r.Id == b.ResourceId)
        }));

        _context.Balances.AddRange(newBalances);
    }

    private Domain.Entities.LoadingDocuments.LoadingDocument CreateDocument(InsertLoadingDocumentCommand request)
    {
        var createParameters = new CreateLoadingDocumentParameters
        {
            DocumentNumber = request.Dto.DocumentNumber,
            DateOnly = request.Dto.DateOnly
        };

        var newDocument = new Domain.Entities.LoadingDocuments.LoadingDocument(createParameters);
        _context.LoadingDocuments.Add(newDocument);

        return newDocument;
    }


    private void CreateDocumentResources(InsertLoadingDocumentCommand request, Domain.Entities.LoadingDocuments.LoadingDocument newDocument)
    {
        var newDocResources = request.Dto.DocumentResources.Select(dr =>
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
                    LoadingDocument = newDocument,
                    MeasureUnit = measureUnit,
                    Balance = balance,
                    Count = dr.Count
                });
        });

        _context.LoadingDocumentResources.AddRange(newDocResources);
    }
}