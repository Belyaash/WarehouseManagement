using MediatR;

namespace Application.Contracts.Features.Resource.Commands.InsertResource;

public sealed record InsertResourceCommand(InsertResourceRequestDto Dto) : IRequest<InsertResourceResponseDto>;