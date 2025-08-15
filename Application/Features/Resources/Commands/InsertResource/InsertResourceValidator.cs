using Application.Contracts.Features.Resource.Commands.InsertResource;
using Domain.Entities.Resources;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Persistence.Contracts;

namespace Application.Features.Resources.Commands.InsertResource;

public sealed class InsertResourceValidator : AbstractValidator<InsertResourceCommand>
{
    private readonly IAppDbContext _context;

    public InsertResourceValidator(IAppDbContext context)
    {
        _context = context;

        RuleFor(c => c.Dto.Name)
            .MustAsync(IsValueUniqueAsync)
            .WithMessage("Ресурс с таким названием уже существует");

        RuleFor(x => x.Dto.Name)
            .NotEmpty()
            .WithMessage("Название не должно быть пустым");
    }

    private Task<bool> IsValueUniqueAsync(string name, CancellationToken cancellationToken)
    {
        return _context.Resources
            .AllAsync(!DomainResource.Spec.ByName(name.Trim()), cancellationToken);
    }
}