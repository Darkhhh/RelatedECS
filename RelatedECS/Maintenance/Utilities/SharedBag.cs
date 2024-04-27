namespace RelatedECS.Maintenance.Utilities;

public interface ISharedBag
{
    public void Add<T>(T obj) where T : notnull;

    public void Add<T>(T obj, string tag) where T : notnull;

    public T Get<T>() where T : notnull;

    public T Get<T>(string tag) where T : notnull;

    public bool Has<T>() where T : notnull;

    public bool Has(string tag);
}

public class SharedBag : ISharedBag
{
    private readonly Dictionary<Type, object> _objects = new();
    private readonly Dictionary<string, object> _taggedObjects = new();

    public int Count => _objects.Count;
    public int CountTagged => _taggedObjects.Count;

    public void Add<T>(T obj) where T : notnull
    {
        var type = typeof(T);
        if (_objects.ContainsKey(type)) throw new Exception($"Object of type {type.Name} already in bag");
        _objects.Add(type, obj);
    }

    public void Add<T>(T obj, string tag) where T : notnull
    {
        if (_taggedObjects.ContainsKey(tag)) throw new Exception($"Object with tag {tag} already in bag");
        _taggedObjects.Add(tag, obj);
    }

    public T Get<T>() where T : notnull
    {
        var type = typeof(T);
        if (!_objects.TryGetValue(type, out var value)) throw new Exception($"Object of type {type.Name} does not exist in bag");
        return (T)value;
    }

    public T Get<T>(string tag) where T : notnull
    {
        if (!_taggedObjects.TryGetValue(tag, out var value)) throw new Exception($"Object with tag {tag} does not exist in bag");
        return (T)value;
    }

    public bool Has<T>() where T : notnull
    {
        return _objects.ContainsKey(typeof(T));
    }

    public bool Has(string tag)
    {
        return _taggedObjects.ContainsKey(tag);
    }
}