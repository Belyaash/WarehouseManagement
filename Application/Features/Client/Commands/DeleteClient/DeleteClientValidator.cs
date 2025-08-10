using Application.Contracts.Features.Client.Commands.DeleteClient;
using Domain.Entities.DomainClients;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Persistence.Contracts;

namespace Application.Features.Client.Commands.DeleteClient;

public sealed class DeleteClientValidator : AbstractValidator<DeleteClientCommand>
{
    private readonly IAppDbContext _context;

    public DeleteClientValidator(IAppDbContext context)
    {
        _context = context;
        
        RuleFor(c => c.Dto.Id)
            .MustAsync(CanBeDeletedAsync)
            .WithMessage("Клиент не может быть удалён, так как уже используется");
    }

    private Task<bool> CanBeDeletedAsync(int id, CancellationToken cancellationToken)
    {
        return _context.DomainClients
            .AnyAsync(DomainClient.Spec.ById(id)
                      &&
                      DomainClient.Spec.CanBeDeleted(), cancellationToken);
    }
}