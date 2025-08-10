using Application.Contracts.Features.MeasureUnit.Commands.InsertMeasureUnit;
using MediatR;

namespace Application.Contracts.Features.Client.Commands.InsertClient;

public sealed record InsertClientCommand(InsertClientRequestDto Dto) : IRequest<InsertClientResponseDto>;