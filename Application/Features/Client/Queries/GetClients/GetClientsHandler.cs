using System.Linq.Expressions;
using Application.Contracts.Features.Client.Queries.GetClients;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contracts;

namespace Application.Features.Client.Queries.GetClients;

file sealed class GetClientsHandler : IRequestHandler<GetClientsQuery, GetClientsResponseDto>
{
    private readonly IAppDbContext _context;

    public GetClientsHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<GetClientsResponseDto> Handle(GetClientsQuery request, CancellationToken cancellationToken)
    {
        var query = GetQuery(request);

        return new GetClientsResponseDto()
        {
            Clients = await query.Select(GetClientDtoSelector())
                .ToListAsync(cancellationToken)
        };
    }

    private IQueryable<Domain.Entities.DomainClients.DomainClient> GetQuery(GetClientsQuery request)
    {
        var query = _context.DomainClients
            .Where(Domain.Entities.DomainClients.DomainClient.Spec.ByState(request.Dto.State));

        if (request.Dto.Skip.HasValue)
            query = query.Skip(request.Dto.Skip.Value);

        if (request.Dto.Take.HasValue)
            query = query.Take(request.Dto.Take.Value);

        query = query.OrderBy(r => r.Name);
        return query;
    }

    private static Expression<Func<Domain.Entities.DomainClients.DomainClient, GetClientsResponseDto.GetClientDto>> GetClientDtoSelector()
    {
        return r => new GetClientsResponseDto.GetClientDto
        {
            Id = r.Id,
            Name = r.Name,
            Address = r.Address,
        };
    }
}