using RelatedECS.Entities;

namespace RelatedECS.Filters.Singleton;

public class SingletonFilter
{
    private readonly RegisteredFilter _filter;

    internal IFilterDeclaration Declaration { get; }

    internal RegisteredFilter Raw => _filter;

    internal SingletonFilter(RegisteredFilter filter, IFilterDeclaration declaration)
    {
        _filter = filter;
        Declaration = declaration;
    }

    public IEntity Entity
    {
        get
        {
            if (_filter.Count != 1) throw new Exception("Singleton filter contains more than one entity, or no entity");
            return _filter.Get(0);
        }
    }
}
