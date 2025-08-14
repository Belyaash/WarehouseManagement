using MediatR;

namespace Application.Contracts.Features.DispatchDocuments.Commands.UpdateDispatchDocument;

public sealed record UpdateDispatchDocumentCommand(UpdateDispatchDocumentRequestDto Dto) : IRequest;