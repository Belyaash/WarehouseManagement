using Application.Contracts.Features.MeasureUnit.Commands.UpdateMeasureUnit;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Persistence.Contracts;

namespace Application.Features.MeasureUnit.Commands.UpdateMeasureUnit;

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
            .Where(!Domain.Entities.MeasureUnits.MeasureUnit.Spec.ById(dto.Id))
            .AllAsync(!Domain.Entities.MeasureUnits.MeasureUnit.Spec.ByName(dto.Name), cancellationToken);
    }
}