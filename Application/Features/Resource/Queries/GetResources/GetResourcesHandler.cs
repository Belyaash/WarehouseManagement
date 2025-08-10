using System.Linq.Expressions;
using Application.Contracts.Features.Resource.Queries.GetResources;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contracts;

namespace Application.Features.Resource.Queries.GetResources;

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

    private IQueryable<Domain.Entities.Resources.DomainResource> GetQuery(GetResourcesQuery request)
    {
        var query = _context.Resources
            .Where(Domain.Entities.Resources.DomainResource.Spec.ByState(request.Dto.State));

        if (request.Dto.Skip.HasValue)
            query = query.Skip(request.Dto.Skip.Value);

        if (request.Dto.Take.HasValue)
            query = query.Take(request.Dto.Take.Value);

        query = query.OrderBy(r => r.Name);
        return query;
    }

    private static Expression<Func<Domain.Entities.Resources.DomainResource, GetResourcesResponseDto.GetResourceDto>> GetResourceDtoSelector()
    {
        return r => new GetResourcesResponseDto.GetResourceDto
        {
            Id = r.Id,
            Name = r.Name,
        };
    }
}