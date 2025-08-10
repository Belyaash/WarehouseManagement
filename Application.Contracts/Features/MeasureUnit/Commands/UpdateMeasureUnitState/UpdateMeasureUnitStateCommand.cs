using MediatR;

namespace Application.Contracts.Features.MeasureUnit.Commands.UpdateMeasureUnitState;

public sealed record UpdateMeasureUnitStateCommand(UpdateMeasureUnitStateRequestDto Dto) : IRequest;