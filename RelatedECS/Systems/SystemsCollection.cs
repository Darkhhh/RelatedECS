using RelatedECS.Systems.SystemGroups;

namespace RelatedECS.Systems;

public interface ISystemsCollection
{
    public ISystemGroup GetSystemGroup(string name);

    public IWorld World { get; }
}

public interface IExecutableSystemCollection
{
    public IExecutableSystemCollection AddSystem(ISystem system);

    public IExecutableSystemCollection AddGroup(ISystemGroup systemGroup);

    public IExecutableSystemCollection Prepare();

    public void Execute();

    public void Dispose();
}

public class SystemsCollection : ISystemsCollection, IExecutableSystemCollection
{
    private readonly IWorld _world;
    private readonly ISystemsController _systemsController;

    public IWorld World => _world;

    public SystemsCollection(IWorld world)
    {
        _world = world;
        _systemsController = new SystemsController();
    }

    public IExecutableSystemCollection AddGroup(ISystemGroup systemGroup)
    {
        _systemsController.AddGroup(systemGroup);
        return this;
    }

    public IExecutableSystemCollection AddSystem(ISystem system)
    {
        _systemsController.AddSystem(system);
        return this;
    }

    public IExecutableSystemCollection Prepare()
    {
        _systemsController.Prepare(this);
        return this;
    }

    public void Execute()
    {
        _systemsController.FramePrepare(this);
        _systemsController.Execute(this);
        _systemsController.LateExecute(this);
        _systemsController.FrameDispose(this);
    }

    public void Dispose()
    {
        _systemsController.Dispose(this);
    }

    public ISystemGroup GetSystemGroup(string name)
    {
        return _systemsController.GetSystemGroup(name);
    }
}