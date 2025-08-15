using Application.Contracts.Features.DispatchDocuments.Commands.InsertDispatchDocument;
using Domain.Entities.Balances;
using Domain.Entities.DispatchDocuments;
using Domain.Enums;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Persistence.Contracts;

namespace Application.Features.DispatchDocuments.Commands.InsertDispatchDocument;

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

    private async Task<bool> IsValueUniqueAsync(string name, CancellationToken cancellationToken)
    {
        return await _context.DispatchDocuments
            .AllAsync(!DispatchDocument.Spec.ByNumber(name.Trim()), cancellationToken);
    }

    private async Task<bool> IsBalanceStayNotNegativeAsync(List<InsertDispatchDocumentRequestDto.DocumentResourceDto> dtos, CancellationToken cancellationToken)
    {
        var balances = await _context.Balances
            .Where(Balance.Spec.ByResourcesContains(dtos.Select(r => r.ResourceId)))
            .Where(Balance.Spec.ByMeasureUnitsContains(dtos.Select(r => r.MeasureUnitId)))
            .ToListAsync(cancellationToken);

        var balancesCount = balances
            .Count(balance => dtos.Any(dto => dto.ResourceId == balance.DomainResourceId
                                              && dto.MeasureUnitId == balance.MeasureUnitId
                                              && balance.Count >= dto.Count));

        return dtos.Count == balancesCount;
    }
}