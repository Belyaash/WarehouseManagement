using Application.Contracts.Features.LoadingDocument.Commands.DeleteLoadingDocument;
using Application.Contracts.Features.LoadingDocument.Commands.InsertLoadingDocument;
using Application.Contracts.Features.LoadingDocument.Commands.UpdateLoadingDocument;
using Application.Contracts.Features.LoadingDocument.Queries.GetLoadingDocument;
using Application.Contracts.Features.LoadingDocument.Queries.GetLoadingDocuments;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Route("api/loading")]
public sealed class LoadingDocumentController : AppController
{
    /// <summary>
    ///     Получить список документов погрузки
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpGet("list")]
    public async Task<Ok<GetLoadingDocumentsResponseDto>> GetLoadingDocuments([FromQuery] GetLoadingDocumentsRequestDto dto)
    {
        return TypedResults.Ok(await Mediator.Send(new GetLoadingDocumentsQuery(dto), HttpContext.RequestAborted));
    }

    /// <summary>
    ///     Получить документ погрузки
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpGet("")]
    public async Task<Ok<GetLoadingDocumentResponseDto>> GetLoadingDocument([FromQuery] GetLoadingDocumentRequestDto dto)
    {
        return TypedResults.Ok(await Mediator.Send(new GetLoadingDocumentQuery(dto), HttpContext.RequestAborted));
    }

    /// <summary>
    ///     Создать документ погрузки
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPost("")]
    public async Task<Ok<InsertLoadingDocumentResponseDto>> InsertLoadingDocument([FromBody] InsertLoadingDocumentRequestDto dto)
    {
        return TypedResults.Ok(await Mediator.Send(new InsertLoadingDocumentCommand(dto), HttpContext.RequestAborted));
    }

    /// <summary>
    ///     Изменить документ погрузки
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpPut("")]
    public async Task<NoContent> UpdateLoadingDocument([FromBody] UpdateLoadingDocumentRequestDto dto)
    {
        await Mediator.Send(new UpdateLoadingDocumentCommand(dto), HttpContext.RequestAborted);
        return TypedResults.NoContent();
    }

    /// <summary>
    ///     Удалить документ погрузки
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    [HttpDelete("")]
    public async Task<NoContent> DeleteLoadingDocument([FromQuery] DeleteLoadingDocumentRequestDto dto)
    {
        await Mediator.Send(new DeleteLoadingDocumentCommand(dto), HttpContext.RequestAborted);
        return TypedResults.NoContent();
    }
}