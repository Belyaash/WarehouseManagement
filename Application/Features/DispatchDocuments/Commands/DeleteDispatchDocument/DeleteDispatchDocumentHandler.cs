using Application.Contracts.Features.DispatchDocuments.Commands.DeleteDispatchDocument;
using Domain.Entities.DispatchDocuments;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contracts;

namespace Application.Features.DispatchDocuments.Commands.DeleteDispatchDocument;

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
            .SingleAsync(DispatchDocument.Spec.ById(request.Dto.Id), cancellationToken);
        _context.DispatchDocuments.Remove(document);
        await _context.SaveChangesAsync(cancellationToken);
    }
}