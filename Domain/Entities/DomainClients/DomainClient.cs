using Domain.Entities.DomainClients.Parameters;
using Domain.Enums;

namespace Domain.Entities.DomainClients;

public class DomainClient
{
    private DomainClient()
    {
    }

    public DomainClient(CreateDomainClientParameters parameters)
    {
        Name = parameters.Name;
        Address = parameters.Address;
        State = StateType.Actual;
    }

    public int Id { get; private set; }
    public string Name { get; private set; } = default!;
    public string Address { get; private set; } = default!;
    public StateType State { get; private set; }
}