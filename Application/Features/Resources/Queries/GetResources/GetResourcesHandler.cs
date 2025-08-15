using System.Linq.Expressions;
using Application.Contracts.Features.Resource.Queries.GetResources;
using Domain.Entities.Resources;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contracts;

namespace Application.Features.Resources.Queries.GetResources;

file sealed class GetResourcesHandler : IRequestHandler<GetResourcesQuery, GetResourcesResponseDto>
{
    private readonly IAppDbContext _context;

    public GetResourcesHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<GetResourcesResponseDto> Handle(GetResourcesQuery request, CancellationToken cancellationToken)
    {
        var query = GetQuery(request);

        return new GetResourcesResponseDto()
        {
            Resources = await query.Select(GetResourceDtoSelector())
                .ToListAsync(cancellationToken)
        };
    }

    private IQueryable<DomainResource> GetQuery(GetResourcesQuery request)
    {
        var query = _context.Resources
            .Where(DomainResource.Spec.ByState(request.Dto.State));

        if (request.Dto.Skip.HasValue)
            query = query.Skip(request.Dto.Skip.Value);

        if (request.Dto.Take.HasValue)
            query = query.Take(request.Dto.Take.Value);

        query = query.OrderBy(r => r.Name);
        return query;
    }

    private static Expression<Func<DomainResource, GetResourcesResponseDto.GetResourceDto>> GetResourceDtoSelector()
    {
        return r => new GetResourcesResponseDto.GetResourceDto
        {
            Id = r.Id,
            Name = r.Name,
        };
    }
}