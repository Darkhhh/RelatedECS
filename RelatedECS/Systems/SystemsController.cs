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


    public ISystemsController Add(ISystem system)
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

    public ISystemsController Add(ISystemGroup systemGroup)
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


    public void Prepare(IWorld world)
    {
        foreach (var system in _prepareSystems) system.Prepare(world);
    }

    public void FramePrepare(IWorld world)
    {
        foreach (var system in _framePrepareSystems) system.FramePrepare(world);
    }

    public void Execute(IWorld world)
    {
        foreach (var system in _executeSystems)
        {
            if (system.CanBeExecuted(world)) system.Execute(world);
        }
    }

    public void LateExecute(IWorld world)
    {
        foreach (var system in _lateExecuteSystems)
        {
            if (system.CanBeLateExecuted(world)) system.LateExecute(world);
        }
    }

    public void FrameDispose(IWorld world)
    {
        foreach (var system in _frameDisposeSystems) system.FrameDispose(world);
    }

    public void Dispose(IWorld world)
    {
        foreach (var system in _disposeSystems) system.Dispose(world);
    }

    public IReadOnlyList<ISystem> GetAllSystems() => _allSystems;

    public ISystemGroup GetSystemGroup(string name)
    {
        if (!_systemGroups.TryGetValue(name, out var systemGroup)) throw new Exception($"SystemGroup with name {name} was not added");
        return systemGroup;
    }

    public IReadOnlyList<ISystemGroup> GetAllGroups() => _groupsList;
}
