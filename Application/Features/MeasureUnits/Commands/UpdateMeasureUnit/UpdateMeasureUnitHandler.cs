using Application.Contracts.Features.MeasureUnit.Commands.UpdateMeasureUnit;
using Domain.Entities.MeasureUnits;
using Domain.Entities.MeasureUnits.Parameters;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contracts;

namespace Application.Features.MeasureUnits.Commands.UpdateMeasureUnit;

file sealed class UpdateMeasureUnitHandler : IRequestHandler<UpdateMeasureUnitCommand>
{
    private readonly IAppDbContext _context;

    public UpdateMeasureUnitHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateMeasureUnitCommand request, CancellationToken cancellationToken)
    {
        var measureUnit = await _context.MeasureUnits
            .SingleAsync(MeasureUnit.Spec.ById(request.Dto.Id), cancellationToken);

        var parameters = new UpdateMeasureUnitParameters
        {
            Name = request.Dto.Name.Trim()
        };
        measureUnit.Update(parameters);

        _context.MeasureUnits.Update(measureUnit);
        await _context.SaveChangesAsync(cancellationToken);
    }
}