using RelatedECS.Systems.SystemGroups;

namespace RelatedECS.Systems;

public interface ISystemsController
{
    public ISystemsController AddSystem(ISystem system);
    public ISystemsController AddGroup(ISystemGroup systemGroup);

    public ISystemGroup GetSystemGroup(string name);
    public IReadOnlyList<ISystem> GetAllSystems();
    public IReadOnlyList<ISystemGroup> GetAllGroups();


    public void Prepare(IWorld world);

    public void FramePrepare(IWorld world);

    public void Execute(IWorld world);

    public void LateExecute(IWorld world);

    public void FrameDispose(IWorld world);

    public void Dispose(IWorld world);
}
