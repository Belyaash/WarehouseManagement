using Application.Contracts.Features.LoadingDocument.Commands.UpdateLoadingDocument;
using Domain.Entities.LoadingDocumentResources;
using Domain.Entities.LoadingDocuments;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Persistence.Contracts;

namespace Application.Features.LoadingDocuments.Command.UpdateLoadingDocument;

public sealed class UpdateLoadingDocumentValidator : AbstractValidator<UpdateLoadingDocumentCommand>
{
    private readonly IAppDbContext _context;

    public UpdateLoadingDocumentValidator(IAppDbContext context)
    {
        _context = context;
        RuleFor(c => c.Dto.DocumentNumber)
            .NotEmpty()
            .WithMessage("У документа должен быть указан номер");

        RuleFor(c => c.Dto)
            .MustAsync(IsValueUniqueAsync)
            .WithMessage("Документ с таким номером уже существует")
            .MustAsync(IsBalanceStayNotNegative)
            .WithMessage("В результате изменения останутся негативные позиции баланса");

        RuleFor(x => x.Dto.DocumentResources)
            .NotEmpty()
            .WithMessage("Должны быть указаны ресурсы")
            .Must(IsCountPositive)
            .WithMessage("Количество должно быть больше нуля")
            .Must(OnlyUniqueResources)
            .WithMessage("Связка ресурс-единицы измерения не должны повторяться");
    }

    private bool OnlyUniqueResources(List<UpdateLoadingDocumentRequestDto.DocumentResourceDto> dtos)
    {
        return dtos.DistinctBy(x => new {x.ResourceId, x.MeasureUnitId}).Count() == dtos.Count;
    }

    private async Task<bool> IsBalanceStayNotNegative(UpdateLoadingDocumentRequestDto dto, CancellationToken cancellationToken)
    {
        var documentResources = await _context.LoadingDocumentResources
            .Include(x => x.Balance)
            .Where(LoadingDocumentResource.Spec.ByDocumentId(dto.Id))
            .ToListAsync(cancellationToken);


        return documentResources
            .All(r => GetCountChangeValue(dto, r) - r.Count >= 0);
    }

    private static int GetCountChangeValue(UpdateLoadingDocumentRequestDto dto, LoadingDocumentResource resource)
    {
        return dto.DocumentResources
            .Where(d => d.ResourceId == resource.DomainResourceId && d.MeasureUnitId == resource.MeasureUnitId)
            .Select(d => d.Count)
            .SingleOrDefault(resource.Count);
    }

    private static bool IsCountPositive(List<UpdateLoadingDocumentRequestDto.DocumentResourceDto> documentResourceDtos)
    {
        return !documentResourceDtos.Any(dto => dto.Count <= 0);
    }

    private Task<bool> IsValueUniqueAsync(UpdateLoadingDocumentRequestDto dto, CancellationToken cancellationToken)
    {
        return _context.LoadingDocuments
            .Where(!LoadingDocument.Spec.ById(dto.Id))
            .AllAsync(!LoadingDocument.Spec.ByNumber(dto.DocumentNumber.Trim()), cancellationToken);
    }
}