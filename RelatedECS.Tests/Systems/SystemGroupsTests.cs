using RelatedECS.Systems;
using RelatedECS.Systems.SystemGroups;
using RelatedECS.Tests.Dummies;
using RelatedECS.Tests.Dummies.MaintenanceDataStructures;
using RelatedECS.Tests.Dummies.Systems;

namespace RelatedECS.Tests.Systems;

[TestClass]
public class SystemGroupsTests
{
    [TestMethod]
    public void CorrectExecutionOrder()
    {
        var world = new SystemsCollection(new WorldDummy());
        var data = new StringData();
        var group = new SystemGroup("Test")
            .AppendSystem(new AppendStringPrepareSystem(data, "0"))
            .AppendSystem(new AppendStringFramePrepareSystem(data, "fp"))
            .AppendSystem(new AppendStringExecuteSystem(data, "1"))
            .AppendSystem(new AppendStringExecuteSystem(data, "2"))
            .AppendSystem(new AppendStringLateExecuteSystem(data, "3"))
            .AppendSystem(new AppendStringLateExecuteSystem(data, "4"));

        group.Prepare(world);
        group.FramePrepare(world);
        group.Execute(world);
        group.LateExecute(world);

        Assert.AreEqual("0fp1234", data.ToString());

        group.Execute(world);
        group.LateExecute(world);

        Assert.AreEqual("0fp12341234", data.ToString());
    }


    [TestMethod]
    public void SystemGroupDeactivation()
    {
        var world = new SystemsCollection(new WorldDummy());
        var data = new StringData();
        var group = new SystemGroup("Test")
            .AppendSystem(new AppendStringPrepareSystem(data, "0"))
            .AppendSystem(new AppendStringFramePrepareSystem(data, "fp"))
            .AppendSystem(new AppendStringExecuteSystem(data, "1"))
            .AppendSystem(new AppendStringExecuteSystem(data, "2"))
            .AppendSystem(new AppendStringLateExecuteSystem(data, "3"))
            .AppendSystem(new AppendStringLateExecuteSystem(data, "4"));

        group.Prepare(world);
        group.FramePrepare(world);
        group.Execute(world);
        group.LateExecute(world);

        Assert.AreEqual("0fp1234", data.ToString());

        group.Deactivate();
        group.Execute(world);
        group.LateExecute(world);

        Assert.AreEqual("0fp1234", data.ToString());
    }

    [TestMethod]
    public void SystemGroupDeactivationActivation()
    {
        var world = new SystemsCollection(new WorldDummy());
        var data = new StringData();
        var group = new SystemGroup("Test")
            .AppendSystem(new AppendStringPrepareSystem(data, "0"))
            .AppendSystem(new AppendStringFramePrepareSystem(data, "fp"))
            .AppendSystem(new AppendStringExecuteSystem(data, "1"))
            .AppendSystem(new AppendStringExecuteSystem(data, "2"))
            .AppendSystem(new AppendStringLateExecuteSystem(data, "3"))
            .AppendSystem(new AppendStringLateExecuteSystem(data, "4"));

        group.Prepare(world);
        group.FramePrepare(world);
        group.Execute(world);
        group.LateExecute(world);

        Assert.AreEqual("0fp1234", data.ToString());

        group.Deactivate();
        group.Execute(world);
        group.LateExecute(world);

        Assert.AreEqual("0fp1234", data.ToString());

        group.Activate();
        group.Execute(world);
        group.LateExecute(world);

        Assert.AreEqual("0fp12341234", data.ToString());
    }


    [TestMethod]
    public void SystemGroupSystemsCount()
    {
        var data = new StringData();
        var group = new SystemGroup("Test")
            .AppendSystem(new AppendStringPrepareSystem(data, "0"))
            .AppendSystem(new AppendStringFramePrepareSystem(data, "fp"))
            .AppendSystem(new AppendStringExecuteSystem(data, "1"))
            .AppendSystem(new AppendStringExecuteSystem(data, "2"))
            .AppendSystem(new AppendStringLateExecuteSystem(data, "3"))
            .AppendSystem(new AppendStringLateExecuteSystem(data, "4"));

        var systems = group.GetAllSystems();
        Assert.AreEqual(6, systems.Count);
    }

    [TestMethod]
    public void CorrectNameAssignment()
    {
        var group = new SystemGroup("Test");
        Assert.AreEqual("Test", group.Name);
    }
}
