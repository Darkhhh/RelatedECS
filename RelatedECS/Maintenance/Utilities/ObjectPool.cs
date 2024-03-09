using System.Collections.Concurrent;

namespace RelatedECS.Maintenance.Utilities;

public interface IAutoReset
{
    public void Init();
    public void Reset();
}

/// <summary>
/// Based on <see href="https://learn.microsoft.com/ru-ru/dotnet/standard/collections/thread-safe/how-to-create-an-object-pool">Microsoft Documentation</see>
/// </summary>
public class ObjectPool<T> where T : IAutoReset, new()
{
    private readonly ConcurrentBag<T> _objects;
    private readonly Func<T> _objectGenerator;

    public ObjectPool(Func<T>? objectGenerator = null)
    {
        _objectGenerator = objectGenerator is null ? () => new T() : objectGenerator;
        _objects = new ConcurrentBag<T>();
    }

    public T Get()
    {
        var result = _objects.TryTake(out T item) ? item : _objectGenerator();
        result.Init();
        return result;
    }

    public void Return(T item)
    {
        item.Reset();
        _objects.Add(item);
    }
}
