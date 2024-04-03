using RelatedECS.Entities;
using RelatedECS.Filters;
using RelatedECS.Pools;
using RelatedECS.Tests.Dummies;

namespace RelatedECS.Tests.Filters;

[TestClass]
public class RegisteredFilterTests
{
    [TestMethod]
    public void CorrectSuitableEntitiesCheck()
    {
        var masks = _poolsController.GetMasks(
            [typeof(C0), typeof(C4), typeof(C7)],
            [typeof(C1), typeof(C8), typeof(C9)]);

        var f = new RegisteredFilter(_entitiesController, masks.With, masks.Without);

        var e = _entitiesController.New();
        ((ComponentsPool<C0>)_pools[0]).Add(e.Id);
        ((ComponentsPool<C4>)_pools[4]).Add(e.Id);
        ((ComponentsPool<C7>)_pools[7]).Add(e.Id);

        Assert.AreEqual(1, f.CheckEntity((Entity)e));
        Assert.AreEqual(1, f.Count);

        var e2 = _entitiesController.New();
        ((ComponentsPool<C0>)_pools[0]).Add(e2.Id);
        ((ComponentsPool<C4>)_pools[4]).Add(e2.Id);
        ((ComponentsPool<C7>)_pools[7]).Add(e2.Id);
        ((ComponentsPool<C3>)_pools[3]).Add(e2.Id);
        Assert.AreEqual(1, f.CheckEntity((Entity)e2));
        Assert.AreEqual(2, f.Count);

        Assert.AreEqual(0, f.CheckEntity((Entity)e));
        Assert.AreEqual(0, f.CheckEntity((Entity)e2));
    }

    [TestMethod]
    public void CorrectNotSuitableEntitiesCheck()
    {
        var masks = _poolsController.GetMasks(
            [typeof(C0), typeof(C4), typeof(C7)],
            [typeof(C1), typeof(C8), typeof(C9)]);

        var f = new RegisteredFilter(_entitiesController, masks.With, masks.Without);

        var e = _entitiesController.New();
        ((ComponentsPool<C0>)_pools[0]).Add(e.Id);
        ((ComponentsPool<C4>)_pools[4]).Add(e.Id);
        ((ComponentsPool<C7>)_pools[7]).Add(e.Id);

        Assert.AreEqual(1, f.CheckEntity((Entity)e));
        Assert.AreEqual(1, f.Count);

        var e2 = _entitiesController.New();
        ((ComponentsPool<C0>)_pools[0]).Add(e2.Id);
        ((ComponentsPool<C4>)_pools[4]).Add(e2.Id);
        ((ComponentsPool<C7>)_pools[7]).Add(e2.Id);
        ((ComponentsPool<C8>)_pools[8]).Add(e2.Id);
        Assert.AreEqual(0, f.CheckEntity((Entity)e2));
        Assert.AreEqual(1, f.Count);

        ((ComponentsPool<C0>)_pools[0]).Delete(e.Id);

        Assert.AreEqual(-1, f.CheckEntity((Entity)e));
        Assert.AreEqual(0, f.Count);
    }

    [TestMethod]
    public void CorrectLockAdding()
    {
        var masks = _poolsController.GetMasks(
            [typeof(C0), typeof(C4), typeof(C7)],
            [typeof(C1), typeof(C8), typeof(C9)]);

        var f = new RegisteredFilter(_entitiesController, masks.With, masks.Without);

        f.Lock();

        var e = _entitiesController.New();
        ((ComponentsPool<C0>)_pools[0]).Add(e.Id);
        ((ComponentsPool<C4>)_pools[4]).Add(e.Id);
        ((ComponentsPool<C7>)_pools[7]).Add(e.Id);
        var e2 = _entitiesController.New();
        ((ComponentsPool<C0>)_pools[0]).Add(e2.Id);
        ((ComponentsPool<C4>)_pools[4]).Add(e2.Id);
        ((ComponentsPool<C7>)_pools[7]).Add(e2.Id);
        ((ComponentsPool<C3>)_pools[3]).Add(e2.Id);

        Assert.AreEqual(1, f.CheckEntity((Entity)e));
        Assert.AreEqual(1, f.CheckEntity((Entity)e2));
        Assert.AreEqual(0, f.Count);

        f.Unlock();
        Assert.AreEqual(2, f.Count);
    }

    [TestMethod]
    public void CorrectLockRemoving()
    {
        var masks = _poolsController.GetMasks(
            [typeof(C0), typeof(C4), typeof(C7)],
            [typeof(C1), typeof(C8), typeof(C9)]);

        var f = new RegisteredFilter(_entitiesController, masks.With, masks.Without);

        var e = _entitiesController.New();
        ((ComponentsPool<C0>)_pools[0]).Add(e.Id);
        ((ComponentsPool<C4>)_pools[4]).Add(e.Id);
        ((ComponentsPool<C7>)_pools[7]).Add(e.Id);
        var e2 = _entitiesController.New();
        ((ComponentsPool<C0>)_pools[0]).Add(e2.Id);
        ((ComponentsPool<C4>)_pools[4]).Add(e2.Id);
        ((ComponentsPool<C7>)_pools[7]).Add(e2.Id);
        ((ComponentsPool<C3>)_pools[3]).Add(e2.Id);

        Assert.AreEqual(1, f.CheckEntity((Entity)e));
        Assert.AreEqual(1, f.CheckEntity((Entity)e2));
        Assert.AreEqual(2, f.Count);

        f.Lock();

        ((ComponentsPool<C0>)_pools[0]).Delete(e.Id);

        Assert.AreEqual(-1, f.CheckEntity((Entity)e));
        Assert.AreEqual(2, f.Count);

        f.Unlock();
        Assert.AreEqual(1, f.Count);
    }

    [TestMethod]
    public void CorrectLockState()
    {
        var masks = _poolsController.GetMasks(
            [typeof(C0), typeof(C4), typeof(C7)],
            [typeof(C1), typeof(C8), typeof(C9)]);

        var f = new RegisteredFilter(_entitiesController, masks.With, masks.Without);
        Assert.IsFalse(f.IsLocked);

        f.Lock();
        Assert.IsTrue(f.IsLocked);

        f.Unlock();
        Assert.IsFalse(f.IsLocked);

        Assert.ThrowsException<Exception>(() => f.Unlock());
    }

    private ComponentsPoolsController _poolsController = null!;
    private EntitiesController _entitiesController = null!;
    private List<IComponentsPool> _pools = new();

    [TestInitialize]
    public void Initialize()
    {
        _entitiesController = new EntitiesController(new DisabledWorldDummy());
        _poolsController = new ComponentsPoolsController(_entitiesController.PoolUpdated);
        _pools = [_poolsController.GetPool<C0>(), _poolsController.GetPool<C1>(), _poolsController.GetPool<C2>(),
                    _poolsController.GetPool<C3>(), _poolsController.GetPool<C4>(), _poolsController.GetPool<C5>(),
                    _poolsController.GetPool<C6>(), _poolsController.GetPool<C7>(), _poolsController.GetPool<C8>(), _poolsController.GetPool<C9>()];
    }

    private struct C0; private struct C1; private struct C2; private struct C3; private struct C4;
    private struct C5; private struct C6; private struct C7; private struct C8; private struct C9;
}
