using System.Linq.Expressions;
using Application.Contracts.Features.Balance.Queries.GetBalances;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contracts;

namespace Application.Features.Balance.Queries.GetBalances;

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
                .ToListAsync(cancellationToken)
        };
    }

    private IQueryable<Domain.Entities.Balances.Balance> GetQuery(GetBalancesQuery request)
    {
        var query = _context.Balances
            .AsQueryable();

        if (request.Dto.Skip.HasValue)
            query = query.Skip(request.Dto.Skip.Value);

        if (request.Dto.Take.HasValue)
            query = query.Take(request.Dto.Take.Value);

        return query.OrderBy(q => q.ResourceId);
    }

    private static Expression<Func<Domain.Entities.Balances.Balance, GetBalancesResponseDto.BalanceDto>> GetResponseDtoSelector()
    {
        return q => new GetBalancesResponseDto.BalanceDto
        {
            Id = q.Id,
            ResourceId = q.ResourceId,
            ResourceName = q.DomainResource.Name,
            MeasureUnitId = q.MeasureUnitId,
            MeasureUnitName = q.MeasureUnit.Name,
            Count = q.Count,
        };
    }
}