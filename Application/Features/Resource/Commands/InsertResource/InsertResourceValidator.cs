using Application.Contracts.Features.Resource.Commands.InsertResource;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Persistence.Contracts;

namespace Application.Features.Resource.Commands.InsertResource;

public sealed class InsertResourceValidator : AbstractValidator<InsertResourceCommand>
{
    private readonly IAppDbContext _context;

    public InsertResourceValidator(IAppDbContext context)
    {
        _context = context;

        RuleFor(c => c.Dto.Name)
            .MustAsync(IsValueUniqueAsync)
            .WithMessage("Ресурс с таким названием уже существует");
    }

    private Task<bool> IsValueUniqueAsync(string name, CancellationToken cancellationToken)
    {
        return _context.Resources
            .AnyAsync(!Domain.Entities.Resources.DomainResource.Spec.ByName(name.Trim()), cancellationToken);
    }
}