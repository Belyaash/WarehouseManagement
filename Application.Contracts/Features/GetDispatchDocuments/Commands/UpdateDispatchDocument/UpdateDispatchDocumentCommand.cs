using MediatR;

namespace Application.Contracts.Features.GetDispatchDocuments.Commands.UpdateDispatchDocument;

public sealed record UpdateDispatchDocumentCommand(UpdateDispatchDocumentRequestDto Dto) : IRequest;