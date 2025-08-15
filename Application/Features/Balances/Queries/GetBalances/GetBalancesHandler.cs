using System.Linq.Expressions;
using Application.Contracts.Features.Balance.Queries.GetBalances;
using Domain.Entities.Balances;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contracts;

namespace Application.Features.Balances.Queries.GetBalances;

file sealed class GetBalancesHandler : IRequestHandler<GetBalancesQuery, GetBalancesResponseDto>
{
    private readonly IAppDbContext _context;

    public GetBalancesHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<GetBalancesResponseDto> Handle(GetBalancesQuery request, CancellationToken cancellationToken)
    {
        var query = GetQuery(request);

        return new GetBalancesResponseDto
        {
            Balances = await query.Select(GetResponseDtoSelector())
                .OrderBy(x => x.ResourceName)
                .ThenBy(x => x.MeasureUnitName)
                .ToListAsync(cancellationToken)
        };
    }

    private IQueryable<Balance> GetQuery(GetBalancesQuery request)
    {
        var query = _context.Balances
            .Where(Balance.Spec.ByPositiveCount())
            .AsQueryable();

        if (request.Dto.ResourceFilterIds.Count != 0)
            query = query.Where(Balance.Spec.ByResourcesContains(request.Dto.ResourceFilterIds));

        if (request.Dto.MeasureUnitFilterIds.Count != 0)
            query = query.Where(Balance.Spec.ByMeasureUnitsContains(request.Dto.MeasureUnitFilterIds));

        if (request.Dto.Skip.HasValue)
            query = query.Skip(request.Dto.Skip.Value);

        if (request.Dto.Take.HasValue)
            query = query.Take(request.Dto.Take.Value);

        return query.OrderBy(q => q.DomainResourceId);
    }

    private static Expression<Func<Balance, GetBalancesResponseDto.BalanceDto>> GetResponseDtoSelector()
    {
        return q => new GetBalancesResponseDto.BalanceDto
        {
            Id = q.Id,
            ResourceId = q.DomainResourceId,
            ResourceName = q.DomainResource.Name,
            MeasureUnitId = q.MeasureUnitId,
            MeasureUnitName = q.MeasureUnit.Name,
            Count = q.Count,
        };
    }
}