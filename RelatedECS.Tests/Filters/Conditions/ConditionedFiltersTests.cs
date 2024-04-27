using RelatedECS.Entities;
using RelatedECS.Filters;
using RelatedECS.Filters.Conditions;
using RelatedECS.Maintenance.Utilities;
using RelatedECS.Pools;
using RelatedECS.Tests.Dummies;

namespace RelatedECS.Tests.Filters.Conditions;

[TestClass]
public class ConditionedFiltersTests
{
    [TestMethod]
    public void BaseCorrectCount()
    {
        var declaration = new FilterDeclaration(new WorldDummy());
        var f = new RegisteredFilter(_entitiesController, _masks.With, _masks.Without);
        const int entitiesNumber = 5;
        for (int i = 0; i < entitiesNumber; i++) f.CheckEntity(NewSuitableEntity());

        var filter = new EntitiesConditionedFilter(f, declaration, _poolsController);

        Assert.AreEqual(f.Count, filter.Count);
        Assert.AreEqual(entitiesNumber, filter.Count);
    }

    [TestMethod]
    public void BaseCorrectDeclarationAssignment()
    {
        var declaration = new FilterDeclaration(new WorldDummy());
        var f = new RegisteredFilter(_entitiesController, _masks.With, _masks.Without);

        var filter = new EntitiesConditionedFilter(f, declaration, _poolsController);

        Assert.IsTrue(ReferenceEquals(filter.Declaration, declaration));
    }

    [TestMethod]
    public void BaseCorrectRawFilterAssignment()
    {
        var declaration = new FilterDeclaration(new WorldDummy());
        var f = new RegisteredFilter(_entitiesController, _masks.With, _masks.Without);

        var filter = new EntitiesConditionedFilter(f, declaration, _poolsController);

        Assert.IsTrue(ReferenceEquals(filter.Raw, f));
    }

    [TestMethod]
    public void BaseCorrectOneCycle()
    {
        var declaration = new FilterDeclaration(new WorldDummy());
        var f = new RegisteredFilter(_entitiesController, _masks.With, _masks.Without);
        const int entitiesNumber = 5;
        for (int i = 0; i < entitiesNumber; i++) f.CheckEntity(NewSuitableEntity());

        var filter = new EntitiesConditionedFilter(f, declaration, _poolsController);
        var count = 0;

        foreach (var e in filter.Entities())
        {
            Assert.IsTrue(f.IsLocked);
            count++;
        }
        Assert.AreEqual(entitiesNumber, count);
    }

