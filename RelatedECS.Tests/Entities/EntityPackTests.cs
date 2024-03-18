using RelatedECS.Entities;
using RelatedECS.Tests.Dummies;
using RelatedECS.Tests.Dummies.Components;

namespace RelatedECS.Tests.Entities;

[TestClass]
public class EntityPackTests
{
    [TestMethod]
    public void CorrectCreation()
    {
        var world = new WorldDummy();
        var e = world.EntitiesController.New();
        e.Add<CPosition>();
        var wrap = e as Entity;
        Assert.IsNotNull(wrap);

        var pack = new EntityPack(wrap, wrap.Generation);
        Assert.IsNotNull(pack);
    }

    [TestMethod]
    public void CorrectUnpackForAliveEntity()
    {
        var world = new WorldDummy();
        var e = world.EntitiesController.New();
        e.Add<CPosition>();
        var wrap = e as Entity;
        Assert.IsNotNull(wrap);

        var pack = new EntityPack(wrap, wrap.Generation);

        Assert.IsTrue(pack.TryGet(out var entity));
        Assert.IsTrue(ReferenceEquals(entity, e));
    }

    [TestMethod]
    public void CorrectUnpackForDeadEntity()
    {
        var world = new WorldDummy();
        var e = world.EntitiesController.New();
        e.Add<CPosition>();
        var wrap = e as Entity;
        Assert.IsNotNull(wrap);

        var pack = new EntityPack(wrap, wrap.Generation);

        e.Delete<CPosition>();

        Assert.IsFalse(pack.TryGet(out var entity));
        Assert.IsNull(entity);
    }

    [TestMethod]
    public void CorrectUnpackForNewGenerationEntity()
    {
        var world = new WorldDummy();
        var e = world.EntitiesController.New();
        e.Add<CPosition>();
        var wrap = e as Entity;
        Assert.IsNotNull(wrap);

        var pack = new EntityPack(wrap, wrap.Generation);

        e.Delete<CPosition>();

        var e1 = world.EntitiesController.New();
        Assert.IsTrue(ReferenceEquals(e, e1));

        Assert.IsFalse(pack.TryGet(out var entity));
        Assert.IsNull(entity);
    }
}
