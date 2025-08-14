using Application.Contracts.Features.DispatchDocuments.Commands.InsertDispatchDocument;
using Domain.Entities.Balances.Parameters;
using Domain.Entities.DispatchDocumentResources;
using Domain.Entities.DispatchDocumentResources.Parameters;
using Domain.Entities.DispatchDocuments.Parameters;
using Domain.Entities.DomainClients;
using Domain.Entities.Resources;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contracts;

namespace Application.Features.DispatchDocument.Commands.InsertDispatchDocument;

file sealed class InsertDispatchDocumentHandler : IRequestHandler<InsertDispatchDocumentCommand, InsertDispatchDocumentResponseDto>
{
    private readonly IAppDbContext _context;

    public InsertDispatchDocumentHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<InsertDispatchDocumentResponseDto> Handle(InsertDispatchDocumentCommand request, CancellationToken cancellationToken)
    {
        await LoadExistingBalancesAsync(request, cancellationToken);

        var client = await GetClientAsync(request, cancellationToken);

        var newDocument = CreateDocument(request, client);
        CreateDocumentResources(request, newDocument);

        await _context.SaveChangesAsync(cancellationToken);

        return new InsertDispatchDocumentResponseDto
        {
            Id = newDocument.Id
        };
    }

    private async Task LoadExistingBalancesAsync(InsertDispatchDocumentCommand request, CancellationToken cancellationToken)
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

    private async Task<DomainClient> GetClientAsync(InsertDispatchDocumentCommand request, CancellationToken cancellationToken)
    {
        return await _context.DomainClients
            .SingleAsync(DomainClient.Spec.ById(request.Dto.ClientId), cancellationToken);
    }

    private Domain.Entities.DispatchDocuments.DispatchDocument CreateDocument(InsertDispatchDocumentCommand request, DomainClient client)
    {
        var createParameters = new CreateDispatchDocumentParameters
        {
            DocumentNumber = request.Dto.DocumentNumber,
            Client = client,
            DateOnly = request.Dto.DateOnly,
            State = request.Dto.StateType,
        };

        var newDocument = new Domain.Entities.DispatchDocuments.DispatchDocument(createParameters);
        _context.DispatchDocuments.Add(newDocument);

        return newDocument;
    }


    private void CreateDocumentResources(InsertDispatchDocumentCommand request, Domain.Entities.DispatchDocuments.DispatchDocument newDocument)
    {
        var newDocResources = request.Dto.DocumentResources.Select(dr =>
        {
            var domainResource = _context.Resources.Local.Single(r => r.Id == dr.ResourceId);
            var measureUnit = _context.MeasureUnits.Local.Single(mu => mu.Id == dr.MeasureUnitId);
            var balance = _context.Balances.Local
                .SingleOrDefault(d => d.DomainResourceId == dr.ResourceId
                                      && d.MeasureUnitId == dr.MeasureUnitId)
                ?? new Domain.Entities.Balances.Balance(new CreateBalanceParameters
                {
                    MeasureUnit = measureUnit,
                    DomainResource = domainResource
                });

            return new DispatchDocumentResource(
                new CreateDispatchDocumentResourceParameters
                {
                    DomainResource = domainResource,
                    DispatchDocument = newDocument,
                    MeasureUnit = measureUnit,
                    Balance = balance,
                    Count = dr.Count
                });
        });

        _context.DispatchDocumentResources.AddRange(newDocResources);
    }
}