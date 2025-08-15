using Application.Contracts.Features.MeasureUnit.Commands.UpdateMeasureUnit;
using Domain.Entities.MeasureUnits;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Persistence.Contracts;

namespace Application.Features.MeasureUnits.Commands.UpdateMeasureUnit;

public sealed class UpdateMeasureUnitValidator : AbstractValidator<UpdateMeasureUnitCommand>
{
    private readonly IAppDbContext _context;

    public UpdateMeasureUnitValidator(IAppDbContext context)
    {
        _context = context;
        
        RuleFor(x => x.Dto)
            .MustAsync(IsNewNameUniqueAsync)
            .WithMessage("Такое название уже используется");

        RuleFor(x => x.Dto.Name)
            .NotEmpty()
            .WithMessage("Название не должно быть пустым");
    }

    private Task<bool> IsNewNameUniqueAsync(UpdateMeasureUnitRequestDto dto, CancellationToken cancellationToken)
    {
        return _context.MeasureUnits
            .Where(!MeasureUnit.Spec.ById(dto.Id))
            .AllAsync(!MeasureUnit.Spec.ByName(dto.Name), cancellationToken);
    }
}