namespace RelatedECS.Entities.Relations;

public class RelationsContainer : IRelations, IRelationsInternal
{
    private readonly Dictionary<Type, HashSet<Relation>> _relations = new();
    private readonly HashSet<IEntity> _chains = new();

    private readonly Entity _main;

    public RelationsContainer(Entity main) => _main = main;

    public void Add<TRelationType>(IEntity entity)
    {
        var type = typeof(TRelationType);
        if (_relations.TryGetValue(type, out var relations))
        {
            relations.Add(new Relation { RelationType = type, EntityFrom = _main, EntityTo = entity });
        }
        else
        {
            _relations.Add(type, new HashSet<Relation> { new Relation { RelationType = type, EntityFrom = _main, EntityTo = entity } });
        }

        ((Entity)entity).GetRelationsContainer().AddChain(_main);
    }

    public void AddChain(IEntity entity) => _chains.Add(entity);

    public IEnumerable<IEntity> Get<TRelationType>()
    {
        if (_relations.TryGetValue(typeof(TRelationType), out var relations)) return relations.Select(x => x.EntityTo);
        return Enumerable.Empty<IEntity>();
    }

    public void RemoveRelatedTo(IEntity entity)
    {
        foreach (var rel in _relations)
        {
            rel.Value.RemoveWhere(x => x.EntityTo == entity || x.EntityFrom == entity);
        }
    }
}
