using Application.Contracts.Features.DispatchDocuments.Commands.UpdateDispatchDocument;
using Domain.Enums;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Persistence.Contracts;

namespace Application.Features.DispatchDocument.Commands.UpdateDispatchDocument;

public sealed class UpdateDispatchDocumentValidator : AbstractValidator<UpdateDispatchDocumentCommand>
{
    private readonly IAppDbContext _context;

    public UpdateDispatchDocumentValidator(IAppDbContext context)
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

        When(x => x.Dto.State == StateType.Actual, () =>
        {
            RuleFor(x => x.Dto)
                .MustAsync(IsBalanceStayNotNegativeAsync)
                .WithMessage("Не хватает ресурсов на складе");
        });
    }

    private bool OnlyUniqueResources(List<UpdateDispatchDocumentRequestDto.DocumentResourceDto> dtos)
    {
        return dtos.DistinctBy(x => new {x.ResourceId, x.MeasureUnitId}).Count() == dtos.Count;
    }

    private static bool IsCountPositive(List<UpdateDispatchDocumentRequestDto.DocumentResourceDto> documentResourceDtos)
    {
        return !documentResourceDtos.Any(dto => dto.Count <= 0);
    }

    private Task<bool> IsValueUniqueAsync(string name, CancellationToken cancellationToken)
    {
        return _context.DispatchDocuments
            .AnyAsync(!Domain.Entities.DispatchDocuments.DispatchDocument.Spec.ByNumber(name.Trim()), cancellationToken);
    }

    private async Task<bool> IsBalanceStayNotNegativeAsync(UpdateDispatchDocumentRequestDto dto, CancellationToken cancellationToken)
    {
        var document = await _context.DispatchDocuments
            .Include(x => x.DispatchDocumentResources)
            .ThenInclude(x => x.Balance)
            .Where(Domain.Entities.DispatchDocuments.DispatchDocument.Spec.ById(dto.Id))
            .SingleAsync(cancellationToken);

        if (document.State == StateType.Archived)
        {
            return await CanCreateNewResources(dto.DocumentResources, cancellationToken);
        }

        if (!CanUpdateOldResources(dto, document)) return false;

        var newResources = dto.DocumentResources.Where(dr =>
                !document.DispatchDocumentResources.Any(ddr => ddr.DomainResourceId == dr.ResourceId
                                                               &&
                                                               dr.MeasureUnitId == ddr.MeasureUnitId))
            .ToList();

        return await CanCreateNewResources(newResources, cancellationToken);
    }

    private static bool CanUpdateOldResources(UpdateDispatchDocumentRequestDto dto, Domain.Entities.DispatchDocuments.DispatchDocument document)
    {
        var isResourcesCanBeUpdated = dto.DocumentResources.Any(dr =>
        {
            var resource = document.DispatchDocumentResources
                .FirstOrDefault(r => dr.ResourceId == r.DomainResourceId
                                     &&
                                     dr.MeasureUnitId == r.MeasureUnitId);
            if (resource == null)
                return true;

            return resource.Balance.Count + resource.Count - dr.Count >= 0;
        });

        if (!isResourcesCanBeUpdated)
            return false;
        return true;
    }

    private async Task<bool> CanCreateNewResources(List<UpdateDispatchDocumentRequestDto.DocumentResourceDto> dtos, CancellationToken cancellationToken)
    {
        var balances = await _context.Balances
            .Where(Domain.Entities.Balances.Balance.Spec.ByResourcesContains(dtos.Select(r => r.ResourceId)))
            .Where(Domain.Entities.Balances.Balance.Spec.ByMeasureUnitsContains(dtos.Select(r => r.MeasureUnitId)))
            .ToListAsync(cancellationToken);

        var balancesCount = balances
            .Count(balance => dtos.Any(dto => dto.ResourceId == balance.DomainResourceId
                                              && dto.MeasureUnitId == balance.MeasureUnitId
                                              && balance.Count >= dto.Count));

        return dtos.Count == balancesCount;
    }
}