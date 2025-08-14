using MediatR;

namespace Application.Contracts.Features.DispatchDocuments.Commands.InsertDispatchDocument;

public sealed record InsertDispatchDocumentCommand(InsertDispatchDocumentRequestDto Dto) : IRequest<InsertDispatchDocumentResponseDto>;