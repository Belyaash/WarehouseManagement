using Application.Contracts.Features.MeasureUnit.Commands.InsertMeasureUnit;
using Application.Contracts.Features.Client.Commands.InsertClient;
using Domain.Entities.DomainClients.Parameters;
using MediatR;
using Persistence.Contracts;

namespace Application.Features.Client.Commands.InsertClient;

file sealed class InsertClientHandler : IRequestHandler<InsertClientCommand, InsertClientResponseDto>
{
    private readonly IAppDbContext _context;

    public InsertClientHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<InsertClientResponseDto> Handle(InsertClientCommand request, CancellationToken cancellationToken)
    {
        var domainClient = CreateClient(request);

        _context.DomainClients.Add(domainClient);
        await _context.SaveChangesAsync(cancellationToken);

        return CreateResponseDto(domainClient);
    }

    private static Domain.Entities.DomainClients.DomainClient CreateClient(InsertClientCommand request)
    {
        var parameters = new CreateDomainClientParameters
        {
            Name = request.Dto.Name.Trim(),
            Address = request.Dto.Address,
        };

        return new Domain.Entities.DomainClients.DomainClient(parameters);
    }

    private static InsertClientResponseDto CreateResponseDto(Domain.Entities.DomainClients.DomainClient domainClient)
    {
        return new InsertClientResponseDto()
        {
            Id = domainClient.Id,
        };
    }
}