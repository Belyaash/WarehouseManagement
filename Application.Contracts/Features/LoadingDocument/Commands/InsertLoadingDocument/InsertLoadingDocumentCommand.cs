using Application.Contracts.Features.LoadingDocument.Queries.GetLoadingDocuments;
using MediatR;

namespace Application.Contracts.Features.LoadingDocument.Commands.InsertLoadingDocument;

public sealed record InsertLoadingDocumentCommand(InsertLoadingDocumentRequestDto Dto) : IRequest<InsertLoadingDocumentResponseDto>;