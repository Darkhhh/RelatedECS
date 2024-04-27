using RelatedECS.Entities;
using RelatedECS.Filters;
using RelatedECS.Maintenance.Utilities;
using RelatedECS.Pools;
using RelatedECS.Tests.Dummies;

namespace RelatedECS.Tests.Filters;

[TestClass]
public class EntitiesFilterTests
{
    [TestMethod]
    public void CorrectCount()
    {
        var declaration = new FilterDeclaration(new WorldDummy());
        var f = new RegisteredFilter(_entitiesController, _masks.With, _masks.Without);
        const int entitiesNumber = 5;
        for (int i = 0; i < entitiesNumber; i++) f.CheckEntity(NewSuitableEntity());

        var filter = new EntitiesFilter(f, declaration);

        Assert.AreEqual(f.Count, filter.Count);
        Assert.AreEqual(entitiesNumber, filter.Count);
    }

    [TestMethod]
    public void CorrectDeclarationAssignment()
    {
        var declaration = new FilterDeclaration(new WorldDummy());
        var f = new RegisteredFilter(_entitiesController, _masks.With, _masks.Without);

        var filter = new EntitiesFilter(f, declaration);

        Assert.IsTrue(ReferenceEquals(filter.Declaration, declaration));
    }

    [TestMethod]
    public void CorrectRawFilterAssignment()
    {
        var declaration = new FilterDeclaration(new WorldDummy());
        var f = new RegisteredFilter(_entitiesController, _masks.With, _masks.Without);

        var filter = new EntitiesFilter(f, declaration);

        Assert.IsTrue(ReferenceEquals(filter.Raw, f));
    }

    [TestMethod]
    public void CorrectOneCycle()
    {
        var declaration = new FilterDeclaration(new WorldDummy());
        var f = new RegisteredFilter(_entitiesController, _masks.With, _masks.Without);
        const int entitiesNumber = 5;
        for (int i = 0; i < entitiesNumber; i++) f.CheckEntity(NewSuitableEntity());

        var filter = new EntitiesFilter(f, declaration);
        var count = 0;

        foreach (var e in filter.Entities())
        {
            Assert.IsTrue(f.IsLocked);
            count++;
        }
        Assert.AreEqual(entitiesNumber, count);
    }

    [TestMethod]
    public void CorrectTwoCycles()
    {
        var declaration = new FilterDeclaration(new WorldDummy());
        var f = new RegisteredFilter(_entitiesController, _masks.With, _masks.Without);
        const int entitiesNumber = 5;
        for (int i = 0; i < entitiesNumber; i++) f.CheckEntity(NewSuitableEntity());

        var filter = new EntitiesFilter(f, declaration);
        var count = 0;

        foreach (var e in filter.Entities())
        {
            Assert.IsTrue(f.IsLocked);
            foreach (var e2 in filter.Entities())
            {
                Assert.IsTrue(f.IsLocked);
                count++;
            }
            Assert.IsTrue(f.IsLocked);
        }
        Assert.AreEqual(entitiesNumber * entitiesNumber, count);
    }



    private Entity NewSuitableEntity()
    {
        var e = _entitiesController.New();
        ((ComponentsPool<C0>)_pools[0]).Add(e.Id);
        ((ComponentsPool<C4>)_pools[4]).Add(e.Id);
        ((ComponentsPool<C7>)_pools[7]).Add(e.Id);
        return (Entity)e;
    }
    private Entity NewNotSuitableEntity()
    {
        var e = _entitiesController.New();
        ((ComponentsPool<C0>)_pools[0]).Add(e.Id);
        ((ComponentsPool<C4>)_pools[4]).Add(e.Id);
        ((ComponentsPool<C7>)_pools[7]).Add(e.Id);
        ((ComponentsPool<C8>)_pools[8]).Add(e.Id);
        return (Entity)e;
    }

    private ComponentsPoolsController _poolsController = null!;
    private EntitiesController _entitiesController = null!;
    private List<IComponentsPool> _pools = new();
    private (Mask With, Mask Without) _masks;

    [TestInitialize]
    public void Initialize()
    {
        _entitiesController = new EntitiesController(new DisabledWorldDummy());
        _poolsController = new ComponentsPoolsController(_entitiesController.PoolUpdated);
        _pools = [_poolsController.GetPool<C0>(), _poolsController.GetPool<C1>(), _poolsController.GetPool<C2>(),
                    _poolsController.GetPool<C3>(), _poolsController.GetPool<C4>(), _poolsController.GetPool<C5>(),
                    _poolsController.GetPool<C6>(), _poolsController.GetPool<C7>(), _poolsController.GetPool<C8>(), _poolsController.GetPool<C9>()];
        _masks = _poolsController.GetMasks(
            [typeof(C0), typeof(C4), typeof(C7)],
            [typeof(C1), typeof(C8), typeof(C9)]);
    }

    private struct C0; private struct C1; private struct C2; private struct C3; private struct C4;
    private struct C5; private struct C6; private struct C7; private struct C8; private struct C9;
}
