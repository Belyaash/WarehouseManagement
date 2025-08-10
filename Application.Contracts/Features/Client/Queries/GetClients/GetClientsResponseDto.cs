namespace Application.Contracts.Features.Client.Queries.GetClients;

public sealed class GetClientsResponseDto
{
    public required List<GetClientDto> Clients { get; init; }
    public class GetClientDto
    {
        public required int Id { get; init; }
        public required string Name { get; init; }
        public required string Address { get; init; }
    }
}