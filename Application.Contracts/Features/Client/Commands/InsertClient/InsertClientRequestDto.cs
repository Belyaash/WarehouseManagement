namespace Application.Contracts.Features.Client.Commands.InsertClient;

public sealed class InsertClientRequestDto
{
    public required string Name { get; init; }
    public required string Address { get; init; }
}