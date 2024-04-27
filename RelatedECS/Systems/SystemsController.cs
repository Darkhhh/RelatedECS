using RelatedECS.Systems.SystemGroups;

namespace RelatedECS.Systems;

public class SystemsController : ISystemsController
{
    private const int InitialCapacity = 8;

    private readonly List<ISystem> _allSystems = new(InitialCapacity);
    private readonly List<IPrepareSystem> _prepareSystems = new(InitialCapacity);
    private readonly List<IFramePrepareSystem> _framePrepareSystems = new(InitialCapacity);
    private readonly List<IExecuteSystem> _executeSystems = new(InitialCapacity);
    private readonly List<ILateExecuteSystem> _lateExecuteSystems = new(InitialCapacity);
    private readonly List<IFrameDisposeSystem> _frameDisposeSystems = new(InitialCapacity);
    private readonly List<IDisposeSystem> _disposeSystems = new(InitialCapacity);

    private readonly List<ISystemGroup> _groupsList = new(InitialCapacity);
    private readonly Dictionary<string, ISystemGroup> _systemGroups = new(InitialCapacity);

    public ISystemsController AddSystem(ISystem system)
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

    public ISystemsController AddGroup(ISystemGroup systemGroup)
    {
        if (_systemGroups.ContainsKey(systemGroup.Name)) throw new Exception($"SystemGroup with name {systemGroup.Name} already exists");

        _systemGroups.Add(systemGroup.Name, systemGroup);
        _groupsList.Add(systemGroup);
        _prepareSystems.Add(systemGroup);
        _framePrepareSystems.Add(systemGroup);
        _executeSystems.Add(systemGroup);
        _lateExecuteSystems.Add(systemGroup);
        _frameDisposeSystems.Add(systemGroup);
        _disposeSystems.Add(systemGroup);

        return this;
    }

    public void Prepare(ISystemsCollection collection)
    {
        foreach (var system in _prepareSystems) system.Prepare(collection);
    }

    public void FramePrepare(ISystemsCollection collection)
    {
        foreach (var system in _framePrepareSystems) system.FramePrepare(collection);
    }

    public void Execute(ISystemsCollection collection)
    {
        foreach (var system in _executeSystems)
        {
            if (system.CanBeExecuted(collection)) system.Execute(collection);
        }
    }

    public void LateExecute(ISystemsCollection collection)
    {
        foreach (var system in _lateExecuteSystems)
        {
            if (system.CanBeLateExecuted(collection)) system.LateExecute(collection);
        }
    }

    public void FrameDispose(ISystemsCollection collection)
    {
        foreach (var system in _frameDisposeSystems) system.FrameDispose(collection);
    }

    public void Dispose(ISystemsCollection collection)
    {
        foreach (var system in _disposeSystems) system.Dispose(collection);
    }

    public IReadOnlyList<ISystem> GetAllSystems() => _allSystems;

    public ISystemGroup GetSystemGroup(string name)
    {
        if (!_systemGroups.TryGetValue(name, out var systemGroup)) throw new Exception($"SystemGroup with name {name} was not added");
        return systemGroup;
    }

    public IReadOnlyList<ISystemGroup> GetAllGroups() => _groupsList;
}