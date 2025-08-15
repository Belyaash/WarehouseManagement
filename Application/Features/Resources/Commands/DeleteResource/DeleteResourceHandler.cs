using Application.Contracts.Features.Resource.Commands.DeleteResource;
using Domain.Entities.Resources;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contracts;

namespace Application.Features.Resources.Commands.DeleteResource;

file sealed class DeleteResourceHandler : IRequestHandler<DeleteResourceCommand>
{
    private readonly IAppDbContext _context;

    public DeleteResourceHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteResourceCommand request, CancellationToken cancellationToken)
    {
        var resource = await _context.Resources
            .SingleAsync(DomainResource.Spec.ById(request.Dto.Id), cancellationToken);

        _context.Resources.Remove(resource);
        await _context.SaveChangesAsync(cancellationToken);
    }
}