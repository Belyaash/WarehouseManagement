using Application.Contracts.Features.Client.Commands.DeleteClient;
using Domain.Entities.DomainClients;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contracts;

namespace Application.Features.Client.Commands.DeleteClient;

file sealed class DeleteClientHandler : IRequestHandler<DeleteClientCommand>
{
    private readonly IAppDbContext _context;

    public DeleteClientHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteClientCommand request, CancellationToken cancellationToken)
    {
        var domainClient = await _context.DomainClients
            .SingleAsync(DomainClient.Spec.ById(request.Dto.Id), cancellationToken);

        _context.DomainClients.Remove(domainClient);
        await _context.SaveChangesAsync(cancellationToken);
    }
}