using Application.Contracts.Features.Resource.Commands.InsertResource;
using Domain.Entities.Resources;
using Domain.Entities.Resources.Parameters;
using MediatR;
using Persistence.Contracts;

namespace Application.Features.Resources.Commands.InsertResource;

file sealed class InsertResourceHandler : IRequestHandler<InsertResourceCommand, InsertResourceResponseDto>
{
    private readonly IAppDbContext _context;

    public InsertResourceHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<InsertResourceResponseDto> Handle(InsertResourceCommand request, CancellationToken cancellationToken)
    {
        var resource = CreateResource(request);

        _context.Resources.Add(resource);
        await _context.SaveChangesAsync(cancellationToken);

        return CreateResponseDto(resource);
    }

    private static DomainResource CreateResource(InsertResourceCommand request)
    {
        var parameters = new CreateResourceParameters
        {
            Name = request.Dto.Name.Trim(),
        };

        return new DomainResource(parameters);
    }

    private static InsertResourceResponseDto CreateResponseDto(DomainResource domainResource)
    {
        return new InsertResourceResponseDto()
        {
            Id = domainResource.Id,
        };
    }
}