using MediatR;

namespace Application.Contracts.Features.MeasureUnit.Commands.InsertMeasureUnit;

public sealed record InsertMeasureUnitCommand(InsertMeasureUnitRequestDto Dto) : IRequest<InsertMeasureUnitResponseDto>;