using Application.Contracts.Features.Resource.Commands.UpdateResource;
using Domain.Entities.Resources;
using Domain.Entities.Resources.Parameters;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contracts;

namespace Application.Features.Resources.Commands.UpdateResource;

file sealed class UpdateResourceHandler : IRequestHandler<UpdateResourceCommand>
{
    private readonly IAppDbContext _context;

    public UpdateResourceHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateResourceCommand request, CancellationToken cancellationToken)
    {
        var resource = await _context.Resources
            .SingleAsync(DomainResource.Spec.ById(request.Dto.Id), cancellationToken);

        var parameters = new UpdateResourceParameters
        {
            Name = request.Dto.Name.Trim()
        };
        resource.Update(parameters);

        _context.Resources.Update(resource);
        await _context.SaveChangesAsync(cancellationToken);
    }
}