using RelatedECS.Pools;
using RelatedECS.Tests.Dummies.Components;

namespace RelatedECS.Tests.Pools;

[TestClass]
public class SimpleComponentsPoolsTests
{
    [TestMethod]
    public void CorrectAdding()
    {
        var list = new List<EntityState>();
        var pool = new ComponentsPool<CPosition>(0, (t, e, add) => list.Add(new EntityState { PoolType = t, Index = e, Added = add}));

        var entity = 7;
        ref var position = ref pool.Add(entity);
        position.X = 4; 
        position.Y = 5;

        Assert.AreEqual(1, list.Count);
        Assert.AreEqual(typeof(CPosition), list[0].PoolType);
        Assert.AreEqual(7, list[0].Index);
        Assert.AreEqual(true, list[0].Added);

        Assert.ThrowsException<Exception>(() =>
        {
            pool.Add(entity);
        });

        var e2 = 554;
        pool.Add(e2);

        var e3 = -15;
        Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
        {
            pool.Add(e3);
        });
    }

    [TestMethod]
    public void CorrectHasMethod()
    {
        var list = new List<EntityState>();
        var pool = new ComponentsPool<CPosition>(0, (t, e, add) => list.Add(new EntityState { PoolType = t, Index = e, Added = add }));

        for (var i = 0; i < IComponentsPool.InitialCapacity + 1; i++)
        {
            Assert.IsFalse(pool.Has(i));
        }

        var entity = 7;
        ref var position = ref pool.Add(entity);

        Assert.IsTrue(pool.Has(entity));
        for (var i = 0; i < IComponentsPool.InitialCapacity + 1; i++)
        {
            if (i == entity) continue;
            Assert.IsFalse(pool.Has(i));
        }

        var e3 = -15;
        Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
        {
            pool.Add(e3);
        });
    }

    [TestMethod]
    public void CorrectGetMethod()
    {
        var list = new List<EntityState>();
        var pool = new ComponentsPool<CPosition>(0, (t, e, add) => list.Add(new EntityState { PoolType = t, Index = e, Added = add }));

        for (var i = 0; i < IComponentsPool.InitialCapacity + 1; i++)
        {
            Assert.ThrowsException<Exception>(() => pool.Get(i));
        }

        var entity = 7;
        ref var position = ref pool.Add(entity);
        position.X = 4;
        position.Y = 5;

        ref var p = ref pool.Get(entity);
        Assert.AreEqual(4, p.X);
        Assert.AreEqual(5, p.Y);

        for (var i = 0; i < IComponentsPool.InitialCapacity + 1; i++)
        {
            if (i == entity) continue;
            Assert.ThrowsException<Exception>(() => pool.Get(i));
        }
    }

    [TestMethod]
    public void CorrectGetRawMethod()
    {
        var list = new List<EntityState>();
        var pool = new ComponentsPool<CPosition>(0, (t, e, add) => list.Add(new EntityState { PoolType = t, Index = e, Added = add }));

        for (var i = 0; i < IComponentsPool.InitialCapacity + 1; i++)
        {
            Assert.ThrowsException<Exception>(() => pool.GetRaw(i));
        }

        var entity = 7;
        ref var position = ref pool.Add(entity);
        position.X = 4;
        position.Y = 5;

        var p = (CPosition)pool.GetRaw(entity);
        Assert.AreEqual(4, p.X);
        Assert.AreEqual(5, p.Y);

        for (var i = 0; i < IComponentsPool.InitialCapacity + 1; i++)
        {
            if (i == entity) continue;
            Assert.ThrowsException<Exception>(() => pool.GetRaw(i));
        }
    }

    [TestMethod]
    public void CorrectDeleteMethod()
    {
        var list = new List<EntityState>();
        var pool = new ComponentsPool<CPosition>(0, (t, e, add) => list.Add(new EntityState { PoolType = t, Index = e, Added = add }));

        var entity = 7;
        pool.Delete(entity);
        Assert.AreEqual(0, list.Count);

        ref var position = ref pool.Add(entity);
        position.X = 4;
        position.Y = 5;

        pool.Delete(entity);
        Assert.AreEqual(2, list.Count);
        Assert.IsFalse(pool.Has(entity));
        Assert.ThrowsException<Exception>(() => pool.Get(entity));

        ref var p = ref pool.Add(entity);
        Assert.AreNotEqual(4, p.X);
        Assert.AreNotEqual(5, p.Y);

        Assert.AreEqual(0, p.X);
        Assert.AreEqual(0, p.Y);
    }

    private class EntityState
    {
        public Type? PoolType { get; set; }
        public int Index { get; set; }
        public bool Added { get; set; }
    }
}
