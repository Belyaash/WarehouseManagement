using Application.Contracts.Features.Balance.Queries.GetBalances;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Microsoft.AspNetCore.Components.Route("api/balance")]
public sealed class BalanceController : AppController
{
    /// <summary>
    ///     Получить список балансов
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpGet("list")]
    public async Task<Ok<GetBalancesResponseDto>> GetBalances([FromQuery] GetBalancesRequestDto dto)
    {
        return TypedResults.Ok(await Mediator.Send(new GetBalancesQuery(dto), HttpContext.RequestAborted));
    }
}