namespace RelatedECS.Systems;

public interface IPrepareSystem : ISystem
{
    public void Prepare(ISystemsCollection collection);
}

public interface IFramePrepareSystem : ISystem
{
    public void FramePrepare(ISystemsCollection collection);
}

public interface IExecuteSystem : ISystem
{
    public bool CanBeExecuted(ISystemsCollection collection) => true;

    public void Execute(ISystemsCollection collection);
}

public interface ILateExecuteSystem : ISystem
{
    public bool CanBeLateExecuted(ISystemsCollection collection) => true;

    public void LateExecute(ISystemsCollection collection);
}

public interface IFrameDisposeSystem : ISystem
{
    public void FrameDispose(ISystemsCollection collection);
}

public interface IDisposeSystem : ISystem
{
    public void Dispose(ISystemsCollection collection);
}