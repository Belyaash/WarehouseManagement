using MediatR;

namespace Application.Contracts.Features.Resource.Commands.DeleteResource;

public sealed record DeleteResourceCommand(DeleteResourceRequestDto Dto) : IRequest;