﻿namespace RelatedECS.Systems.SystemGroups;

public interface ISystemGroup : IPrepareSystem, IFramePrepareSystem, IExecuteSystem, ILateExecuteSystem, IFrameDisposeSystem, IDisposeSystem
{
    public string Name { get; }

    public ISystemGroup AddSystem(ISystem system);

    public IReadOnlyList<ISystem> GetAllSystems();

    public void Activate();

    public void Deactivate();
}
