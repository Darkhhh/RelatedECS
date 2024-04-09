using RelatedECS.Entities;
using RelatedECS.Filters;
using RelatedECS.Maintenance.Utilities;
using System.Collections;

namespace RelatedECS.Maintenance.Filters;


public abstract class EntitiesEnumeratorBase<TResultType>(IEntitiesProvider provider, Action<EntitiesEnumeratorBase<TResultType>> onReset) : 
    IEnumerable<TResultType>, IEnumerator<TResultType>
{
    protected readonly IEntitiesProvider Provider = provider;

    protected int Index = -1;

    public abstract TResultType Current { get; }

    public virtual bool MoveNext()
    {
        var next = Provider.Next(Index, out var index);
        Index = index;
        return next;
    }

    public void Reset()
    {
        Index = -1;
        onReset.Invoke(this);
    }

    object IEnumerator.Current => Current ?? throw new NullReferenceException();

    public IEnumerator<TResultType> GetEnumerator()
    {
        Index = Provider.GetInitialIndex();
        return this;
    }
    public void Dispose() => Reset();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}


public class EntitiesEnumerator(IEntitiesProvider provider, Action<EntitiesEnumeratorBase<IEntity>> onReset) : EntitiesEnumeratorBase<IEntity>(provider, onReset), IObjectPoolAutoReset
{
    public override IEntity Current => Provider.Get(Index);

    public void PoolInit() { }

    public void PoolReset() { }
}


public class IndicesEnumerator(IEntitiesProvider provider, Action<EntitiesEnumeratorBase<int>> onReset) : EntitiesEnumeratorBase<int>(provider, onReset), IObjectPoolAutoReset
{
    public override int Current => Provider.Get(Index).Id;

    public void PoolInit() { }

    public void PoolReset() { }
}
