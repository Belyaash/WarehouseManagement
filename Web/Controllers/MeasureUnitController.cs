using Application.Contracts.Features.MeasureUnit.Commands.DeleteMeasureUnit;
using Application.Contracts.Features.MeasureUnit.Commands.InsertMeasureUnit;
using Application.Contracts.Features.MeasureUnit.Commands.UpdateMeasureUnit;
using Application.Contracts.Features.MeasureUnit.Commands.UpdateMeasureUnitState;
using Application.Contracts.Features.MeasureUnit.Queries.GetMeasureUnit;
using Application.Contracts.Features.MeasureUnit.Queries.GetMeasureUnits;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Route("api/measure-unit")]
public sealed class MeasureUnitController : AppController
{
    /// <summary>
    ///     Получить список ресурсов
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpGet("list")]
    public async Task<Ok<GetMeasureUnitsResponseDto>> GetMeasureUnits([FromQuery] GetMeasureUnitsRequestDto dto)
    {
        return TypedResults.Ok(await Mediator.Send(new GetMeasureUnitsQuery(dto), HttpContext.RequestAborted));
    }

    /// <summary>
    ///     Получить один ресурс
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpGet("")]
    public async Task<Ok<GetMeasureUnitResponseDto>> GetMeasureUnit([FromQuery] GetMeasureUnitRequestDto dto)
    {
        return TypedResults.Ok(await Mediator.Send(new GetMeasureUnitQuery(dto), HttpContext.RequestAborted));
    }

    /// <summary>
    ///     Создать экземпляр ресурса
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost("")]
    public async Task<Ok<InsertMeasureUnitResponseDto>> InsertMeasureUnit([FromBody] InsertMeasureUnitRequestDto dto)
    {
        return TypedResults.Ok(await Mediator.Send(new InsertMeasureUnitCommand(dto), HttpContext.RequestAborted));
    }

    /// <summary>
    ///     Удалить ресурс
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpDelete("")]
    public async Task<NoContent> DeleteMeasureUnit([FromQuery] DeleteMeasureUnitRequestDto dto)
    {
        await Mediator.Send(new DeleteMeasureUnitCommand(dto), HttpContext.RequestAborted);
        return TypedResults.NoContent();
    }

    /// <summary>
    ///     Изменить ресурс
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPut("")]
    public async Task<NoContent> UpdateMeasureUnit([FromBody] UpdateMeasureUnitRequestDto dto)
    {
        await Mediator.Send(new UpdateMeasureUnitCommand(dto), HttpContext.RequestAborted);
        return TypedResults.NoContent();
    }

    /// <summary>
    ///     Архивировать/Разархивировать ресурс
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPut("state")]
    public async Task<NoContent> UpdateMeasureUnitState([FromBody] UpdateMeasureUnitStateRequestDto dto)
    {
        await Mediator.Send(new UpdateMeasureUnitStateCommand(dto), HttpContext.RequestAborted);
        return TypedResults.NoContent();
    }
}