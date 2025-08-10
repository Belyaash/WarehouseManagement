using MediatR;

namespace Application.Contracts.Features.Resource.Commands.UpdateResourceState;

public sealed record UpdateResourceStateCommand(UpdateResourceStateRequestDto Dto) : IRequest;