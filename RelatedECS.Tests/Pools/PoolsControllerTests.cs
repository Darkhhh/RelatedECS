using RelatedECS.Pools;
using RelatedECS.Tests.Dummies.Components;

namespace RelatedECS.Tests.Pools;

[TestClass]
public class PoolsControllerTests
{
    [TestMethod]
    public void CorrectGet()
    {
        IComponentsPoolsController controller = new ComponentsPoolsController((type, id, entity, added) =>
        {

        });

        var pool = controller.GetPool<CPosition>();
        Assert.IsNotNull(pool);

        var pool1 = controller.GetPool<CPosition>();
        Assert.IsTrue(ReferenceEquals(pool1, pool));

        var pool2 = controller.GetPool(typeof(CPosition));
        Assert.IsTrue(ReferenceEquals(pool2, pool1));
        Assert.IsTrue(ReferenceEquals(pool, pool2));
    }

    [TestMethod]
    public void CorrectGetAll()
    {
        IComponentsPoolsController controller = new ComponentsPoolsController((type, id, entity, added) =>
        {

        });
        var positionsPool = controller.GetPool<CPosition>();
        var rangesPool = controller.GetPool<CRange>();

        var list = controller.GetAll();
        Assert.AreEqual(2, list.Count);
        Assert.IsTrue(ReferenceEquals(list[0], positionsPool));
        Assert.IsTrue(ReferenceEquals(list[1], rangesPool));
    }

    [TestMethod]
    public void CorrectCount()
    {
        IComponentsPoolsController controller = new ComponentsPoolsController((type, id, entity, added) =>
        {

        });
        var positionsPool = controller.GetPool<CPosition>();
        var rangesPool = controller.GetPool<CRange>();

        Assert.AreEqual(2, controller.PoolsCount);
    }

    [TestMethod]
    public void CorrectExceptionThrow()
    {
        IComponentsPoolsController controller = new ComponentsPoolsController((type, id, entity, added) =>
        {

        });

        Assert.ThrowsException<Exception>(() => controller.GetPool(typeof(CPosition)));
    }

    [TestMethod]
    public void CorrectIndices()
    {
        IComponentsPoolsController controller = new ComponentsPoolsController((type, id, entity, added) =>
        {

        });

        var positionsPool = controller.GetPool<CPosition>();
        var rangesPool = controller.GetPool<CRange>();

        Assert.AreEqual(0, positionsPool.Id);
        Assert.AreEqual(1, rangesPool.Id);
    }

    private Type? _currentType;
    private int _currentIndex, _currentEntity, _eventInvokes;
    private bool _currentAdded;
    [TestMethod]
    public void CorrectEvents()
    {
        IComponentsPoolsController controller = new ComponentsPoolsController(PoolBeenChanged);

        var positionsPool = controller.GetPool<CPosition>();
        _eventInvokes = 0;
        _currentIndex = positionsPool.Id;
        _currentType = typeof(CPosition);
        _currentEntity = 14;
        _currentAdded = true;
        positionsPool.Add(_currentEntity);
        Assert.AreEqual(1, _eventInvokes);

        _currentAdded = false;
        positionsPool.Delete(_currentEntity);
        Assert.AreEqual(2, _eventInvokes);

        void PoolBeenChanged(Type poolType, int poolIndex, int entity, bool added)
        {
            _eventInvokes++;
            Assert.AreEqual(_currentType, poolType);
            Assert.AreEqual(_currentIndex, poolIndex);
            Assert.AreEqual(_currentEntity, entity);
            Assert.AreEqual(_currentAdded, added);
        }
    }

    [TestMethod]
    public void CorrectReflectionPoolCreation()
    {
        var controller = new ComponentsPoolsController(PoolBeenChanged);

        var pool = controller.CreatePoolOfType(typeof(CPosition));

        _eventInvokes = 0;
        _currentIndex = pool.Id;
        _currentType = typeof(CPosition);
        _currentEntity = 14;
        _currentAdded = true;

        ((ComponentsPool<CPosition>)pool).Add(_currentEntity);
        Assert.AreEqual(1, _eventInvokes);
        _currentAdded = false;
        ((ComponentsPool<CPosition>)pool).Delete(_currentEntity);
        Assert.AreEqual(2, _eventInvokes);

        var pool1 = controller.GetPool<CPosition>();
        var pool2 = controller.GetPool(typeof(CPosition));

        Assert.IsNotNull(pool);
        Assert.IsNotNull(pool1);
        Assert.IsNotNull(pool2);
        Assert.IsTrue(ReferenceEquals(pool, pool1));
        Assert.IsTrue(ReferenceEquals(pool, pool2));
        Assert.IsTrue(ReferenceEquals(pool1, pool2));

        void PoolBeenChanged(Type poolType, int poolIndex, int entity, bool added)
        {
            _eventInvokes++;
            Assert.AreEqual(_currentType, poolType);
            Assert.AreEqual(_currentIndex, poolIndex);
            Assert.AreEqual(_currentEntity, entity);
            Assert.AreEqual(_currentAdded, added);
        }
    }

    [TestMethod]
    public void CorrectMasks()
    {
        IComponentsPoolsController controller = new ComponentsPoolsController((type, id, entity, added) => { });

        int i0 = controller.GetPool<C0>().Id, i1 = controller.GetPool<C1>().Id, i2 = controller.GetPool<C2>().Id; 
        int i3 = controller.GetPool<C3>().Id, i4 = controller.GetPool<C4>().Id, i5 = controller.GetPool<C5>().Id; 
        int i6 = controller.GetPool<C7>().Id, i7 = controller.GetPool<C7>().Id, i8 = controller.GetPool<C8>().Id, i9 = controller.GetPool<C9>().Id;

        var masks = controller.GetMasks(
            [typeof(C0), typeof(C8), typeof(C4)], 
            [typeof(C3), typeof(C1), typeof(C5), typeof(C9)]);
        Assert.IsTrue(masks.With.Get(i0));
        Assert.IsTrue(masks.With.Get(i4));
        Assert.IsTrue(masks.With.Get(i8));

        Assert.IsTrue(masks.Without.Get(i3));
        Assert.IsTrue(masks.Without.Get(i1));
        Assert.IsTrue(masks.Without.Get(i5));
        Assert.IsTrue(masks.Without.Get(i9));
    }

    private struct C0;
    private struct C1;
    private struct C2;
    private struct C3;
    private struct C4;
    private struct C5;
    private struct C6;
    private struct C7;
    private struct C8;
    private struct C9;
}
