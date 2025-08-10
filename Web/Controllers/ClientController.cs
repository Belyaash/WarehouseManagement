using Application.Contracts.Features.Client.Commands.DeleteClient;
using Application.Contracts.Features.Client.Commands.InsertClient;
using Application.Contracts.Features.Client.Commands.UpdateClient;
using Application.Contracts.Features.Client.Commands.UpdateClientState;
using Application.Contracts.Features.Client.Queries.GetClient;
using Application.Contracts.Features.Client.Queries.GetClients;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Route("api/client")]
public sealed class ClientController : AppController
{
    /// <summary>
    ///     Получить список ресурсов
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpGet("list")]
    public async Task<Ok<GetClientsResponseDto>> GetClients([FromQuery] GetClientsRequestDto dto)
    {
        return TypedResults.Ok(await Mediator.Send(new GetClientsQuery(dto), HttpContext.RequestAborted));
    }

    /// <summary>
    ///     Получить один ресурс
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpGet("")]
    public async Task<Ok<GetClientResponseDto>> GetClient([FromQuery] GetClientRequestDto dto)
    {
        return TypedResults.Ok(await Mediator.Send(new GetClientQuery(dto), HttpContext.RequestAborted));
    }

    /// <summary>
    ///     Создать экземпляр ресурса
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost("")]
    public async Task<Ok<InsertClientResponseDto>> InsertClient([FromBody] InsertClientRequestDto dto)
    {
        return TypedResults.Ok(await Mediator.Send(new InsertClientCommand(dto), HttpContext.RequestAborted));
    }

    /// <summary>
    ///     Удалить ресурс
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpDelete("")]
    public async Task<NoContent> DeleteClient([FromQuery] DeleteClientRequestDto dto)
    {
        await Mediator.Send(new DeleteClientCommand(dto), HttpContext.RequestAborted);
        return TypedResults.NoContent();
    }

    /// <summary>
    ///     Изменить ресурс
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPut("")]
    public async Task<NoContent> UpdateClient([FromBody] UpdateClientRequestDto dto)
    {
        await Mediator.Send(new UpdateClientCommand(dto), HttpContext.RequestAborted);
        return TypedResults.NoContent();
    }

    /// <summary>
    ///     Архивировать/Разархивировать ресурс
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPut("state")]
    public async Task<NoContent> UpdateClientState([FromBody] UpdateClientStateRequestDto dto)
    {
        await Mediator.Send(new UpdateClientStateCommand(dto), HttpContext.RequestAborted);
        return TypedResults.NoContent();
    }
}