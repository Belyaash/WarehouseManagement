using Application.Contracts.Features.LoadingDocument.Commands.DeleteLoadingDocument;
using Domain.Entities.LoadingDocumentResources;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Persistence.Contracts;

namespace Application.Features.LoadingDocument.Command.DeleteLoadingDocument;

public sealed class DeleteLoadingDocumentValidator : AbstractValidator<DeleteLoadingDocumentCommand>
{
    private readonly IAppDbContext _context;

    public DeleteLoadingDocumentValidator(IAppDbContext context)
    {
        _context = context;
        RuleFor(c => c.Dto)
            .MustAsync(IsBalanceStayNotNegative)
            .WithMessage("В результате изменения останутся негативные позиции баланса");
    }
    
    private Task<bool> IsBalanceStayNotNegative(DeleteLoadingDocumentRequestDto dto, CancellationToken cancellationToken)
    {
        return _context.LoadingDocumentResources
            .Where(LoadingDocumentResource.Spec.ByDocumentId(dto.Id))
            .AllAsync(r => r.Balance.Count - r.Count >= 0, cancellationToken);
    }
}