using Application.Contracts.Features.Resource.Commands.UpdateResource;
using MediatR;

namespace Application.Contracts.Features.MeasureUnit.Commands.UpdateMeasureUnit;

public sealed record UpdateMeasureUnitCommand(UpdateMeasureUnitRequestDto Dto) : IRequest;