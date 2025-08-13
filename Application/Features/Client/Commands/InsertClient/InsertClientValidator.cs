using Application.Contracts.Features.Client.Commands.InsertClient;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Persistence.Contracts;

namespace Application.Features.Client.Commands.InsertClient;

public sealed class InsertClientValidator : AbstractValidator<InsertClientCommand>
{
    private readonly IAppDbContext _context;

    public InsertClientValidator(IAppDbContext context)
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
        return _context.DomainClients
            .AllAsync(!Domain.Entities.DomainClients.DomainClient.Spec.ByName(name.Trim()), cancellationToken);
    }
}