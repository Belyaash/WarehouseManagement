using Application.Contracts.Features.Balance.Queries.GetBalances;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Route("api/balance")]
public sealed class BalanceController : AppController
{
    /// <summary>
    ///     Получить список балансов
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost("list")]
    public async Task<Ok<GetBalancesResponseDto>> GetBalances([FromBody] GetBalancesRequestDto dto)
    {
        return TypedResults.Ok(await Mediator.Send(new GetBalancesQuery(dto), HttpContext.RequestAborted));
    }
}