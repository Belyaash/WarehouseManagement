using Application.Contracts.Features.LoadingDocument.Commands.DeleteLoadingDocument;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contracts;

namespace Application.Features.LoadingDocument.Command.DeleteLoadingDocument;

file sealed class DeleteLoadingDocumentHandler : IRequestHandler<DeleteLoadingDocumentCommand>
{
    private readonly IAppDbContext _context;

    public DeleteLoadingDocumentHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteLoadingDocumentCommand request, CancellationToken cancellationToken)
    {
        var document = await _context.LoadingDocuments
            .Include(x => x.Resources)
            .ThenInclude(x => x.Balance)
            .SingleAsync(Domain.Entities.LoadingDocuments.LoadingDocument.Spec.ById(request.Dto.Id), cancellationToken);

        foreach (var loadingDocumentResource in document.Resources)
        {
            loadingDocumentResource.Balance.Count -= loadingDocumentResource.Count;
        }
        _context.LoadingDocumentResources.RemoveRange(document.Resources);
        _context.LoadingDocuments.Remove(document);
        await _context.SaveChangesAsync(cancellationToken);
    }
}