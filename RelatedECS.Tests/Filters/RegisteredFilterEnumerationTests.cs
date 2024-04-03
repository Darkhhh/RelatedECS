using RelatedECS.Entities;
using RelatedECS.Filters;
using RelatedECS.Maintenance.Utilities;
using RelatedECS.Pools;
using RelatedECS.Tests.Dummies;

namespace RelatedECS.Tests.Filters;

[TestClass]
public class RegisteredFilterEnumerationTests
{
    [TestMethod]
    public void CorrectEnumeration()
    {
        const int entitiesNumber = 5;
        var f = new RegisteredFilter(_entitiesController, _masks.With, _masks.Without);
        var set = new HashSet<Entity>();
        for (var i = 0; i < entitiesNumber; i++)
        {
            var e = NewSuitableEntity();
            set.Add(e);
            Assert.AreEqual(1, f.CheckEntity(e));
        }
        Assert.AreEqual(entitiesNumber, f.Count);

        var count = 0;
        Assert.ThrowsException<Exception>(() => f.GetInitialIndex());
        f.Lock();
        var prev = f.GetInitialIndex();
        while (f.Next(prev, out int next))
        {
            prev = next;
            var e = f.Get(next);
            Assert.IsTrue(set.Contains(e));
            count++;
        }
        f.Unlock();
        Assert.AreEqual(entitiesNumber, count);
    }

    [TestMethod]
    public void CorrectEnumerationWithDeletions()
    {
        const int entitiesNumber = 5;
        var f = new RegisteredFilter(_entitiesController, _masks.With, _masks.Without);
        for (var i = 0; i < entitiesNumber; i++)
        {
            Assert.AreEqual(1, f.CheckEntity(NewSuitableEntity()));
        }
        Assert.AreEqual(entitiesNumber, f.Count);
        f.Lock();
        var prev = f.GetInitialIndex();
        var deleted = 0;
        while (f.Next(prev, out int next))
        {
            prev = next;
            var e = f.Get(next);
            if (Random.Shared.NextDouble() < 0.5)
            {
                deleted++;
                ((ComponentsPool<C0>)_pools[0]).Delete(e.Id);
                Assert.AreEqual(-1, f.CheckEntity(e));
            }
        }
        Assert.AreEqual(entitiesNumber, f.Count);
        f.Unlock();
        Assert.AreEqual(entitiesNumber - deleted, f.Count);
    }

    [TestMethod]
    public void CorrectEnumerationWithAdditions()
    {
        const int entitiesNumber = 5;
        var f = new RegisteredFilter(_entitiesController, _masks.With, _masks.Without);
        for (var i = 0; i < entitiesNumber; i++)
        {
            Assert.AreEqual(1, f.CheckEntity(NewSuitableEntity()));
        }
        Assert.AreEqual(entitiesNumber, f.Count);
        f.Lock();
        var prev = f.GetInitialIndex();
        var added = 0;
        while (f.Next(prev, out int next))
        {
            prev = next;
            var e = f.Get(next);
            if (Random.Shared.NextDouble() < 0.5)
            {
                added++;
                Assert.AreEqual(1, f.CheckEntity(NewSuitableEntity()));
            }
        }
        Assert.AreEqual(entitiesNumber, f.Count);
        f.Unlock();
        Assert.AreEqual(entitiesNumber + added, f.Count);
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
