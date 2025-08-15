using Application.Contracts.Features.Client.Queries.GetClient;
using Domain.Entities.DomainClients;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contracts;

namespace Application.Features.Clients.Queries.GetClient;

file sealed class GetClientHandler : IRequestHandler<GetClientQuery, GetClientResponseDto>
{
    private readonly IAppDbContext _context;

    public GetClientHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<GetClientResponseDto> Handle(GetClientQuery request, CancellationToken cancellationToken)
    {
        return await _context.DomainClients
            .Where(DomainClient.Spec.ById(request.Dto.Id))
            .Select(r => new GetClientResponseDto
            {
                Name = r.Name,
                State = r.State,
                IsUsed = r.DispatchDocuments.Any(),
                Address = r.Address
            }).SingleAsync(cancellationToken);
    }
}