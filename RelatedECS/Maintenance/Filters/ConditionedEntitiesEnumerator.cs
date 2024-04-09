using RelatedECS.Entities;
using RelatedECS.Filters.Conditions;
using RelatedECS.Filters;
using RelatedECS.Pools;

namespace RelatedECS.Maintenance.Filters;

public class ConditionedEntitiesEnumerator : EntitiesEnumerator
{
    private readonly IConditionsCheckController _checksController;

    public ConditionedEntitiesEnumerator(IEntitiesProvider provider, IPoolsProvider poolsProvider, Action<EntitiesEnumeratorBase<IEntity>> onReset) : base(provider, onReset)
    {
        _checksController = new ConditionsCheckController(poolsProvider);
    }

    public void Add(IConditionCheck check) => _checksController.AddCheck(check);

    public void Remove(IConditionCheck check) => _checksController.RemoveCheck(check);

    public void ClearChecks() => _checksController.Clear();

    public override bool MoveNext()
    {
        var next = Provider.Next(Index, out var index);
        Index = index;
        if (!next) return false;
        while (!_checksController.Check(Provider.Get(Index).Id))
        {
            next = Provider.Next(Index, out index);
            Index = index;
            if (!next) break;
        }
        return next;
    }
}
