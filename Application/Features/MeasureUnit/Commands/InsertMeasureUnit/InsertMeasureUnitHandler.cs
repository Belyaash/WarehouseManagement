using Application.Contracts.Features.MeasureUnit.Commands.InsertMeasureUnit;
using Domain.Entities.MeasureUnits.Parameters;
using MediatR;
using Persistence.Contracts;

namespace Application.Features.MeasureUnit.Commands.InsertMeasureUnit;

file sealed class InsertMeasureUnitHandler : IRequestHandler<InsertMeasureUnitCommand, InsertMeasureUnitResponseDto>
{
    private readonly IAppDbContext _context;

    public InsertMeasureUnitHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<InsertMeasureUnitResponseDto> Handle(InsertMeasureUnitCommand request, CancellationToken cancellationToken)
    {
        var resource = CreateMeasureUnit(request);

        _context.MeasureUnits.Add(resource);
        await _context.SaveChangesAsync(cancellationToken);

        return CreateResponseDto(resource);
    }

    private static Domain.Entities.MeasureUnits.MeasureUnit CreateMeasureUnit(InsertMeasureUnitCommand request)
    {
        var parameters = new CreateMeasureUnitParameters
        {
            Name = request.Dto.Name.Trim(),
        };

        return new Domain.Entities.MeasureUnits.MeasureUnit(parameters);
    }

    private static InsertMeasureUnitResponseDto CreateResponseDto(Domain.Entities.MeasureUnits.MeasureUnit measureUnit)
    {
        return new InsertMeasureUnitResponseDto()
        {
            Id = measureUnit.Id,
        };
    }
}