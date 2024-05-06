using RelatedECS.Entities.Relations;
using RelatedECS.Maintenance.Utilities;

namespace RelatedECS.Entities;

internal interface IInternalEntity : IObjectPoolAutoReset
{
    public Mask GetMask();

    public int Generation { get; }

    public RelationsContainer GetRelationsContainer();
}

public interface IEntity
{
    public int Id { get; }

    public bool IsAlive { get; }

    public IWorld World { get; }

    public ref T Add<T>() where T : struct;

    public ref T Get<T>() where T : struct;

    public bool Has<T>() where T : struct;

    public void Delete<T>() where T : struct;
}