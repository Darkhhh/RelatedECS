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
}
