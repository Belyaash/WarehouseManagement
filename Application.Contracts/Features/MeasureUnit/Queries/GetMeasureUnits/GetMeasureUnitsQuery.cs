using Application.Contracts.Features.Resource.Queries.GetResources;
using MediatR;

namespace Application.Contracts.Features.MeasureUnit.Queries.GetMeasureUnits;

public sealed record GetMeasureUnitsQuery(GetMeasureUnitsRequestDto Dto) : IRequest<GetMeasureUnitsResponseDto>;