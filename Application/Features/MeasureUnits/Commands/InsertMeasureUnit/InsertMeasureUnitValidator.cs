using Application.Contracts.Features.MeasureUnit.Commands.InsertMeasureUnit;
using Domain.Entities.MeasureUnits;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Persistence.Contracts;

namespace Application.Features.MeasureUnits.Commands.InsertMeasureUnit;

public sealed class InsertMeasureUnitValidator : AbstractValidator<InsertMeasureUnitCommand>
{
    private readonly IAppDbContext _context;

    public InsertMeasureUnitValidator(IAppDbContext context)
    {
        _context = context;

        RuleFor(c => c.Dto.Name)
            .MustAsync(IsValueUniqueAsync)
            .WithMessage("Единица измерения с таким названием уже существует");

        RuleFor(x => x.Dto.Name)
            .NotEmpty()
            .WithMessage("Название не должно быть пустым");
    }

    private Task<bool> IsValueUniqueAsync(string name, CancellationToken cancellationToken)
    {
        return _context.MeasureUnits
            .AllAsync(!MeasureUnit.Spec.ByName(name.Trim()), cancellationToken);
    }
}