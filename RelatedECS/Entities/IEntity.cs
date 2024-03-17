using RelatedECS.Maintenance.Utilities;

namespace RelatedECS.Entities;

public interface IEntity : IAutoReset
{
    public int Id { get; }

    public bool IsAlive { get; }

    public IWorld World { get; }

    public Mask GetMask();

    public ref T Add<T>() where T : struct;

    public ref T Get<T>() where T : struct;

    public bool Has<T>() where T : struct;

    public void Delete<T>() where T : struct;
}
