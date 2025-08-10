namespace Application.Contracts.Features.Balance.Queries.GetBalances;

public sealed class GetBalancesRequestDto
{
    public int? Skip { get; init; }
    public int? Take { get; init; }
    public List<int> ResourceFilterIds { get; init; } = new();
    public List<int> MeasureUnitFilterIds { get; init; } = new();
}