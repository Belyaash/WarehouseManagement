using MediatR;

namespace Application.Contracts.Features.LoadingDocument.Queries.GetLoadingDocuments;

public sealed record GetLoadingDocumentsQuery(GetLoadingDocumentsRequestDto Dto) : IRequest<GetLoadingDocumentsResponseDto>;