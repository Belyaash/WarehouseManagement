using MediatR;

namespace Application.Contracts.Features.Client.Commands.DeleteClient;

public sealed record DeleteClientCommand(DeleteClientRequestDto Dto) : IRequest;