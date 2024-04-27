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

    public void Prepare(ISystemsCollection collection)
    {
        if (!_active) return;
        foreach (var system in _prepareSystems) system.Prepare(collection);
    }

    public void FramePrepare(ISystemsCollection collection)
    {
        if (!_active) return;
        foreach (var system in _framePrepareSystems) system.FramePrepare(collection);
    }

    public void Execute(ISystemsCollection collection)
    {
        if (!_active) return;
        foreach (var system in _executeSystems)
        {
            if (system.CanBeExecuted(collection)) system.Execute(collection);
        }
    }

    public void LateExecute(ISystemsCollection collection)
    {
        if (!_active) return;
        foreach (var system in _lateExecuteSystems)
        {
            if (system.CanBeLateExecuted(collection)) system.LateExecute(collection);
        }
    }

    public void FrameDispose(ISystemsCollection collection)
    {
        if (!_active) return;
        foreach (var system in _frameDisposeSystems) system.FrameDispose(collection);
    }

    public void Dispose(ISystemsCollection collection)
    {
        if (!_active) return;
        foreach (var system in _disposeSystems) system.Dispose(collection);
    }
}