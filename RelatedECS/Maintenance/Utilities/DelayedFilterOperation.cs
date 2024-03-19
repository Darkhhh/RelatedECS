using RelatedECS.Entities;

namespace RelatedECS.Maintenance.Utilities;

internal enum FilterOperationType
{
    Add, Remove
}

internal readonly struct DelayedFilterOperation
{
    public readonly Entity Reference;
    public readonly int EndityId;
    public readonly FilterOperationType Operation;

    public DelayedFilterOperation(Entity reference, int endityId, FilterOperationType operation)
    {
        Reference = reference;
        EndityId = endityId;
        Operation = operation;
    }
}
