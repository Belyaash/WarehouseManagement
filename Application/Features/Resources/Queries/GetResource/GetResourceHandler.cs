using Application.Contracts.Features.Resource.Queries.GetResource;
using Domain.Entities.Resources;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contracts;

namespace Application.Features.Resources.Queries.GetResource;

file sealed class GetResourceHandler : IRequestHandler<GetResourceQuery, GetResourceResponseDto>
{
    private readonly IAppDbContext _context;

    public GetResourceHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<GetResourceResponseDto> Handle(GetResourceQuery request, CancellationToken cancellationToken)
    {
        return await _context.Resources
            .Where(DomainResource.Spec.ById(request.Dto.Id))
            .Select(r => new GetResourceResponseDto
            {
                Name = r.Name,
                State = r.State,
                IsUsed = r.Balances.Any() || r.DispatchDocumentResources.Any() || r.LoadingDocumentResources.Any()
            }).SingleAsync(cancellationToken);
    }
}