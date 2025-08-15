using Application.Contracts.Features.LoadingDocument.Queries.GetLoadingDocumentNumbers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contracts;

namespace Application.Features.LoadingDocuments.Queries.GetLoadingDocumentNumbers;

file sealed class GetLoadingDocumentNumbersHandler : IRequestHandler<GetLoadingDocumentNumbersQuery, GetLoadingDocumentNumbersResponseDto>
{
    private readonly IAppDbContext _context;

    public GetLoadingDocumentNumbersHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<GetLoadingDocumentNumbersResponseDto> Handle(GetLoadingDocumentNumbersQuery request, CancellationToken cancellationToken)
    {
        return new GetLoadingDocumentNumbersResponseDto()
        {
            DocumentNumbers = await _context.LoadingDocuments
                .Select(x => x.DocumentNumber)
                .OrderBy(x => x)
                .ToListAsync(cancellationToken)
        };
    }
}