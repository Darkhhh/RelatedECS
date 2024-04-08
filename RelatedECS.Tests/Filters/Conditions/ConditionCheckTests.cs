using RelatedECS.Filters.Conditions;
using RelatedECS.Pools;
using RelatedECS.Tests.Dummies.Components;

namespace RelatedECS.Tests.Filters.Conditions;

[TestClass]
public class ConditionCheckTests
{
    [TestMethod]
    public void CorrectEmptyChecks()
    {
        var provider = new PoolsProvider();
        var controller = new ConditionsCheckController(provider);

        Assert.IsTrue(controller.Check(14));
        Assert.IsTrue(controller.Check(2));
    }

    [TestMethod]
    public void CorrectAddChecks()
    {
        var provider = new PoolsProvider();
        var controller = new ConditionsCheckController(provider);

        Assert.IsTrue(controller.AddCheck(new PositionCheck()));
        Assert.AreEqual(1, controller.Count);

        Assert.IsFalse(controller.AddCheck(new PositionCheck()));
        Assert.AreEqual(1, controller.Count);

        Assert.IsTrue(controller.AddCheck(new VolumeCheck()));
        Assert.AreEqual(2, controller.Count);
    }

    [TestMethod]
    public void CorrectRemoveChecks()
    {
        var provider = new PoolsProvider();
        var controller = new ConditionsCheckController(provider);

        var c1 = new PositionCheck();
        var c2 = new VolumeCheck();

        Assert.IsTrue(controller.AddCheck(c1));
        Assert.IsTrue(controller.AddCheck(c2));

        controller.RemoveCheck(c1);
        Assert.AreEqual(1, controller.Count);
        controller.RemoveCheck(c2);
        Assert.AreEqual(0, controller.Count);
    }

    [TestMethod]
    public void CorrectClearChecks()
    {
        var provider = new PoolsProvider();
        var controller = new ConditionsCheckController(provider);

        var c1 = new PositionCheck();
        var c2 = new VolumeCheck();

        Assert.IsTrue(controller.AddCheck(c1));
        Assert.IsTrue(controller.AddCheck(c2));

        controller.Clear();
        Assert.AreEqual(0, controller.Count);
    }

    [TestMethod]
    public void CorrectOneTypeCheck()
    {
        var provider = new PoolsProvider();
        var controller = new ConditionsCheckController(provider);
        var pool = provider.GetPool<CPosition>();

        controller.AddCheck(new PositionCheck());

        var e1 = 4;
        ref var ep1 = ref pool.Add(e1);
        ep1.X = 7; ep1.Y = 2;

        Assert.IsTrue(controller.Check(e1));

        var e2 = 17;
        ref var ep2 = ref pool.Add(e2);
        ep2.X = 4; ep2.Y = 2;

        Assert.IsFalse(controller.Check(e2));

        ref var ep22 = ref pool.Get(e2);
        ep22.X = 16; ep22.Y = -5;

        Assert.IsTrue(controller.Check(e2));
    }

    [TestMethod]
    public void CorrectChainCheck()
    {
        var provider = new PoolsProvider();
        var controller = new ConditionsCheckController(provider);
        var pool = provider.GetPool<CPosition>();
        var pool2 = provider.GetPool<Volume>();

        controller.AddCheck(new PositionCheck());
        controller.AddCheck(new VolumeCheck());

        var e1 = 4;
        ref var ep1 = ref pool.Add(e1);
        ep1.X = 7; ep1.Y = 2;
        ref var ev1 = ref pool2.Add(e1);
        ev1.Value = 84;
        Assert.IsTrue(controller.Check(e1));

        var e2 = 17;
        ref var ep2 = ref pool.Add(e2);
        ep2.X = 4; ep2.Y = 2;
        ref var ev2 = ref pool2.Add(e2);
        ev2.Value = 84;
        Assert.IsFalse(controller.Check(e2));

        var e3 = 19;
        ref var ep3 = ref pool.Add(e3);
        ep3.X = 22; ep3.Y = 0;
        ref var ev3 = ref pool2.Add(e3);
        ev3.Value = 17;
        Assert.IsFalse(controller.Check(e3));
    }

    private class PoolsProvider : IPoolsProvider
    {
        private readonly Dictionary<Type, IComponentsPool> _pools = new();

        public IReadOnlyList<IComponentsPool> GetAll() => _pools.Values.ToList();

        public ComponentsPool<T> GetPool<T>() where T : struct
        {
            var type = typeof(T);
            if (_pools.TryGetValue(type, out var value)) return (ComponentsPool<T>)value;

            var pool = new ComponentsPool<T>(_pools.Count, PoolBeenChanged);
            _pools.Add(type, pool);
            return pool;
        }

        public IComponentsPool GetPool(Type type)
        {
            if (!_pools.TryGetValue(type, out var value)) throw new Exception("Pool is not registered");
            return value;
        }

        private void PoolBeenChanged(Type poolType, int entity, bool added) { }
    }

    private class PositionCheck : ConditionCheck<CPosition>
    {
        public override bool CheckCondition(CPosition obj)
        {
            return obj.X > 6 && obj.Y < 3;
        }
    }
    private class VolumeCheck : ConditionCheck<Volume>
    {
        public override bool CheckCondition(Volume obj)
        {
            return obj.Value > 70;
        }
    }
}
