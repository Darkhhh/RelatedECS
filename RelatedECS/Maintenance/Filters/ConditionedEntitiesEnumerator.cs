using RelatedECS.Entities;
using RelatedECS.Filters;
using RelatedECS.Filters.Conditions;
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
        bool next, check;
        do
        {
            next = Provider.Next(Index, out int index);
            Index = index;
            check = next ? _checksController.Check(Provider.Get(Index).Id) : false;
        }
        while (next && !check);
        return next;
    }
}