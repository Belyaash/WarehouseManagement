using Application.Contracts.Features.Resource.Queries.GetResource;
using MediatR;

namespace Application.Contracts.Features.MeasureUnit.Queries.GetMeasureUnit;

public sealed record GetMeasureUnitQuery(GetMeasureUnitRequestDto Dto) : IRequest<GetMeasureUnitResponseDto>;