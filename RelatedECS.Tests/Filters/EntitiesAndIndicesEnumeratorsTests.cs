using RelatedECS.Entities;
using RelatedECS.Filters;
using RelatedECS.Maintenance.Filters;
using RelatedECS.Maintenance.Utilities;
using RelatedECS.Pools;
using RelatedECS.Tests.Dummies;

namespace RelatedECS.Tests.Filters;

[TestClass]
public class EntitiesAndIndicesEnumeratorsTests
{
    [TestMethod]
    public void CorrectEntitiesEnumerator()
    {
        const int entitiesNumber = 5;
        var f = new RegisteredFilter(_entitiesController, _masks.With, _masks.Without);
        var set = new HashSet<IEntity>();
        for (var i = 0; i < entitiesNumber; i++)
        {
            var e = NewSuitableEntity();
            set.Add(e);
            Assert.AreEqual(1, f.CheckEntity(e));
        }
        Assert.AreEqual(entitiesNumber, f.Count);

        var enumerator = new EntitiesEnumerator(f, (e) => { });
        var count = 0;
        var deleted = 0;
        f.Lock();
        foreach (var entity in enumerator)
        {
            Assert.IsTrue(set.Contains(entity));
            count++;
            if (Random.Shared.NextDouble() < 0.5)
            {
                deleted++;
                ((ComponentsPool<C0>)_pools[0]).Delete(entity.Id);
                Assert.AreEqual(-1, f.CheckEntity((Entity)entity));
            }
        }
        Assert.AreEqual(entitiesNumber, count);
        f.Unlock();
        count = 0;
        f.Lock();
        foreach (var entity in enumerator)
        {
            count++;
        }
        Assert.AreEqual(entitiesNumber - deleted, count);
        f.Unlock();
    }

    [TestMethod]
    public void CorrectIndicesEnumerator()
    {
        const int entitiesNumber = 5;
        var f = new RegisteredFilter(_entitiesController, _masks.With, _masks.Without);
        var set = new HashSet<IEntity>();
        for (var i = 0; i < entitiesNumber; i++)
        {
            var e = NewSuitableEntity();
            set.Add(e);
            Assert.AreEqual(1, f.CheckEntity(e));
        }
        Assert.AreEqual(entitiesNumber, f.Count);

        var enumerator = new IndicesEnumerator(f, (e) => { });
        var count = 0;
        var deleted = 0;
        f.Lock();
        foreach (var entity in enumerator)
        {
            Assert.IsTrue(set.Contains(_entitiesController.GetById(entity)));
            count++;
            if (Random.Shared.NextDouble() < 0.5)
            {
                deleted++;
                ((ComponentsPool<C0>)_pools[0]).Delete(entity);
                Assert.AreEqual(-1, f.CheckEntity((Entity)_entitiesController.GetById(entity)));
            }
        }
        Assert.AreEqual(entitiesNumber, count);
        f.Unlock();
        count = 0;
        f.Lock();
        foreach (var entity in enumerator)
        {
            count++;
        }
        Assert.AreEqual(entitiesNumber - deleted, count);
        f.Unlock();
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
