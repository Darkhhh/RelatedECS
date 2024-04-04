namespace RelatedECS.Filters;

public interface IFilter<TReturnType>
{
    public int Count { get; }

    public IEnumerable<TReturnType> Entities();
}