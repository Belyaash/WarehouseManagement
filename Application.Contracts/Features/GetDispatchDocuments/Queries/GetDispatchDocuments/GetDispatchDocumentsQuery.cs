using MediatR;

namespace Application.Contracts.Features.GetDispatchDocuments.Queries.GetDispatchDocuments;

public sealed record GetDispatchDocumentsQuery(GetDispatchDocumentsRequestDto Dto) : IRequest<GetDispatchDocumentsResponseDto>;