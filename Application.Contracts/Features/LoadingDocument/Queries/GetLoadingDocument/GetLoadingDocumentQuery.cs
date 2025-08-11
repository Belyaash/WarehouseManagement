using MediatR;

namespace Application.Contracts.Features.LoadingDocument.Queries.GetLoadingDocument;

public sealed record GetLoadingDocumentQuery(GetLoadingDocumentRequestDto Dto) : IRequest<GetLoadingDocumentResponseDto>;