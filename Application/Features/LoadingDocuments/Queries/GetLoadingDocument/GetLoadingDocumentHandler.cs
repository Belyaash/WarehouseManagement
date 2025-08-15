using Application.Contracts.Features.LoadingDocument.Queries.GetLoadingDocument;
using Domain.Entities.LoadingDocuments;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contracts;

namespace Application.Features.LoadingDocuments.Queries.GetLoadingDocument;

file sealed class GetLoadingDocumentHandler : IRequestHandler<GetLoadingDocumentQuery, GetLoadingDocumentResponseDto>
{
    private readonly IAppDbContext _context;

    public GetLoadingDocumentHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<GetLoadingDocumentResponseDto> Handle(GetLoadingDocumentQuery request, CancellationToken cancellationToken)
    {
        return await _context.LoadingDocuments
            .Where(LoadingDocument.Spec.ById(request.Dto.Id))
            .Select(d => new GetLoadingDocumentResponseDto
            {
                DocumentNumber = d.DocumentNumber,
                DateOnly = d.DateOnly,
                ResourceDtos = d.Resources.Select(r => new GetLoadingDocumentResponseDto.LoadingDocumentResourceDto
                    {
                        Id = r.Id,
                        ResourceId = r.DomainResourceId,
                        ResourceName = r.DomainResource.Name,
                        MeasureUnitId = r.MeasureUnitId,
                        MeasureUnitName = r.MeasureUnit.Name,
                        Count = r.Count
                    })
                    .ToList()
            })
            .SingleAsync(cancellationToken);
    }
}