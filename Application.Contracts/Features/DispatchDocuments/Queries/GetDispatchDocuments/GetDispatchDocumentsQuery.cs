using MediatR;

namespace Application.Contracts.Features.DispatchDocuments.Queries.GetDispatchDocuments;

public sealed record GetDispatchDocumentsQuery(GetDispatchDocumentsRequestDto Dto) : IRequest<GetDispatchDocumentsResponseDto>;