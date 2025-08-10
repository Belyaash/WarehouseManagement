using MediatR;

namespace Application.Contracts.Features.Client.Commands.UpdateClient;

public sealed record UpdateClientCommand(UpdateClientRequestDto Dto) : IRequest;