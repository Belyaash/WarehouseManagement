namespace Application.Contracts.Features.Client.Commands.UpdateClient;

public sealed class UpdateClientRequestDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string Address { get; init; }
}