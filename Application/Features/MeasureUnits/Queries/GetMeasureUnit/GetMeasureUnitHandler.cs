using Application.Contracts.Features.MeasureUnit.Queries.GetMeasureUnit;
using Domain.Entities.MeasureUnits;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contracts;

namespace Application.Features.MeasureUnits.Queries.GetMeasureUnit;

file sealed class GetMeasureUnitHandler : IRequestHandler<GetMeasureUnitQuery, GetMeasureUnitResponseDto>
{
    private readonly IAppDbContext _context;

    public GetMeasureUnitHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<GetMeasureUnitResponseDto> Handle(GetMeasureUnitQuery request, CancellationToken cancellationToken)
    {
        return await _context.MeasureUnits
            .Where(MeasureUnit.Spec.ById(request.Dto.Id))
            .Select(r => new GetMeasureUnitResponseDto
            {
                Name = r.Name,
                State = r.State,
                IsUsed = r.Balances.Any() || r.DispatchDocumentResources.Any() || r.LoadingDocumentResources.Any()
            }).SingleAsync(cancellationToken);
    }
}