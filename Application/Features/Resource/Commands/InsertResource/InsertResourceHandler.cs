using Application.Contracts.Features.MeasureUnit.Commands.InsertMeasureUnit;
using Application.Contracts.Features.Resource.Commands.InsertResource;
using Domain.Entities.Resources.Parameters;
using MediatR;
using Persistence.Contracts;

namespace Application.Features.Resource.Commands.InsertResource;

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

    private static Domain.Entities.Resources.DomainResource CreateResource(InsertResourceCommand request)
    {
        var parameters = new CreateResourceParameters
        {
            Name = request.Dto.Name,
        };

        return new Domain.Entities.Resources.DomainResource(parameters);
    }

    private static InsertResourceResponseDto CreateResponseDto(Domain.Entities.Resources.DomainResource domainResource)
    {
        return new InsertResourceResponseDto()
        {
            Id = domainResource.Id,
        };
    }
}