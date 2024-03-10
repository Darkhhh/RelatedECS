namespace RelatedECS.Systems.SystemGroups;

public class SystemGroup : ISystemGroup
{
    private const int InitialCapacity = 8;

    private bool _active = true;

    private readonly HashSet<ISystem> _allSystems = new(InitialCapacity);
    private readonly HashSet<IPrepareSystem> _prepareSystems = new(InitialCapacity);
    private readonly HashSet<IFramePrepareSystem> _framePrepareSystems = new(InitialCapacity);
    private readonly HashSet<IExecuteSystem> _executeSystems = new(InitialCapacity);
    private readonly HashSet<ILateExecuteSystem> _lateExecuteSystems = new(InitialCapacity);
    private readonly HashSet<IFrameDisposeSystem> _frameDisposeSystems = new(InitialCapacity);
    private readonly HashSet<IDisposeSystem> _disposeSystems = new(InitialCapacity);

    public string Name { get; }

    public SystemGroup(string name) => Name = name;

    public void Activate() => _active = true;
    public void Deactivate() => _active = false;


    public ISystemGroup AppendSystem(ISystem system)
    {
        _allSystems.Add(system);

        if (system is IPrepareSystem initSystem) _prepareSystems.Add(initSystem);
        if (system is IFramePrepareSystem frameInitSystem) _framePrepareSystems.Add(frameInitSystem);
        if (system is IExecuteSystem executeSystem) _executeSystems.Add(executeSystem);
        if (system is ILateExecuteSystem lateExecuteSystem) _lateExecuteSystems.Add(lateExecuteSystem);
        if (system is IFrameDisposeSystem frameDisposeSystem) _frameDisposeSystems.Add(frameDisposeSystem);
        if (system is IDisposeSystem disposeSystem) _disposeSystems.Add(disposeSystem);

        return this;
    }

    public IReadOnlyList<ISystem> GetAllSystems() => _allSystems.ToList();


    public void Prepare(IWorld world)
    {
        if (!_active) return;
        foreach (var system in _prepareSystems) system.Prepare(world);
    }

    public void FramePrepare(IWorld world)
    {
        if (!_active) return;
        foreach (var system in _framePrepareSystems) system.FramePrepare(world);
    }

    public void Execute(IWorld world)
    {
        if (!_active) return;
        foreach (var system in _executeSystems)
        {
            if (system.CanBeExecuted(world)) system.Execute(world);
        }
    }

    public void LateExecute(IWorld world)
    {
        if (!_active) return;
        foreach (var system in _lateExecuteSystems)
        {
            if (system.CanBeLateExecuted(world)) system.LateExecute(world);
        }
    }

    public void FrameDispose(IWorld world)
    {
        if (!_active) return;
        foreach (var system in _frameDisposeSystems) system.FrameDispose(world);
    }

    public void Dispose(IWorld world)
    {
        if (!_active) return;
        foreach (var system in _disposeSystems) system.Dispose(world);
    }
}
