using Application.Contracts.Features.DispatchDocuments.Queries.GetDispatchDocumentNumbers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contracts;

namespace Application.Features.DispatchDocument.Queries.GetDispatchDocumentNumbers;

file sealed class GetDispatchDocumentNumbersHandler : IRequestHandler<GetDispatchDocumentNumbersQuery, GetDispatchDocumentNumbersResponseDto>
{
    private readonly IAppDbContext _context;

    public GetDispatchDocumentNumbersHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<GetDispatchDocumentNumbersResponseDto> Handle(GetDispatchDocumentNumbersQuery request, CancellationToken cancellationToken)
    {
        return new GetDispatchDocumentNumbersResponseDto
        {
            DocumentNumbers = await _context.DispatchDocuments
                .Select(x => x.DocumentNumber)
                .OrderBy(x => x)
                .ToListAsync(cancellationToken)
        };
    }
}