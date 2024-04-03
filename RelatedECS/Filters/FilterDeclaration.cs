namespace RelatedECS.Filters;

public class FilterDeclaration : IFilterDeclaration
{
    private readonly IWorld _world;
    private readonly HashSet<Type> _withTypes = new();
    private readonly HashSet<Type> _withoutTypes = new();

    public FilterDeclaration(IWorld world) => _world = world;

    public Type[] GetWithoutTypes() => _withoutTypes.ToArray();
    public Type[] GetWithTypes() => _withTypes.ToArray();

    public IFilterDeclaration With<T>() where T : struct
    {
        var type = typeof(T);
        _world.GetPool<T>();
        if (_withoutTypes.Contains(type)) throw new Exception($"Intersection by with and without types: {type.Name}");
        _withTypes.Add(type);
        return this;
    }

    public IFilterDeclaration Without<T>() where T : struct
    {
        var type = typeof(T);
        _world.GetPool<T>();
        if (_withTypes.Contains(type)) throw new Exception($"Intersection by with and without types: {type.Name}");
        _withoutTypes.Add(type);
        return this;
    }

    public void WithoutTypes(params Type[] types)
    {
        _withoutTypes.Clear();
        foreach (var type in types)
        {
            if (_withTypes.Contains(type)) throw new Exception($"Intersection by with and without types: {type.Name}");
            _withoutTypes.Add(type);
        }
    }

    public void WithTypes(params Type[] types)
    {
        _withTypes.Clear();
        foreach (var type in types)
        {
            if (_withoutTypes.Contains(type)) throw new Exception($"Intersection by with and without types: {type.Name}");
            _withTypes.Add(type);
        }
    }

    internal bool EqualTo(IFilterDeclaration other)
    {
        var with = other.GetWithTypes();
        var without = other.GetWithoutTypes();

        if (with.Length != _withTypes.Count || without.Length != _withoutTypes.Count) return false;
        foreach(var type in with)
        {
            if (!_withTypes.Contains(type)) return false;
        }
        foreach(var type in without)
        {
            if (!_withoutTypes.Contains(type)) return false;
        }

        return true;
    }
}
