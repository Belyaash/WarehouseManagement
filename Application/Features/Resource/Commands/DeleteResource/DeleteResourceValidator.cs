using Application.Contracts.Features.Resource.Commands.DeleteResource;
using Domain.Entities.Resources;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Persistence.Contracts;

namespace Application.Features.Resource.Commands.DeleteResource;

public sealed class DeleteResourceValidator : AbstractValidator<DeleteResourceCommand>
{
    private readonly IAppDbContext _context;

    public DeleteResourceValidator(IAppDbContext context)
    {
        _context = context;
        
        RuleFor(c => c.Dto.Id)
            .MustAsync(CanBeDeletedAsync)
            .WithMessage("Ресурс не может быть удалён, так как уже используется");
    }

    private async Task<bool> CanBeDeletedAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Resources
            .AnyAsync(DomainResource.Spec.ById(id)
                      &&
                      DomainResource.Spec.CanBeDeleted(), cancellationToken);
    }
}