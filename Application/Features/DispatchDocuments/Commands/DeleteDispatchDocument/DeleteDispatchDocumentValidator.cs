using Application.Contracts.Features.DispatchDocuments.Commands.DeleteDispatchDocument;
using Domain.Entities.DispatchDocuments;
using Domain.Enums;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Persistence.Contracts;

namespace Application.Features.DispatchDocuments.Commands.DeleteDispatchDocument;

public sealed class DeleteDispatchDocumentValidator : AbstractValidator<DeleteDispatchDocumentCommand>
{
    private readonly IAppDbContext _context;

    public DeleteDispatchDocumentValidator(IAppDbContext context)
    {
        _context = context;
        RuleFor(x => x.Dto)
            .MustAsync(IsNotActiveAsync)
            .WithMessage("Нельзя удалить активный документ отгрузки, сначала необходимо отозвать подпись.");
    }

    private Task<bool> IsNotActiveAsync(DeleteDispatchDocumentRequestDto dto, CancellationToken cancellationToken)
    {
        return _context.DispatchDocuments
            .AnyAsync(DispatchDocument.Spec.ById(dto.Id)
                      &&
                      !DispatchDocument.Spec.ByState(StateType.Actual), cancellationToken);
    }
}