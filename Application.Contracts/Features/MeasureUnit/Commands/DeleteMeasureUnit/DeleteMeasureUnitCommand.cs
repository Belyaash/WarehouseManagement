using Application.Contracts.Features.Resource.Commands.DeleteResource;
using MediatR;

namespace Application.Contracts.Features.MeasureUnit.Commands.DeleteMeasureUnit;

public sealed record DeleteMeasureUnitCommand(DeleteMeasureUnitRequestDto Dto) : IRequest;