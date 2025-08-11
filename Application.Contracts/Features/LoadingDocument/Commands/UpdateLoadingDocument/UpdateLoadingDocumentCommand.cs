using MediatR;

namespace Application.Contracts.Features.LoadingDocument.Commands.UpdateLoadingDocument;

public sealed record UpdateLoadingDocumentCommand(UpdateLoadingDocumentRequestDto Dto) : IRequest;