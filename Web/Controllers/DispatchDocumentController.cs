using Application.Contracts.Features.DispatchDocuments.Commands.DeleteDispatchDocument;
using Application.Contracts.Features.DispatchDocuments.Commands.InsertDispatchDocument;
using Application.Contracts.Features.DispatchDocuments.Commands.UpdateDispatchDocument;
using Application.Contracts.Features.DispatchDocuments.Queries.GetDispatchDocument;
using Application.Contracts.Features.DispatchDocuments.Queries.GetDispatchDocumentNumbers;
using Application.Contracts.Features.DispatchDocuments.Queries.GetDispatchDocuments;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Route("api/dispatch")]
public sealed class DispatchDocumentController : AppController
{
    /// <summary>
    ///     Получить список документов отгрузки
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost("list")]
    public async Task<Ok<GetDispatchDocumentsResponseDto>> GetDispatchDocuments([FromBody] GetDispatchDocumentsRequestDto dto)
    {
        return TypedResults.Ok(await Mediator.Send(new GetDispatchDocumentsQuery(dto), HttpContext.RequestAborted));
    }

    /// <summary>
    ///     Получить список номеров документов
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpGet("numbers/list")]
    public async Task<Ok<GetDispatchDocumentNumbersResponseDto>> GetDispatchDocumentNumbers([FromQuery] GetDispatchDocumentNumbersRequestDto dto)
    {
        return TypedResults.Ok(await Mediator.Send(new GetDispatchDocumentNumbersQuery(dto), HttpContext.RequestAborted));
    }

    /// <summary>
    ///     Получить документ отгрузки
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpGet("")]
    public async Task<Ok<GetDispatchDocumentResponseDto>> GetDispatchDocument([FromQuery] GetDispatchDocumentRequestDto dto)
    {
        return TypedResults.Ok(await Mediator.Send(new GetDispatchDocumentQuery(dto), HttpContext.RequestAborted));
    }

    /// <summary>
    ///     Создать документ отгрузки
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost("")]
    public async Task<Ok<InsertDispatchDocumentResponseDto>> InsertDispatchDocument([FromBody] InsertDispatchDocumentRequestDto dto)
    {
        return TypedResults.Ok(await Mediator.Send(new InsertDispatchDocumentCommand(dto), HttpContext.RequestAborted));
    }

    /// <summary>
    ///     Изменить документ погрузки
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPut("")]
    public async Task<NoContent> UpdateDispatchDocument([FromBody] UpdateDispatchDocumentRequestDto dto)
    {
        await Mediator.Send(new UpdateDispatchDocumentCommand(dto), HttpContext.RequestAborted);
        return TypedResults.NoContent();
    }

    /// <summary>
    ///     Удалить документ погрузки
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpDelete("")]
    public async Task<NoContent> DeleteDispatchDocument([FromQuery] DeleteDispatchDocumentRequestDto dto)
    {
        await Mediator.Send(new DeleteDispatchDocumentCommand(dto), HttpContext.RequestAborted);
        return TypedResults.NoContent();
    }
}