using System.Linq.Expressions;
using Application.Contracts.Features.MeasureUnit.Queries.GetMeasureUnits;
using Domain.Entities.MeasureUnits;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contracts;

namespace Application.Features.MeasureUnits.Queries.GetMeasureUnits;

file sealed class GetMeasureUnitsHandler : IRequestHandler<GetMeasureUnitsQuery, GetMeasureUnitsResponseDto>
{
    private readonly IAppDbContext _context;

    public GetMeasureUnitsHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<GetMeasureUnitsResponseDto> Handle(GetMeasureUnitsQuery request, CancellationToken cancellationToken)
    {
        var query = GetQuery(request);

        return new GetMeasureUnitsResponseDto()
        {
            MeasureUnits = await query.Select(GetMeasureUnitDtoSelector())
                .ToListAsync(cancellationToken)
        };
    }

    private IQueryable<MeasureUnit> GetQuery(GetMeasureUnitsQuery request)
    {
        var query = _context.MeasureUnits
            .Where(MeasureUnit.Spec.ByState(request.Dto.State));

        if (request.Dto.Skip.HasValue)
            query = query.Skip(request.Dto.Skip.Value);

        if (request.Dto.Take.HasValue)
            query = query.Take(request.Dto.Take.Value);

        query = query.OrderBy(r => r.Name);
        return query;
    }

    private static Expression<Func<MeasureUnit, GetMeasureUnitsResponseDto.GetMeasureUnitDto>> GetMeasureUnitDtoSelector()
    {
        return r => new GetMeasureUnitsResponseDto.GetMeasureUnitDto
        {
            Id = r.Id,
            Name = r.Name,
        };
    }
}