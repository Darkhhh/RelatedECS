namespace RelatedECS.Filters;

public class FilterDeclaration : IFilterDeclaration
{
    private readonly HashSet<Type> _withTypes = new();
    private readonly HashSet<Type> _withoutTypes = new();

    public Type[] GetWithoutTypes() => _withoutTypes.ToArray();
    public Type[] GetWithTypes() => _withTypes.ToArray();

    public IFilterDeclaration With<T>() where T : struct
    {
        var type = typeof(T);
        _withTypes.Add(type);
        return this;
    }

    public IFilterDeclaration Without<T>() where T : struct
    {
        var type = typeof(T);
        _withoutTypes.Add(type);
        return this;
    }

    public void WithoutTypes(params Type[] types)
    {
        _withoutTypes.Clear();
        foreach (var type in types) _withoutTypes.Add(type);
    }

    public void WithTypes(params Type[] types)
    {
        _withTypes.Clear();
        foreach (var type in types) _withTypes.Add(type);
    }
}
