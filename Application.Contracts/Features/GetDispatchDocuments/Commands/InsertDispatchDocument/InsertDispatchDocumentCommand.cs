using MediatR;

namespace Application.Contracts.Features.GetDispatchDocuments.Commands.InsertDispatchDocument;

public sealed record InsertDispatchDocumentCommand(InsertDispatchDocumentRequestDto Dto) : IRequest<InsertDispatchDocumentResponseDto>;