using Application.Contracts.Features.MeasureUnit.Commands.UpdateMeasureUnit;
using Domain.Entities.MeasureUnits.Parameters;
using MediatR;
using Persistence.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.MeasureUnit.Commands.UpdateMeasureUnit;

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
            .SingleAsync(Domain.Entities.MeasureUnits.MeasureUnit.Spec.ById(request.Dto.Id), cancellationToken);

        var parameters = new UpdateMeasureUnitParameters
        {
            Name = request.Dto.Name.Trim()
        };
        measureUnit.Update(parameters);

        _context.MeasureUnits.Update(measureUnit);
        await _context.SaveChangesAsync(cancellationToken);
    }
}