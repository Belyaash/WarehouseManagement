using System.Linq.Expressions;
using Application.Contracts.Features.LoadingDocument.Queries.GetLoadingDocuments;
using Domain.Entities.LoadingDocuments;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contracts;

namespace Application.Features.LoadingDocuments.Queries.GetLoadingDocuments;

file sealed class GetLoadingDocumentsHandler : IRequestHandler<GetLoadingDocumentsQuery, GetLoadingDocumentsResponseDto>
{
    private readonly IAppDbContext _context;

    public GetLoadingDocumentsHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<GetLoadingDocumentsResponseDto> Handle(GetLoadingDocumentsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.LoadingDocuments
            .OrderBy(ld => ld.DateOnly)
            .ThenBy(ld => ld.DocumentNumber)
            .AsQueryable();

        query = FilterAndPaginateQuery(request, query);

        var loadingDocuments = await query.Select(GetResponseDtoSelector(request))
            .ToListAsync(cancellationToken);
        return new GetLoadingDocumentsResponseDto()
        {
            LoadingDocuments = loadingDocuments
        };
    }

    private static IQueryable<LoadingDocument> FilterAndPaginateQuery(GetLoadingDocumentsQuery request, IQueryable<LoadingDocument> query)
    {
        if (request.Dto.DateFrom.HasValue)
            query = query.Where(LoadingDocument.Spec.FromDate(request.Dto.DateFrom.Value));

        if (request.Dto.DateTo.HasValue)
            query = query.Where(LoadingDocument.Spec.ToDate(request.Dto.DateTo.Value));

        if (request.Dto.ResourceFilter != null && request.Dto.ResourceFilter.Count != 0)
            query = query.Where(LoadingDocument.Spec.IsResourceIdContains(request.Dto.ResourceFilter));

        if (request.Dto.DocumentNumbersFilter != null && request.Dto.DocumentNumbersFilter.Count != 0)
            query = query.Where(LoadingDocument.Spec.IsNumberContains(request.Dto.DocumentNumbersFilter));

        if (request.Dto.MeasureUnitFilter != null && request.Dto.MeasureUnitFilter.Count != 0)
            query = query.Where(LoadingDocument.Spec.IsMeasureUnitIdContains(request.Dto.MeasureUnitFilter));

        if (request.Dto.Skip.HasValue)
            query = query.Skip(request.Dto.Skip.Value);

        if (request.Dto.Take.HasValue)
            query = query.Take(request.Dto.Take.Value);

        return query;
    }

    private static Expression<Func<LoadingDocument, GetLoadingDocumentsResponseDto.LoadingDocumentDto>> GetResponseDtoSelector(GetLoadingDocumentsQuery request)
    {
        return ld => new GetLoadingDocumentsResponseDto.LoadingDocumentDto
        {
            Id = ld.Id,
            DocumentNumber = ld.DocumentNumber,
            DateOnly = ld.DateOnly,
            DocumentResources =
                ld.Resources.AsQueryable().Select(r => new GetLoadingDocumentsResponseDto.DocumentResourceDto
                {
                    ResourceId = r.DomainResourceId,
                    ResourceName = r.DomainResource.Name,
                    MeasureUnitId = r.MeasureUnitId,
                    MeasureUnitName = r.MeasureUnit.Name,
                    Count = r.Count,
                })
                .Where(FilterByResource(request))
                .Where(FilterByMeasureUnit(request))
                .OrderBy(x => x.ResourceName)
                .ThenBy(x => x.MeasureUnitName)
                .ToList()
        };
    }

    private static Expression<Func<GetLoadingDocumentsResponseDto.DocumentResourceDto, bool>> FilterByMeasureUnit(GetLoadingDocumentsQuery request)
    {
        return r => request.Dto.MeasureUnitFilter == null
                    ||
                    request.Dto.MeasureUnitFilter.Count == 0
                    ||
                    request.Dto.MeasureUnitFilter.Contains(r.MeasureUnitId);
    }

    private static Expression<Func<GetLoadingDocumentsResponseDto.DocumentResourceDto, bool>> FilterByResource(GetLoadingDocumentsQuery request)
    {
        return r => request.Dto.ResourceFilter == null
                    ||
                    request.Dto.ResourceFilter.Count == 0
                    ||
                    request.Dto.ResourceFilter.Contains(r.ResourceId);
    }
}