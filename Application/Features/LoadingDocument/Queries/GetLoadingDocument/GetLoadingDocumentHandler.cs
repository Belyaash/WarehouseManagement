using Application.Contracts.Features.LoadingDocument.Queries.GetLoadingDocument;
using MediatR;
using Persistence.Contracts;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.LoadingDocument.Queries.GetLoadingDocument;

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
            .Where(Domain.Entities.LoadingDocuments.LoadingDocument.Spec.ById(request.Dto.Id))
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