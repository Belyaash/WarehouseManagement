using MediatR;

namespace Application.Contracts.Features.Client.Commands.UpdateClientState;

public sealed record UpdateClientStateCommand(UpdateClientStateRequestDto Dto) : IRequest;