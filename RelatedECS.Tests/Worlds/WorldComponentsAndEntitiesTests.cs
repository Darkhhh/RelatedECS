using RelatedECS.Entities;
using RelatedECS.Tests.Dummies.Components;

namespace RelatedECS.Tests.Worlds;

[TestClass]
public class WorldComponentsAndEntitiesTests
{
    [TestMethod]
    public void CorrectNewEntitiesCreating()
    {
        var world = new World();

        var e1 = world.NewEntity();
        var e2 = world.NewEntity();
        var e3 = world.NewEntity();
        var e4 = world.NewEntity();
        var e5 = world.NewEntity();

        Assert.IsNotNull(e1);
        Assert.IsNotNull(e2);
        Assert.IsNotNull(e3);
        Assert.IsNotNull(e4);
        Assert.IsNotNull(e5);

        var list = new List<IEntity> { e1, e2, e3, e4, e5 };
        Assert.IsTrue(list.Select(e => e.Id).ToHashSet().Count == 5);
    }

    [TestMethod]
    public void CorrectPoolsGet()
    {
        var world = new World();
        var pool1 = world.GetPool<CPosition>();
        var pool2 = world.GetPool(typeof(CPosition));
        Assert.IsTrue(ReferenceEquals(pool1, pool2));

        Assert.ThrowsException<Exception>(()=>world.GetPool(typeof(CRange)));
    }

    [TestMethod]
    public void CorrectEntitiesMasksChange()
    {
        var world = new World();
        var e1 = world.NewEntity() as Entity;
        Assert.IsNotNull(e1);
        ref var position = ref e1.Add<CPosition>();

        var mask = e1.GetMask();
        Assert.IsFalse(mask.IsEmpty);
        Assert.AreEqual<ulong>(1, mask[0]);

        e1.Delete<CPosition>();
        Assert.IsTrue(mask.IsEmpty);
        Assert.AreEqual<ulong>(0, mask[0]);
    }

    [TestMethod]
    public void CorrectGetById()
    {
        var world = new World();
        var e1 = world.NewEntity();
        var e2 = world.GetEntityById(e1.Id);
        Assert.IsTrue(ReferenceEquals(e1, e2));
    }
}
