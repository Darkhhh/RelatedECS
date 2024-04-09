namespace RelatedECS.Filters;

public interface IFilterDeclaration
{
    public IFilterDeclaration With<T>() where T : struct;

    public IFilterDeclaration Without<T>() where T : struct;

    public void WithTypes(params Type[] types);

    public void WithoutTypes(params Type[] types);

    public Type[] GetWithTypes();

    public Type[] GetWithoutTypes();

    public bool HasWithType(Type type);

    public bool HasWithoutType(Type type);
}
