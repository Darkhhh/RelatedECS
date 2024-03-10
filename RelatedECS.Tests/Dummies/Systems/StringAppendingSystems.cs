using RelatedECS.Systems;
using RelatedECS.Tests.Dummies.MaintenanceDataStructures;

namespace RelatedECS.Tests.Dummies.Systems;

internal class AppendStringPrepareSystem : IPrepareSystem
{
    private readonly StringData data;
    private readonly string initial;

    public AppendStringPrepareSystem(StringData data, string initial)
    {
        this.data = data;
        this.initial = initial;
    }

    public void Prepare(IWorld world)
    {
        data.Append(initial);
    }
}

internal class AppendStringFramePrepareSystem : IFramePrepareSystem
{
    private readonly StringData data;
    private readonly string initial;

    public AppendStringFramePrepareSystem(StringData data, string initial)
    {
        this.data = data;
        this.initial = initial;
    }

    public void FramePrepare(IWorld world)
    {
        data.Append(initial);
    }
}


internal class AppendStringExecuteSystem : IExecuteSystem
{
    private readonly StringData _data;
    private readonly string _appendix;
    private Func<bool>? _canBeExecuted;

    public AppendStringExecuteSystem(StringData data, string appendix, Func<bool>? canBeExecuted = null)
    {
        _data = data;
        _appendix = appendix;
        _canBeExecuted = canBeExecuted;
    }

    public bool CanBeExecuted(IWorld world)
    {
        return _canBeExecuted is null ? true : _canBeExecuted();
    }

    public void Execute(IWorld world)
    {
        _data.Append(_appendix);
    }
}


internal class AppendStringLateExecuteSystem : ILateExecuteSystem
{
    private readonly StringData _data;
    private readonly string _appendix;
    private Func<bool>? _canBeExecuted;

    public AppendStringLateExecuteSystem(StringData data, string appendix, Func<bool>? canBeExecuted = null)
    {
        _data = data;
        _appendix = appendix;
        _canBeExecuted = canBeExecuted;
    }

    public bool CanBeLateExecuted(IWorld world)
    {
        return _canBeExecuted is null ? true : _canBeExecuted();
    }

    public void LateExecute(IWorld world)
    {
        _data.Append(_appendix);
    }
}
