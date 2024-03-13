using RelatedECS.Entities;
using RelatedECS.Maintenance.Utilities;

namespace RelatedECS.Filters;

public interface IRegisteredFilter
{
    public int CheckEntity(Entity entity);

    public void SetMasks(Mask with, Mask without);

    public void ResizeMasks(int maxPoolIndex);

    public int Count { get; }

    public Entity GetEntity(int index);

    public int GetEntityIndex(int index);

    public IWorld World { get; }
}
