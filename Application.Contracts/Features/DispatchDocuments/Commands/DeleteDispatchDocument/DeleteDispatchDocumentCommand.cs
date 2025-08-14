using MediatR;

namespace Application.Contracts.Features.DispatchDocuments.Commands.DeleteDispatchDocument;

public sealed record DeleteDispatchDocumentCommand(DeleteDispatchDocumentRequestDto Dto) : IRequest;