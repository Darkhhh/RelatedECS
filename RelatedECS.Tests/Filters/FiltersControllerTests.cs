using RelatedECS.Entities;
using RelatedECS.Filters;
using RelatedECS.Pools;
using RelatedECS.Tests.Dummies;

namespace RelatedECS.Tests.Filters;

[TestClass]
public class FiltersControllerTests
{
    [TestMethod]
    public void CorrectRegistration()
    {
        var declaration = new FilterDeclaration(new World());
        declaration.WithTypes([typeof(C0), typeof(C4), typeof(C7)]);
        declaration.WithoutTypes([typeof(C1), typeof(C8), typeof(C9)]);

        _filtersController = new FiltersController(_entitiesController, _poolsController);

        var f = _filtersController.RegisterAsEntitiesFilter(declaration);
        Assert.IsNotNull(f);
    }

    private ComponentsPoolsController _poolsController = null!;
    private EntitiesController _entitiesController = null!;
    private FiltersController _filtersController = null!;

    [TestInitialize]
    public void Initialize()
    {
        _entitiesController = new EntitiesController(new DisabledWorldDummy());
        _poolsController = new ComponentsPoolsController(PoolUpdated);
    }

    private void PoolUpdated(Type poolType, int poolIndex, int entity, bool added)
    {
        _entitiesController.PoolUpdated(poolType, poolIndex, entity, added);
        _filtersController.PoolUpdated(poolType, poolIndex, entity, added);
    }

    private struct C0; private struct C1; private struct C2; private struct C3; private struct C4;
    private struct C5; private struct C6; private struct C7; private struct C8; private struct C9;
}
