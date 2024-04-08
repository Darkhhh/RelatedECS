namespace RelatedECS.Filters.Conditions;

public interface IConditionCheck
{
    Type ObjectType { get; }

    bool Check(object obj);
}

abstract class ConditionCheck<T> : IConditionCheck where T : struct
{
    public Type ObjectType => typeof(T);

    public bool Check(object obj) => CheckCondition((T)obj);

    public abstract bool CheckCondition(T obj);
}
