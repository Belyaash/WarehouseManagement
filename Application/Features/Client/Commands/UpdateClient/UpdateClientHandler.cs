using Application.Contracts.Features.Client.Commands.UpdateClient;
using Domain.Entities.DomainClients;
using Domain.Entities.DomainClients.Parameters;
using MediatR;
using Persistence.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Client.Commands.UpdateClient;

file sealed class UpdateClientHandler : IRequestHandler<UpdateClientCommand>
{
    private readonly IAppDbContext _context;

    public UpdateClientHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateClientCommand request, CancellationToken cancellationToken)
    {
        var client = await _context.DomainClients
            .SingleAsync(DomainClient.Spec.ById(request.Dto.Id), cancellationToken);

        var parameters = new UpdateDomainClientParameters
        {
            Name = request.Dto.Name.Trim(),
            Address = request.Dto.Address,
        };
        client.Update(parameters);

        _context.DomainClients.Update(client);
        await _context.SaveChangesAsync(cancellationToken);
    }
}