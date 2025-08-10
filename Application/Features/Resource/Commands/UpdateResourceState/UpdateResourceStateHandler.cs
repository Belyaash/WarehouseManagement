using Application.Contracts.Features.Resource.Commands.UpdateResourceState;
using Domain.Entities.Resources;
using Domain.Entities.Resources.Parameters;
using Domain.Enums;
using MediatR;
using Persistence.Contracts;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Resource.Commands.UpdateResourceState;

file sealed class UpdateResourceStateHandler : IRequestHandler<UpdateResourceStateCommand>
{
    private readonly IAppDbContext _context;

    public UpdateResourceStateHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateResourceStateCommand request, CancellationToken cancellationToken)
    {
        var resource = await _context.Resources
            .SingleAsync(DomainResource.Spec.ById(request.Dto.Id), cancellationToken);

        var parameters = new ChangeResourceStateParameters
        {
            StateType = request.Dto.StateType
        };
        resource.ChangeState(parameters);

        _context.Resources.Update(resource);
        await _context.SaveChangesAsync(cancellationToken);
    }
}