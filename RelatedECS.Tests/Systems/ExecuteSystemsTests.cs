using RelatedECS.Tests.Dummies.MaintenanceDataStructures;
using RelatedECS.Tests.Dummies;
using RelatedECS.Tests.Dummies.Systems;
using RelatedECS.Systems;

namespace RelatedECS.Tests.Systems;

[TestClass]
public class ExecuteSystemsTests
{
    [TestMethod]
    public void CanBeExecutedSystemsCorrectExecution()
    {
        var world = new SystemsCollection(new WorldDummy());
        var data = new StringData();

        IExecuteSystem sys1;
        sys1 = new AppendStringExecuteSystem(data, nameof(sys1), () => true);

        IExecuteSystem sys2;
        sys2 = new AppendStringExecuteSystem(data, nameof(sys2), () => false);

        if (sys1.CanBeExecuted(world)) sys1.Execute(world);
        if (sys2.CanBeExecuted(world)) sys2.Execute(world);

        Assert.AreEqual("sys1", data.ToString());
    }

    [TestMethod]
    public void CanBeExecutedSystemsCorrectExecutionWithDefaultImplementation()
    {
        var world = new SystemsCollection(new WorldDummy());
        var data = new StringData();

        IExecuteSystem sys1;
        sys1 = new AppendStringExecuteSystem(data, nameof(sys1), () => true);

        IExecuteSystem sys2;
        sys2 = new AppendStringExecuteSystem(data, nameof(sys2), () => false);

        IExecuteSystem sys3;
        sys3 = new AppendStringSystem(data, nameof(sys3));

        if (sys1.CanBeExecuted(world)) sys1.Execute(world);
        if (sys2.CanBeExecuted(world)) sys2.Execute(world);
        if (sys3.CanBeExecuted(world)) sys3.Execute(world);

        Assert.AreEqual("sys1sys3", data.ToString());
    }

    [TestMethod]
    public void CanBeLateExecutedSystemsCorrectExecution()
    {
        var world = new SystemsCollection(new WorldDummy());
        var data = new StringData();

        ILateExecuteSystem sys1;
        sys1 = new AppendStringLateExecuteSystem(data, nameof(sys1), () => false);

        ILateExecuteSystem sys2;
        sys2 = new AppendStringLateExecuteSystem(data, nameof(sys2), () => true);

        if (sys1.CanBeLateExecuted(world)) sys1.LateExecute(world);
        if (sys2.CanBeLateExecuted(world)) sys2.LateExecute(world);

        Assert.AreEqual("sys2", data.ToString());
    }
}

internal class AppendStringSystem : IExecuteSystem
{
    private readonly StringData _data;
    private readonly string _appendix;

    public AppendStringSystem(StringData data, string appendix)
    {
        _appendix = appendix;
        _data = data;
    }

    public void Execute(ISystemsCollection world)
    {
        _data.Append(_appendix);
    }
}
