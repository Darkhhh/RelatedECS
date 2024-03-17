using RelatedECS.Entities;
using RelatedECS.Maintenance.Utilities;
using RelatedECS.Pools;
using RelatedECS.Tests.Dummies;
using RelatedECS.Tests.Dummies.Components;

namespace RelatedECS.Tests.Entities;

[TestClass]
public class EntitiesControllerTests
{
    [TestMethod]
    public void CorrectEntitiesCreation()
    {
        var entities = new EntitiesController(new WorldDummy());
        var components = new ComponentsPoolsController(entities.PoolUpdated);

        var world = new WorldDummy();
        var set = new HashSet<int>();

        for (int i = 0; i < 10; i++)
        {
            var e = entities.New();
            Assert.IsNotNull(e);
            Assert.IsFalse(set.Contains(e.Id));
            set.Add(e.Id);
        }
    }

    [TestMethod]
    public void CorrectPoolAdd()
    {
        var world = new WorldDummy();

        var e1 = world.EntitiesController.New(); ;

        e1.Add<CPosition>();
        var pool = world.GetPool<CPosition>();

        Assert.IsTrue(pool.Has(e1.Id));
        Assert.IsTrue(world.EntitiesController.GetWrapById(e1.Id).GetMask().Get(pool.Id));
    }

    [TestMethod]
    public void CorrectGetRaw()
    {
        var world = new WorldDummy();
        var set = new HashSet<IEntity>();
        for (int i = 0; i < 10; i++)
        {
            set.Add(world.EntitiesController.New());
        }

        var raw = world.EntitiesController.GetEntitiesRaw();
        foreach (var entity in raw)
        {
            Assert.IsTrue(set.Contains(entity));
        }

        foreach (var entity in set)
        {
            Assert.IsTrue(raw.Contains(entity));
        }
    }

    [TestMethod]
    public void CorrectEntityReset()
    {
        var world = new WorldDummy();
        var e1 = world.EntitiesController.New();

        e1.Add<CPosition>();

        e1.Delete<CPosition>();

        Assert.IsFalse(e1.IsAlive);

        Entity? e = e1 as Entity;
        Assert.IsNotNull(e);
        Assert.IsTrue(e.GetMask().IsEmpty);
    }

    [TestMethod]
    public void CorrectGetById()
    {
        var world = new WorldDummy();
        var e1 = world.EntitiesController.New();

        var e2 = world.EntitiesController.GetById(e1.Id);

        Assert.IsTrue(ReferenceEquals(e1,e2));

        var e3 = world.EntitiesController.GetWrapById(e1.Id);

        Assert.IsTrue(ReferenceEquals(e1, e3));
        Assert.IsTrue(ReferenceEquals(e2, e3));
    }

    [TestMethod]
    public void GetByIdExceptions()
    {
        var world = new WorldDummy();
        var e1 = world.EntitiesController.New();

        e1.Add<CPosition>();

        e1.Delete<CPosition>();

        Assert.ThrowsException<InvalidOperationException>(() =>
        {
            world.EntitiesController.GetById(e1.Id);
        });
        Assert.ThrowsException<InvalidOperationException>(() =>
        {
            world.EntitiesController.GetWrapById(e1.Id);
        });
    }

    [TestMethod]
    public void CorrectPoolsAmountResize()
    {
        var world = new WorldDummy();
        var set = new HashSet<Entity>();
        for (int i = 0; i < 10; i++)
        {
            var e = world.EntitiesController.New() as Entity;
            Assert.IsNotNull(e);
            set.Add(e);
        }

        world.EntitiesController.UpdatePoolsAmount(Mask.BitSize - 2);

        foreach (var e in set)
        {
            Assert.AreEqual(2, e.GetMask().Length);
        }

        world.EntitiesController.UpdatePoolsAmount(2 * Mask.BitSize - 2);

        foreach (var e in set)
        {
            Assert.AreEqual(2, e.GetMask().Length);
        }

        world.EntitiesController.UpdatePoolsAmount(3 * Mask.BitSize - 2);

        foreach (var e in set)
        {
            Assert.AreEqual(3, e.GetMask().Length);
        }
    }
}
