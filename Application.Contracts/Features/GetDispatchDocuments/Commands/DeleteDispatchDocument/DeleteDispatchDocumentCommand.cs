using MediatR;

namespace Application.Contracts.Features.GetDispatchDocuments.Commands.DeleteDispatchDocument;

public sealed record DeleteDispatchDocumentCommand(DeleteDispatchDocumentRequestDto Dto) : IRequest;