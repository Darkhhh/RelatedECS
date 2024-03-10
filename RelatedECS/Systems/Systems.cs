namespace RelatedECS.Systems;

public interface IPrepareSystem : ISystem
{
    public void Prepare(IWorld world);
}

public interface IFramePrepareSystem : ISystem
{
    public void FramePrepare(IWorld world);
}

public interface IExecuteSystem : ISystem
{
    public bool CanBeExecuted(IWorld world) => true;

    public void Execute(IWorld world);
}

public interface ILateExecuteSystem : ISystem
{
    public bool CanBeLateExecuted(IWorld world) => true;

    public void LateExecute(IWorld world);
}

public interface IFrameDisposeSystem : ISystem
{
    public void FrameDispose(IWorld world);
}

public interface IDisposeSystem : ISystem
{
    public void Dispose(IWorld world);
}
