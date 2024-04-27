using RelatedECS.Entities;

namespace RelatedECS.Filters;

public interface IEntitiesProvider
{
    public int GetInitialIndex();

    public bool Next(int previousIndex, out int currentIndex);

    public Entity Get(int index);
}

public interface IRegisteredFilter
{
    public void Lock();

    public void Unlock();

    public int CheckEntity(Entity entity);

    public void ResizeMasks(int maxPoolIndex);

    public int Count { get; }

    public bool IsLocked { get; }
}