using Application.Contracts.Features.Resource.Commands.UpdateResource;
using Domain.Entities.Resources;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Persistence.Contracts;

namespace Application.Features.Resource.Commands.UpdateResource;

public sealed class UpdateResourceValidator : AbstractValidator<UpdateResourceCommand>
{
    private readonly IAppDbContext _context;

    public UpdateResourceValidator(IAppDbContext context)
    {
        _context = context;
        
        RuleFor(x => x.Dto)
            .MustAsync(IsNewNameUniqueAsync)
            .WithMessage("Такое название уже используется");
    }

    private Task<bool> IsNewNameUniqueAsync(UpdateResourceRequestDto dto, CancellationToken cancellationToken)
    {
        return _context.Resources
            .Where(!DomainResource.Spec.ById(dto.Id))
            .AnyAsync(!DomainResource.Spec.ByName(dto.Name), cancellationToken);
    }
}