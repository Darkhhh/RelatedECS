using RelatedECS.Maintenance.Utilities;

namespace RelatedECS.Entities;

public class Entity : IEntity, IInternalEntity
{
    private readonly IWorld _world;
    private readonly int _id;
    private readonly Mask _mask = new();

    private bool _alive;
    private int _generation;

    public Entity(int id, IWorld world) => (_id, _world) = (id, world);

    public int Id => _id;

    public bool IsAlive => _alive;

    public IWorld World => _world;

    public int Generation => _generation;

    public ref T Add<T>() where T : struct => ref _world.GetPool<T>().Add(_id);

    public void Delete<T>() where T : struct => _world.GetPool<T>().Delete(_id);

    public ref T Get<T>() where T : struct => ref _world.GetPool<T>().Get(_id);

    public bool Has<T>() where T : struct => _world.GetPool<T>().Has(_id);

    public Mask GetMask() => _mask;

    public void PoolInit()
    {
        _mask.Clear();
        _alive = true;
        _generation++;
    }

    public void PoolReset()
    {
        _mask.Clear();
        _alive = false;
    }
}