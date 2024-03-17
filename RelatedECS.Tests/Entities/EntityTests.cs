using RelatedECS.Tests.Dummies;
using RelatedECS.Tests.Dummies.Components;

namespace RelatedECS.Tests.Entities;

[TestClass]
public class EntityTests
{
    [TestMethod]
    public void CorrectEntityCreation()
    {
        var world = new WorldDummy();

        var e1 = world.NewEntity();

        Assert.IsTrue(e1.IsAlive);
        Assert.IsNotNull(e1.World);
        Assert.IsTrue(ReferenceEquals(e1.World, world));
    }

    [TestMethod]
    public void CorrectPoolAdd()
    {
        var world = new WorldDummy();

        var e1 = world.NewEntity();

        e1.Add<CPosition>();

        var pool = world.GetPool<CPosition>();

        Assert.IsTrue(pool.Has(e1.Id));
    }

    [TestMethod]
    public void CorrectPoolHas()
    {
        var world = new WorldDummy();

        var e1 = world.NewEntity();

        e1.Add<CPosition>();

        var pool = world.GetPool<CPosition>();

        Assert.AreEqual(pool.Has(e1.Id), e1.Has<CPosition>());
    }

    [TestMethod]
    public void CorrectPoolGet()
    {
        var world = new WorldDummy();

        var e1 = world.NewEntity();

        ref var p = ref e1.Add<CPosition>();
        p.X = 7;
        p.Y = 7;

        var pool = world.GetPool<CPosition>();

        ref var p1 = ref pool.Get(e1.Id);
        ref var p2 = ref e1.Get<CPosition>();

        Assert.AreEqual(p1.X, p2.X);
        Assert.AreEqual(p1.Y, p2.Y);

        Assert.AreEqual(7, p1.X);
        Assert.AreEqual(7, p1.Y);
    }

    [TestMethod]
    public void CorrectPoolDelete()
    {
        var world = new WorldDummy();

        var e1 = world.NewEntity();

        e1.Add<CPosition>();

        var pool = world.GetPool<CPosition>();

        Assert.IsTrue(pool.Has(e1.Id));

        e1.Delete<CPosition>();

        Assert.IsFalse(pool.Has(e1.Id));
    }
}
