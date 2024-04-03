using System.Collections.Concurrent;

namespace RelatedECS.Maintenance.Utilities;

public interface IObjectPoolAutoReset
{
    public void PoolInit();
    public void PoolReset();
}

public interface IObjectPool
{
    public Type GetObjectType();
}

public class ObjectPoolNoResetWrap<T> : IObjectPoolAutoReset
{
    public T Value { get; }

    public ObjectPoolNoResetWrap(T value) => Value = value;

    public void PoolInit()
    {
        return;
    }

    public void PoolReset()
    {
        return;
    }
}

/// <summary>
/// Based on <see href="https://learn.microsoft.com/ru-ru/dotnet/standard/collections/thread-safe/how-to-create-an-object-pool">Microsoft Documentation</see>
/// </summary>
public class ObjectPool<T> : IObjectPool where T : IObjectPoolAutoReset
{
    private readonly ConcurrentBag<T> _objects;
    private readonly Func<T> _objectGenerator;

    public ObjectPool(Func<T> objectGenerator)
    {
        _objectGenerator = objectGenerator;
        _objects = new ConcurrentBag<T>();
    }

    public T Get()
    {
        var result = _objects.TryTake(out var item) ? item : _objectGenerator();
        result.PoolInit();
        return result;
    }

    public void Return(T item)
    {
        item.PoolReset();
        _objects.Add(item);
    }

    public Type GetObjectType() => typeof(T);

    public int CurrentPoolCapacity => _objects.Count;
}
