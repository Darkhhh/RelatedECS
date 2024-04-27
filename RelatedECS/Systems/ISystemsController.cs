using RelatedECS.Systems.SystemGroups;

namespace RelatedECS.Systems;

public interface ISystemsController
{
    public ISystemsController AddSystem(ISystem system);

    public ISystemsController AddGroup(ISystemGroup systemGroup);

    public ISystemGroup GetSystemGroup(string name);

    public IReadOnlyList<ISystem> GetAllSystems();

    public IReadOnlyList<ISystemGroup> GetAllGroups();

    public void Prepare(ISystemsCollection collection);

    public void FramePrepare(ISystemsCollection collection);

    public void Execute(ISystemsCollection collection);

    public void LateExecute(ISystemsCollection collection);

    public void FrameDispose(ISystemsCollection collection);

    public void Dispose(ISystemsCollection collection);
}