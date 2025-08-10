namespace Application.Contracts.Features.Balance.Queries.GetBalances;

public sealed class GetBalancesResponseDto
{
    public required List<BalanceDto> Balances { get; init; }

    public sealed class BalanceDto
    {
        public required int Id { get; init; }
        public required int ResourceId { get; init; }
        public required string ResourceName { get; init; }
        public required int MeasureUnitId { get; init; }
        public required string MeasureUnitName { get; init; }
        public required int Count { get; init; }
    }
}