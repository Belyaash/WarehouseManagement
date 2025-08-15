using Application.Contracts.Features.MeasureUnit.Commands.UpdateMeasureUnitState;
using Domain.Entities.MeasureUnits;
using Domain.Entities.MeasureUnits.Parameters;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contracts;

namespace Application.Features.MeasureUnits.Commands.UpdateMeasureUnitState;

file sealed class UpdateMeasureUnitStateHandler : IRequestHandler<UpdateMeasureUnitStateCommand>
{
    private readonly IAppDbContext _context;

    public UpdateMeasureUnitStateHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateMeasureUnitStateCommand request, CancellationToken cancellationToken)
    {
        var measureUnit = await _context.MeasureUnits
            .SingleAsync(MeasureUnit.Spec.ById(request.Dto.Id), cancellationToken);

        var parameters = new ChangeMeasureUnitStateParameters
        {
            StateType = request.Dto.StateType
        };
        measureUnit.ChangeState(parameters);

        _context.MeasureUnits.Update(measureUnit);
        await _context.SaveChangesAsync(cancellationToken);
    }
}