namespace RelatedECS.Systems;

public interface IPrepareSystem
{
    public void Prepare(IWorld world);
}

public interface IFramePrepareSystem
{
    public void FramePrepare(IWorld world);
}

public interface IExecuteSystem
{
    public bool CanBeExecuted(IWorld world) => true;

    public void Execute(IWorld world);
}

public interface ILateExecuteSystem
{
    public bool CanBeLateExecuted(IWorld world) => true;

    public void LateExecute(IWorld world);
}

public interface IFrameDisposeSystem
{
    public void FrameDispose(IWorld world);
}

public interface IDisposeSystem
{
    public void Dispose(IWorld world);
}
