using Application.Contracts.Features.GetDispatchDocuments.Commands.DeleteDispatchDocument;
using MediatR;
using Persistence.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.DispatchDocument.Commands.DeleteDispatchDocument;

file sealed class DeleteDispatchDocumentHandler : IRequestHandler<DeleteDispatchDocumentCommand>
{
    private readonly IAppDbContext _context;

    public DeleteDispatchDocumentHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteDispatchDocumentCommand request, CancellationToken cancellationToken)
    {
        var document = await _context.DispatchDocuments
            .SingleAsync(Domain.Entities.DispatchDocuments.DispatchDocument.Spec.ById(request.Dto.Id), cancellationToken);
        _context.DispatchDocuments.Remove(document);
        await _context.SaveChangesAsync(cancellationToken);
    }
}