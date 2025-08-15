using System.Linq.Expressions;
using Application.Contracts.Features.DispatchDocuments.Queries.GetDispatchDocuments;
using Domain.Entities.DispatchDocuments;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contracts;

namespace Application.Features.DispatchDocuments.Queries.GetDispatchDocuments;

file sealed class GetDispatchDocumentsHandler : IRequestHandler<GetDispatchDocumentsQuery, GetDispatchDocumentsResponseDto>
{
    private readonly IAppDbContext _context;

    public GetDispatchDocumentsHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<GetDispatchDocumentsResponseDto> Handle(GetDispatchDocumentsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.DispatchDocuments
            .OrderBy(ld => ld.DateOnly)
            .ThenBy(ld => ld.DocumentNumber)
            .AsQueryable();

        query = FilterAndPaginateQuery(request, query);

        var dispatchDocuments = await query
            .Select(GetResponseDtoSelector(request))
            .ToListAsync(cancellationToken);
        return new GetDispatchDocumentsResponseDto()
        {
            DispatchDocumentDtos = dispatchDocuments
        };
    }
    
    private static IQueryable<DispatchDocument> FilterAndPaginateQuery(GetDispatchDocumentsQuery request, IQueryable<DispatchDocument> query)
    {
        if (request.Dto.DateFrom.HasValue)
            query = query.Where(DispatchDocument.Spec.FromDate(request.Dto.DateFrom.Value));

        if (request.Dto.DateTo.HasValue)
            query = query.Where(DispatchDocument.Spec.ToDate(request.Dto.DateTo.Value));

        if (request.Dto.ResourceFilter != null && request.Dto.ResourceFilter.Count != 0)
            query = query.Where(DispatchDocument.Spec.IsResourceIdContains(request.Dto.ResourceFilter));

        if (request.Dto.DocumentNumbersFilter != null && request.Dto.DocumentNumbersFilter.Count != 0)
            query = query.Where(DispatchDocument.Spec.IsNumberContains(request.Dto.DocumentNumbersFilter));

        if (request.Dto.MeasureUnitFilter != null && request.Dto.MeasureUnitFilter.Count != 0)
            query = query.Where(DispatchDocument.Spec.IsMeasureUnitIdContains(request.Dto.MeasureUnitFilter));

        if (request.Dto.ClientFilter != null && request.Dto.ClientFilter.Count != 0)
            query = query.Where(DispatchDocument.Spec.IsClientIdContains(request.Dto.ClientFilter));

        if (request.Dto.Skip.HasValue)
            query = query.Skip(request.Dto.Skip.Value);

        if (request.Dto.Take.HasValue)
            query = query.Take(request.Dto.Take.Value);

        return query;
    }

    private static Expression<Func<DispatchDocument, GetDispatchDocumentsResponseDto.DispatchDocumentDto>> GetResponseDtoSelector(GetDispatchDocumentsQuery request)
    {
        return ld => new GetDispatchDocumentsResponseDto.DispatchDocumentDto
        {
            Id = ld.Id,
            DocumentNumber = ld.DocumentNumber,
            DateOnly = ld.DateOnly,
            ClientId = ld.ClientId,
            ClientName = ld.Client.Name,
            State = ld.State,
            DocumentResources = ld.DispatchDocumentResources.AsQueryable().Select(r =>
                    new GetDispatchDocumentsResponseDto.DocumentResourceDto
                    {
                        ResourceId = r.DomainResourceId,
                        ResourceName = r.DomainResource.Name,
                        MeasureUnitId = r.MeasureUnitId,
                        MeasureUnitName = r.MeasureUnit.Name,
                        Count = r.Count
                    })
                .Where(FilterByResource(request))
                .Where(FilterByMeasureUnit(request))
                .ToList(),
        };
    }

    private static Expression<Func<GetDispatchDocumentsResponseDto.DocumentResourceDto, bool>> FilterByMeasureUnit(GetDispatchDocumentsQuery request)
    {
        return r => request.Dto.MeasureUnitFilter == null
                    ||
                    request.Dto.MeasureUnitFilter.Count == 0
                    ||
                    request.Dto.MeasureUnitFilter.Contains(r.MeasureUnitId);
    }

    private static Expression<Func<GetDispatchDocumentsResponseDto.DocumentResourceDto, bool>> FilterByResource(GetDispatchDocumentsQuery request)
    {
        return r => request.Dto.ResourceFilter == null
                    ||
                    request.Dto.ResourceFilter.Count == 0
                    ||
                    request.Dto.ResourceFilter.Contains(r.ResourceId);
    }
}