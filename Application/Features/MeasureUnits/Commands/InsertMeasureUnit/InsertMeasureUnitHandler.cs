using Application.Contracts.Features.MeasureUnit.Commands.InsertMeasureUnit;
using Domain.Entities.MeasureUnits;
using Domain.Entities.MeasureUnits.Parameters;
using MediatR;
using Persistence.Contracts;

namespace Application.Features.MeasureUnits.Commands.InsertMeasureUnit;

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

    private static MeasureUnit CreateMeasureUnit(InsertMeasureUnitCommand request)
    {
        var parameters = new CreateMeasureUnitParameters
        {
            Name = request.Dto.Name.Trim(),
        };

        return new MeasureUnit(parameters);
    }

    private static InsertMeasureUnitResponseDto CreateResponseDto(MeasureUnit measureUnit)
    {
        return new InsertMeasureUnitResponseDto()
        {
            Id = measureUnit.Id,
        };
    }
}