    [TestMethod]
    public void BaseCorrectTwoCycles()
    {
        var declaration = new FilterDeclaration(new WorldDummy());
        var f = new RegisteredFilter(_entitiesController, _masks.With, _masks.Without);
        const int entitiesNumber = 5;
        for (int i = 0; i < entitiesNumber; i++) f.CheckEntity(NewSuitableEntity());

        var filter = new EntitiesConditionedFilter(f, declaration, _poolsController);
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


    [TestMethod]
    public void CorrectSuitableEntitiesEnumeration()
    {
        var declaration = new FilterDeclaration(new WorldDummy());
        declaration.WithTypes([typeof(C0), typeof(C4), typeof(C7)]);
        declaration.WithoutTypes([typeof(C1), typeof(C8), typeof(C9)]);
        var f = new RegisteredFilter(_entitiesController, _masks.With, _masks.Without);
        const int entitiesNumber = 5;
        for (int i = 0; i < entitiesNumber; i++) f.CheckEntity(NewSuitableEntity());

        var filter = new EntitiesConditionedFilter(f, declaration, _poolsController);
        var count = 0;

        var c0Check = new C0Checker();
        var c4Check = new C4Checker();
        var c7Check = new C7Checker();

        foreach (var e in filter.Entities(c0Check, c4Check, c7Check))
        {
            Assert.IsTrue(f.IsLocked);
            count++;
        }
        Assert.AreEqual(entitiesNumber, count);
    }

    [TestMethod]
    public void CorrectNotSuitableEntitiesEnumerationWithoutChecks()
    {
        var declaration = new FilterDeclaration(new WorldDummy());
        declaration.WithTypes([typeof(C0), typeof(C4), typeof(C7)]);
        declaration.WithoutTypes([typeof(C1), typeof(C8), typeof(C9)]);
        var f = new RegisteredFilter(_entitiesController, _masks.With, _masks.Without);
        const int entitiesNumber = 5;
        for (int i = 0; i < entitiesNumber; i++) f.CheckEntity(NewSuitableEntity());
        for (int i = 0; i < entitiesNumber; i++) f.CheckEntity(NewNotC0ValueSuitableEntity());

        var filter = new EntitiesConditionedFilter(f, declaration, _poolsController);
        var count = 0;
        foreach (var e in filter.Entities())
        {
            Assert.IsTrue(f.IsLocked);
            count++;
        }
        Assert.AreEqual(entitiesNumber * 2, count);
    }

    [TestMethod]
    public void CorrectNotSuitableEntitiesEnumeration1()
    {
        var declaration = new FilterDeclaration(new WorldDummy());
        declaration.WithTypes([typeof(C0), typeof(C4), typeof(C7)]);
        declaration.WithoutTypes([typeof(C1), typeof(C8), typeof(C9)]);
        var f = new RegisteredFilter(_entitiesController, _masks.With, _masks.Without);
        const int entitiesNumber = 5;
        for (int i = 0; i < entitiesNumber; i++) f.CheckEntity(NewSuitableEntity());
        for (int i = 0; i < entitiesNumber; i++) f.CheckEntity(NewNotC0ValueSuitableEntity());

        var filter = new EntitiesConditionedFilter(f, declaration, _poolsController);
        var count = 0;

        var c0Check = new C0Checker();
        var c4Check = new C4Checker();
        var c7Check = new C7Checker();

        foreach (var e in filter.Entities(c0Check, c4Check, c7Check))
        {
            Assert.IsTrue(f.IsLocked);
            count++;
        }
        Assert.AreEqual(entitiesNumber, count);
    }

    [TestMethod]
    public void CorrectNotSuitableEntitiesEnumeration2()
    {
        var declaration = new FilterDeclaration(new WorldDummy());
        declaration.WithTypes([typeof(C0), typeof(C4), typeof(C7)]);
        declaration.WithoutTypes([typeof(C1), typeof(C8), typeof(C9)]);
        var f = new RegisteredFilter(_entitiesController, _masks.With, _masks.Without);
        const int entitiesNumber = 5;
        for (int i = 0; i < entitiesNumber; i++) f.CheckEntity(NewSuitableEntity());
        for (int i = 0; i < entitiesNumber; i++) f.CheckEntity(NewNotC7ValueSuitableEntity());

        var filter = new EntitiesConditionedFilter(f, declaration, _poolsController);
        var count = 0;

        var c0Check = new C0Checker();
        var c4Check = new C4Checker();

        foreach (var e in filter.Entities(c0Check, c4Check))
        {
            Assert.IsTrue(f.IsLocked);
            count++;
        }
        Assert.AreEqual(entitiesNumber * 2, count);
    }

    private Entity NewSuitableEntity()
    {
        var e = _entitiesController.New();
        ref var p1 = ref ((ComponentsPool<C0>)_pools[0]).Add(e.Id);
        p1.Value = 2;
        ref var p2 = ref ((ComponentsPool<C4>)_pools[4]).Add(e.Id);
        p2.Value = 77;
        ref var p3 = ref ((ComponentsPool<C7>)_pools[7]).Add(e.Id);
        p3.Value = 24;
        return (Entity)e;
    }

    private Entity NewNotC0ValueSuitableEntity()
    {
        var e = _entitiesController.New();
        ref var p1 = ref ((ComponentsPool<C0>)_pools[0]).Add(e.Id);
        p1.Value = 13;
        ref var p2 = ref ((ComponentsPool<C4>)_pools[4]).Add(e.Id);
        p2.Value = 13;
        ref var p3 = ref ((ComponentsPool<C7>)_pools[7]).Add(e.Id);
        p3.Value = 24;
        return (Entity)e;
    }
    private Entity NewNotC4ValueSuitableEntity()
    {
        var e = _entitiesController.New();
        ref var p1 = ref ((ComponentsPool<C0>)_pools[0]).Add(e.Id);
        p1.Value = 3;
        ref var p2 = ref ((ComponentsPool<C4>)_pools[4]).Add(e.Id);
        p2.Value = 2;
        ref var p3 = ref ((ComponentsPool<C7>)_pools[7]).Add(e.Id);
        p3.Value = 24;
        return (Entity)e;
    }
    private Entity NewNotC7ValueSuitableEntity()
    {
        var e = _entitiesController.New();
        ref var p1 = ref ((ComponentsPool<C0>)_pools[0]).Add(e.Id);
        p1.Value = 3;
        ref var p2 = ref ((ComponentsPool<C4>)_pools[4]).Add(e.Id);
        p2.Value = 64;
        ref var p3 = ref ((ComponentsPool<C7>)_pools[7]).Add(e.Id);
        p3.Value = 14;
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

    private struct C0{public int Value;}
    private struct C1{public int Value;}
    private struct C2{public int Value;}
    private struct C3{public int Value;}
    private struct C4{public int Value;}
    private struct C5{public int Value;}
    private struct C6{public int Value;}
    private struct C7{public int Value;}
    private struct C8{public int Value;}
    private struct C9{public int Value;}

    private class C0Checker : ConditionCheck<C0>
    {
        public override bool CheckCondition(C0 obj)
        {
            return obj.Value < 5;
        }
    }
    private class C4Checker : ConditionCheck<C4>
    {
        public override bool CheckCondition(C4 obj)
        {
            return obj.Value >= 7;
        }
    }
    private class C7Checker : ConditionCheck<C7>
    {
        public override bool CheckCondition(C7 obj)
        {
            return obj.Value >= 17;
        }
    }
}
