using Application.Contracts.Features.DispatchDocuments.Queries.GetDispatchDocument;
using Domain.Entities.DispatchDocuments;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contracts;

namespace Application.Features.DispatchDocuments.Queries.GetDispatchDocument;

file sealed class GetDispatchDocumentHandler : IRequestHandler<GetDispatchDocumentQuery, GetDispatchDocumentResponseDto>
{
    private readonly IAppDbContext _context;

    public GetDispatchDocumentHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<GetDispatchDocumentResponseDto> Handle(GetDispatchDocumentQuery request, CancellationToken cancellationToken)
    {
        return await _context.DispatchDocuments
            .Where(DispatchDocument.Spec.ById(request.Dto.Id))
            .Select(d => new GetDispatchDocumentResponseDto
            {
                DocumentNumber = d.DocumentNumber,
                DateOnly = d.DateOnly,
                ResourceDtos = d.DispatchDocumentResources.Select(r =>
                        new GetDispatchDocumentResponseDto.DocumentResourceDto()
                        {
                            Id = r.Id,
                            ResourceId = r.DomainResourceId,
                            ResourceName = r.DomainResource.Name,
                            MeasureUnitId = r.MeasureUnitId,
                            MeasureUnitName = r.MeasureUnit.Name,
                            Count = r.Count
                        })
                    .ToList(),
                State = d.State,
                ClientId = d.ClientId,
                ClientName = d.Client.Name
            })
            .SingleAsync(cancellationToken);
    }
}