using RelatedECS.Entities;
using RelatedECS.Tests.Dummies;

namespace RelatedECS.Tests.Entities.Relations;

[TestClass]
public class AddingRelations
{
    [TestMethod]
    public void CorrectAddingOneRelation()
    {
        var controller = new EntitiesController(new WorldDummy());
        var e1 = (Entity)controller.New();
        var e2 = (Entity)controller.New();

        e1.GetRelationsContainer().Add<SimpleRelation>(e2);

        var related = e1.GetRelationsContainer().Get<SimpleRelation>();
        Assert.AreEqual(1, related.Count());
        Assert.AreEqual(e2, related.First());
    }

    [TestMethod]
    public void CorrectAddingFewRelation()
    {
        var controller = new EntitiesController(new WorldDummy());
        var e1 = (Entity)controller.New();
        var e2 = (Entity)controller.New();
        var e3 = (Entity)controller.New();

        e1.GetRelationsContainer().Add<SimpleRelation>(e2);
        e1.GetRelationsContainer().Add<SimpleRelation>(e3);

        var related = e1.GetRelationsContainer().Get<SimpleRelation>();
        Assert.AreEqual(2, related.Count());
        CollectionAssert.AreEquivalent(new List<IEntity> { e2, e3 }, related.ToList());
    }

    [TestMethod]
    public void CorrectAddingSeveralRelationTypes()
    {
        var controller = new EntitiesController(new WorldDummy());
        var e1 = (Entity)controller.New();
        var e2 = (Entity)controller.New();
        var e3 = (Entity)controller.New();

        e1.GetRelationsContainer().Add<SimpleRelation>(e2);
        e1.GetRelationsContainer().Add<SimpleRelation1>(e3);

        var related = e1.GetRelationsContainer().Get<SimpleRelation>();
        Assert.AreEqual(1, related.Count());
        Assert.AreEqual(e2, related.First());
        var related1 = e1.GetRelationsContainer().Get<SimpleRelation1>();
        Assert.AreEqual(1, related1.Count());
        Assert.AreEqual(e3, related1.First());
    }

    [TestMethod]
    public void CorrectCyclicExceptionAdding()
    {
        var controller = new EntitiesController(new WorldDummy());
        var e1 = (Entity)controller.New();

        Assert.ThrowsException<Exception>(() => e1.GetRelationsContainer().Add<SimpleRelation>(e1));
    }

    [TestMethod]
    public void CorrectChainsCache()
    {
        var controller = new EntitiesController(new WorldDummy());
        var e1 = (Entity)controller.New();
        var e2 = (Entity)controller.New();
        var e3 = (Entity)controller.New();

        e1.GetRelationsContainer().Add<SimpleRelation>(e3);
        e2.GetRelationsContainer().Add<SimpleRelation1>(e3);

        CollectionAssert.AreEquivalent(new List<IEntity> { e1, e2 }, e3.GetRelationsContainer().GetChains());
    }

    [TestMethod]
    public void CorrectMixedRelations()
    {
        var controller = new EntitiesController(new WorldDummy());
        var e1 = (Entity)controller.New();
        var e2 = (Entity)controller.New();
        var e3 = (Entity)controller.New();

        e1.GetRelationsContainer().Add<SimpleRelation>(e3);
        e1.GetRelationsContainer().Add<SimpleRelation>(e2);
        e2.GetRelationsContainer().Add<SimpleRelation>(e1);
        e2.GetRelationsContainer().Add<SimpleRelation>(e3);
        e3.GetRelationsContainer().Add<SimpleRelation>(e1);
        e3.GetRelationsContainer().Add<SimpleRelation>(e2);

        CollectionAssert.AreEquivalent(new List<IEntity> { e1, e2 }, e3.GetRelationsContainer().GetChains());
        CollectionAssert.AreEquivalent(new List<IEntity> { e2, e3 }, e1.GetRelationsContainer().GetChains());
        CollectionAssert.AreEquivalent(new List<IEntity> { e1, e3 }, e2.GetRelationsContainer().GetChains());

        CollectionAssert.AreEquivalent(new List<IEntity> { e2, e3 }, e1.GetRelationsContainer().Get<SimpleRelation>().ToList());
        CollectionAssert.AreEquivalent(new List<IEntity> { e1, e3 }, e2.GetRelationsContainer().Get<SimpleRelation>().ToList());
        CollectionAssert.AreEquivalent(new List<IEntity> { e1, e2 }, e3.GetRelationsContainer().Get<SimpleRelation>().ToList());
    }

    private struct SimpleRelation;

    private struct SimpleRelation1;
}
