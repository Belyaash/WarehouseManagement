using System.Linq.Expressions;
using Application.Contracts.Features.Client.Queries.GetClients;
using Domain.Entities.DomainClients;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contracts;

namespace Application.Features.Clients.Queries.GetClients;

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

    private IQueryable<DomainClient> GetQuery(GetClientsQuery request)
    {
        var query = _context.DomainClients
            .Where(DomainClient.Spec.ByState(request.Dto.State));

        if (request.Dto.Skip.HasValue)
            query = query.Skip(request.Dto.Skip.Value);

        if (request.Dto.Take.HasValue)
            query = query.Take(request.Dto.Take.Value);

        query = query.OrderBy(r => r.Name);
        return query;
    }

    private static Expression<Func<DomainClient, GetClientsResponseDto.GetClientDto>> GetClientDtoSelector()
    {
        return r => new GetClientsResponseDto.GetClientDto
        {
            Id = r.Id,
            Name = r.Name,
            Address = r.Address,
        };
    }
}