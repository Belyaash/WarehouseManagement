using Domain.Entities.Balances;
using Domain.Entities.DispatchDocumentResources;
using Domain.Entities.LoadingDocumentResources;
using Domain.Entities.LoadingDocuments;
using Domain.Entities.Resources.Parameters;
using Domain.Enums;

namespace Domain.Entities.Resources;

public class Resource
{
    private Resource()
    {
    }

    public Resource(CreateResourceParameters parameters)
    {
        Name = parameters.Name;
        State = StateType.Actual;
    }

    public int Id { get; private set; }
    public string Name { get; private set; } = default!;
    public StateType State { get; private set; }

    private List<LoadingDocumentResource> _loadingDocumentResources = new();
    public IReadOnlyList<LoadingDocumentResource> LoadingDocumentResources => _loadingDocumentResources.AsReadOnly();

    private List<DispatchDocumentResource> _dispatchDocumentResources = new();
    public IReadOnlyList<DispatchDocumentResource> DispatchDocumentResources => _dispatchDocumentResources.AsReadOnly();

    private List<Balance> _balances = new();
    public IReadOnlyList<Balance> Balances => _balances.AsReadOnly();
}