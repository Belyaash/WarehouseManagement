using Application.Contracts.Features.DispatchDocuments.Commands.InsertDispatchDocument;
using Domain.Enums;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Persistence.Contracts;

namespace Application.Features.DispatchDocument.Commands.InsertDispatchDocument;

public sealed class InsertDispatchDocumentValidator : AbstractValidator<InsertDispatchDocumentCommand>
{
    private readonly IAppDbContext _context;

    public InsertDispatchDocumentValidator(IAppDbContext context)
    {
        _context = context;
        RuleFor(c => c.Dto.DocumentNumber)
            .MustAsync(IsValueUniqueAsync)
            .WithMessage("Документ с таким номером уже существует")
            .NotEmpty()
            .WithMessage("У документа должен быть указан номер");

        RuleFor(x => x.Dto.DocumentResources)
            .NotEmpty()
            .WithMessage("Должны быть указаны ресурсы")
            .Must(IsCountPositive)
            .WithMessage("Количество должно быть больше нуля")
            .Must(OnlyUniqueResources)
            .WithMessage("Связка ресурс-единицы измерения не должны повторяться");

        When(x => x.Dto.StateType == StateType.Actual, () =>
        {
            RuleFor(x => x.Dto.DocumentResources)
                .MustAsync(IsBalanceStayNotNegativeAsync)
                .WithMessage("Не хватает ресурсов на складе");
        });
    }

    private bool OnlyUniqueResources(List<InsertDispatchDocumentRequestDto.DocumentResourceDto> dtos)
    {
        return dtos.DistinctBy(x => new {x.ResourceId, x.MeasureUnitId}).Count() == dtos.Count;
    }

    private static bool IsCountPositive(List<InsertDispatchDocumentRequestDto.DocumentResourceDto> documentResourceDtos)
    {
        return !documentResourceDtos.Any(dto => dto.Count <= 0);
    }

    private Task<bool> IsValueUniqueAsync(string name, CancellationToken cancellationToken)
    {
        return _context.LoadingDocuments
            .AllAsync(!Domain.Entities.LoadingDocuments.LoadingDocument.Spec.ByNumber(name.Trim()), cancellationToken);
    }

    private async Task<bool> IsBalanceStayNotNegativeAsync(List<InsertDispatchDocumentRequestDto.DocumentResourceDto> dtos, CancellationToken cancellationToken)
    {
        var balancesCount = await _context.Balances
            .Where(balance => dtos.Any(dto => dto.ResourceId == balance.Id
                                              && dto.MeasureUnitId == balance.MeasureUnitId
                                              && balance.Count >= dto.Count))
            .CountAsync(cancellationToken);

        return dtos.Count == balancesCount;
    }
}