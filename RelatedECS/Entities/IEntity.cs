namespace RelatedECS.Entities;

public interface IEntity
{
    public int Id { get; }

    public IReadOnlyList<IEntity> Relations { get; }

    public ref T Add<T>() where T : struct;

    public ref T Get<T>() where T : struct;

    public bool Has<T>() where T : struct;

    public void Delete<T>() where T : struct;
}
