using Application.Contracts.Features.MeasureUnit.Commands.DeleteMeasureUnit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contracts;

namespace Application.Features.MeasureUnit.Commands.DeleteMeasureUnit;

file sealed class DeleteMeasureUnitHandler : IRequestHandler<DeleteMeasureUnitCommand>
{
    private readonly IAppDbContext _context;

    public DeleteMeasureUnitHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteMeasureUnitCommand request, CancellationToken cancellationToken)
    {
        var measureUnit = await _context.MeasureUnits
            .SingleAsync(Domain.Entities.MeasureUnits.MeasureUnit.Spec.ById(request.Dto.Id), cancellationToken);

        _context.MeasureUnits.Remove(measureUnit);
        await _context.SaveChangesAsync(cancellationToken);
    }
}