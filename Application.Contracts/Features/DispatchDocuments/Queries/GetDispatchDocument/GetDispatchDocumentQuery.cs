using MediatR;

namespace Application.Contracts.Features.DispatchDocuments.Queries.GetDispatchDocument;

public sealed record GetDispatchDocumentQuery(GetDispatchDocumentRequestDto Dto) : IRequest<GetDispatchDocumentResponseDto>;