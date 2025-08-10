using MediatR;

namespace Application.Contracts.Features.Resource.Commands.UpdateResource;

public sealed record UpdateResourceCommand(UpdateResourceRequestDto Dto) : IRequest;