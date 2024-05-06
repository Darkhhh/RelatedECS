namespace RelatedECS.Entities.Relations;

internal interface IRelationsInternal
{
    public void AddChain(IEntity entity);
}


public interface IRelations
{
    public void Add<TRelationType>(IEntity entity);

    public void RemoveRelatedTo(IEntity entity);

    public IEnumerable<IEntity> Get<TRelationType>();
}
