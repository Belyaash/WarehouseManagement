using Domain.Enums;

namespace Domain.Entities.Resources.Parameters;

public sealed class ChangeResourceStateParameters
{
    public required StateType StateType { get; init; }
}