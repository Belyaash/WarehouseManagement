using Application.Contracts.Features.Resource.Commands.DeleteResource;
using Application.Contracts.Features.Resource.Commands.InsertResource;
using Application.Contracts.Features.Resource.Commands.UpdateResource;
using Application.Contracts.Features.Resource.Commands.UpdateResourceState;
using Application.Contracts.Features.Resource.Queries.GetResource;
using Application.Contracts.Features.Resource.Queries.GetResources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Route("api/resource")]
public sealed class ResourceController : AppController
{
    /// <summary>
    ///     Получить список ресурсов
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpGet("list")]
    [AllowAnonymous]
    public async Task<Ok<GetResourcesResponseDto>> GetResources([FromQuery] GetResourcesRequestDto dto)
    {
        return TypedResults.Ok(await Mediator.Send(new GetResourcesQuery(dto), HttpContext.RequestAborted));
    }

    /// <summary>
    ///     Получить один ресурс
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpGet("")]
    public async Task<Ok<GetResourceResponseDto>> GetResource([FromQuery] GetResourceRequestDto dto)
    {
        return TypedResults.Ok(await Mediator.Send(new GetResourceQuery(dto), HttpContext.RequestAborted));
    }

    /// <summary>
    ///     Создать экземпляр ресурса
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost("")]
    public async Task<Ok<InsertResourceResponseDto>> InsertResource([FromBody] InsertResourceRequestDto dto)
    {
        return TypedResults.Ok(await Mediator.Send(new InsertResourceCommand(dto), HttpContext.RequestAborted));
    }

    /// <summary>
    ///     Удалить ресурс
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpDelete("")]
    public async Task<NoContent> DeleteResource([FromQuery] DeleteResourceRequestDto dto)
    {
        await Mediator.Send(new DeleteResourceCommand(dto), HttpContext.RequestAborted);
        return TypedResults.NoContent();
    }

    /// <summary>
    ///     Изменить ресурс
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPut("")]
    public async Task<NoContent> UpdateResource([FromBody] UpdateResourceRequestDto dto)
    {
        await Mediator.Send(new UpdateResourceCommand(dto), HttpContext.RequestAborted);
        return TypedResults.NoContent();
    }

    /// <summary>
    ///     Архивировать/Разархивировать ресурс
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPut("state")]
    public async Task<NoContent> UpdateResourceState([FromBody] UpdateResourceStateRequestDto dto)
    {
        await Mediator.Send(new UpdateResourceStateCommand(dto), HttpContext.RequestAborted);
        return TypedResults.NoContent();
    }
}