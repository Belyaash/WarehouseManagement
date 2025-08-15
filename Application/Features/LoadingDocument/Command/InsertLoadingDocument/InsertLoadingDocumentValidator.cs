using Application.Contracts.Features.LoadingDocument.Commands.InsertLoadingDocument;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Persistence.Contracts;

namespace Application.Features.LoadingDocument.Command.InsertLoadingDocument;

public sealed class InsertLoadingDocumentValidator : AbstractValidator<InsertLoadingDocumentCommand>
{
    private readonly IAppDbContext _context;

    public InsertLoadingDocumentValidator(IAppDbContext context)
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
    }

    private bool OnlyUniqueResources(List<InsertLoadingDocumentRequestDto.DocumentResourceDto> dtos)
    {
        return dtos.DistinctBy(x => new {x.ResourceId, x.MeasureUnitId}).Count() == dtos.Count;
    }

    private static bool IsCountPositive(List<InsertLoadingDocumentRequestDto.DocumentResourceDto> documentResourceDtos)
    {
        return !documentResourceDtos.Any(dto => dto.Count <= 0);
    }

    private Task<bool> IsValueUniqueAsync(string name, CancellationToken cancellationToken)
    {
        return _context.LoadingDocuments
            .AllAsync(!Domain.Entities.LoadingDocuments.LoadingDocument.Spec.ByNumber(name.Trim()), cancellationToken);
    }
}