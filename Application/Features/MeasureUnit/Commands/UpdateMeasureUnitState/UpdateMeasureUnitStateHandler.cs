using Application.Contracts.Features.MeasureUnit.Commands.UpdateMeasureUnitState;
using Domain.Entities.MeasureUnits;
using Domain.Entities.MeasureUnits.Parameters;
using Domain.Enums;
using MediatR;
using Persistence.Contracts;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.MeasureUnit.Commands.UpdateMeasureUnitState;

file sealed class UpdateMeasureUnitStateHandler : IRequestHandler<UpdateMeasureUnitStateCommand>
{
    private readonly IAppDbContext _context;

    public UpdateMeasureUnitStateHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateMeasureUnitStateCommand request, CancellationToken cancellationToken)
    {
        var resource = await _context.MeasureUnits
            .SingleAsync(Domain.Entities.MeasureUnits.MeasureUnit.Spec.ById(request.Dto.Id), cancellationToken);

        var parameters = new ChangeMeasureUnitStateParameters
        {
            StateType = request.Dto.StateType
        };
        resource.ChangeState(parameters);

        _context.MeasureUnits.Update(resource);
        await _context.SaveChangesAsync(cancellationToken);
    }
}