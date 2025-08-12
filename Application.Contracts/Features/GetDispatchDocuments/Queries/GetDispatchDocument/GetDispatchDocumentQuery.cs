using MediatR;

namespace Application.Contracts.Features.GetDispatchDocuments.Queries.GetDispatchDocument;

public sealed record GetDispatchDocumentQuery(GetDispatchDocumentRequestDto Dto) : IRequest<GetDispatchDocumentResponseDto>;