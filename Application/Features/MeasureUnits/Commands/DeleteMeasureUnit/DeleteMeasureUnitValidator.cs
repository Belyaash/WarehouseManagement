using Application.Contracts.Features.MeasureUnit.Commands.DeleteMeasureUnit;
using Domain.Entities.MeasureUnits;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Persistence.Contracts;

namespace Application.Features.MeasureUnits.Commands.DeleteMeasureUnit;

public sealed class DeleteMeasureUnitValidator : AbstractValidator<DeleteMeasureUnitCommand>
{
    private readonly IAppDbContext _context;

    public DeleteMeasureUnitValidator(IAppDbContext context)
    {
        _context = context;
        
        RuleFor(c => c.Dto.Id)
            .MustAsync(CanBeDeletedAsync)
            .WithMessage("Единица измерения не может быть удалена, так как уже используется");
    }

    private Task<bool> CanBeDeletedAsync(int id, CancellationToken cancellationToken)
    {
        return _context.MeasureUnits
            .AnyAsync(MeasureUnit.Spec.ById(id)
                      &&
                      MeasureUnit.Spec.CanBeDeleted(), cancellationToken);
    }
}