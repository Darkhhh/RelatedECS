using RelatedECS.Pools;

namespace RelatedECS.Filters.Conditions;

public interface IConditionsCheckController
{
    public bool AddCheck(IConditionCheck check);

    public void RemoveCheck(IConditionCheck check);

    public void Clear();

    public bool Check(int entity);

    public int Count { get; }
}

public class ConditionsCheckController(IPoolsProvider controller) : IConditionsCheckController
{
    private readonly Dictionary<Type, IConditionCheck> _checks = new();
    private readonly IPoolsProvider _controller = controller;

    public int Count => _checks.Count;

    public bool AddCheck(IConditionCheck check) => _checks.TryAdd(check.ObjectType, check);

    public void RemoveCheck(IConditionCheck check) => _checks.Remove(check.ObjectType);

    public void Clear() => _checks.Clear();

    public bool Check(int entity)
    {
        var flag = true;

        foreach (var check in _checks)
        {
            var pool = _controller.GetPool(check.Key);
            if (!check.Value.Check(pool.GetRaw(entity))) flag = false;
        }
        return flag;
    }
}
