using Application.Contracts.Features.Client.Commands.UpdateClientState;
using Domain.Entities.DomainClients;
using Domain.Entities.DomainClients.Parameters;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contracts;

namespace Application.Features.Clients.Commands.UpdateClientState;

file sealed class UpdateClientStateHandler : IRequestHandler<UpdateClientStateCommand>
{
    private readonly IAppDbContext _context;

    public UpdateClientStateHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateClientStateCommand request, CancellationToken cancellationToken)
    {
        var client = await _context.DomainClients
            .SingleAsync(DomainClient.Spec.ById(request.Dto.Id), cancellationToken);

        var parameters = new ChangeDomainClientStateParameters
        {
            StateType = request.Dto.StateType
        };
        client.ChangeState(parameters);

        _context.DomainClients.Update(client);
        await _context.SaveChangesAsync(cancellationToken);
    }
}