using RelatedECS.Systems;
using RelatedECS.Systems.SystemGroups;
using RelatedECS.Tests.Dummies;
using RelatedECS.Tests.Dummies.MaintenanceDataStructures;
using RelatedECS.Tests.Dummies.Systems;

namespace RelatedECS.Tests.Systems;

[TestClass]
public class SystemsControllerTests
{
    private ISystemsController _systemsController = null!;
    private StringData _data = null!;

    [TestMethod]
    public void AddingSystemsAndGroups()
    {
        var allSystems = _systemsController.GetAllSystems();
        var groups = _systemsController.GetAllGroups();

        Assert.AreEqual(8, allSystems.Count);
        Assert.AreEqual(1, groups.Count);
    }

    [TestMethod]
    public void SystemsAndGroupsExecutionOrder()
    {
        var world = new WorldDummy();

        _systemsController.Prepare(world);
        _systemsController.FramePrepare(world);
        _systemsController.Execute(world);
        _systemsController.LateExecute(world);
        _systemsController.FrameDispose(world);
        _systemsController.Dispose(world);

        Assert.AreEqual("psfps0g1g2g3123lg1l1l2", _data.ToString());
    }

    [TestMethod]
    public void GetOrAddSystemGroup()
    {
        Assert.ThrowsException<Exception>(() =>
        {
            _systemsController.AddGroup(new SystemGroup("Group1"));
        });
        Assert.ThrowsException<Exception>(() =>
        {
            _systemsController.GetSystemGroup("Group2");
        });

        Assert.IsNotNull(_systemsController.GetSystemGroup("Group1"));

        _systemsController.AddGroup(new SystemGroup("Group2"));

        Assert.AreEqual(2, _systemsController.GetAllGroups().Count);
    }

    [TestInitialize]
    public void Init()
    {
        _systemsController = new SystemsController();
        _data = new StringData();
        _systemsController
            .AddSystem(new AppendStringPrepareSystem(_data, "ps"))
            .AddSystem(new AppendStringFramePrepareSystem(_data, "fps"))
            .AddSystem(new AppendStringExecuteSystem(_data, "0"))
            .AddGroup(new SystemGroup("Group1")
                            .AppendSystem(new AppendStringExecuteSystem(_data, "g1"))
                            .AppendSystem(new AppendStringExecuteSystem(_data, "g2"))
                            .AppendSystem(new AppendStringExecuteSystem(_data, "g3"))
                            .AppendSystem(new AppendStringLateExecuteSystem(_data, "lg1"))
                            )
            .AddSystem(new AppendStringExecuteSystem(_data, "1"))
            .AddSystem(new AppendStringExecuteSystem(_data, "2"))
            .AddSystem(new AppendStringExecuteSystem(_data, "3"))
            .AddSystem(new AppendStringLateExecuteSystem(_data, "l1"))
            .AddSystem(new AppendStringLateExecuteSystem(_data, "l2"));
    }
}
