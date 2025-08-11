using Application.Contracts.Features.Resource.Commands.InsertResource;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Persistence.Contracts;

namespace Application.Features.MeasureUnit.Commands.InsertMeasureUnit;

public sealed class InsertResourceValidator : AbstractValidator<InsertResourceCommand>
{
    private readonly IAppDbContext _context;

    public InsertResourceValidator(IAppDbContext context)
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
        return _context.Resources
            .AnyAsync(!Domain.Entities.Resources.DomainResource.Spec.ByName(name.Trim()), cancellationToken);
    }
}