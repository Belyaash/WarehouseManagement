using MediatR;

namespace Application.Contracts.Features.LoadingDocument.Commands.DeleteLoadingDocument;

public sealed record DeleteLoadingDocumentCommand(DeleteLoadingDocumentRequestDto Dto) : IRequest;