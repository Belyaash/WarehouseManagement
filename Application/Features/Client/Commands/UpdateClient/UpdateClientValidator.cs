using Application.Contracts.Features.Client.Commands.UpdateClient;
using Domain.Entities.DomainClients;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Persistence.Contracts;

namespace Application.Features.Client.Commands.UpdateClient;

public sealed class UpdateClientValidator : AbstractValidator<UpdateClientCommand>
{
    private readonly IAppDbContext _context;

    public UpdateClientValidator(IAppDbContext context)
    {
        _context = context;
        
        RuleFor(x => x.Dto)
            .MustAsync(IsNewNameUniqueAsync)
            .WithMessage("Такое название уже используется");
    }

    private Task<bool> IsNewNameUniqueAsync(UpdateClientRequestDto dto, CancellationToken cancellationToken)
    {
        return _context.DomainClients
            .Where(!DomainClient.Spec.ById(dto.Id))
            .AnyAsync(!DomainClient.Spec.ByName(dto.Name), cancellationToken);
    }
}