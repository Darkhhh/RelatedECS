using RelatedECS.Entities;
using RelatedECS.Filters.Conditions;
using RelatedECS.Pools;

namespace RelatedECS.Filters;

internal interface IFiltersController
{
    public EntitiesFilter RegisterAsEntitiesFilter(IFilterDeclaration declaration);

    public EntitiesConditionedFilter RegisterAsConditionedFilter(IFilterDeclaration declaration);

    public void ResizeMasks(int maxPoolIndex);

    public void PoolUpdated(Type poolType, int poolIndex, int entity, bool added);
}

internal class FiltersController : IFiltersController
{
    private readonly List<IRegisteredFilter> _allFilters = new();
    private readonly Dictionary<Type, HashSet<IRegisteredFilter>> _filtersByPoolType = new();
    private readonly EntitiesController _entitiesController;
    private readonly IComponentsPoolsController _componentsPoolsController;

    public FiltersController(EntitiesController entitiesController, IComponentsPoolsController componentsPoolsController)
    {
        _entitiesController = entitiesController;
        _componentsPoolsController = componentsPoolsController;
    }

    public void PoolUpdated(Type poolType, int poolIndex, int entity, bool added)
    {
        if (!_filtersByPoolType.TryGetValue(poolType, out var filters)) return;

        foreach (var filter in filters)
        {
            filter.CheckEntity(_entitiesController.GetWrapById(entity));
        }
    }

    public EntitiesFilter RegisterAsEntitiesFilter(IFilterDeclaration declaration)
    {
        var masks = _componentsPoolsController.GetMasks(declaration.GetWithTypes(), declaration.GetWithoutTypes());
        var f = new RegisteredFilter(_entitiesController, masks.With, masks.Without);

        SaveRegisteredFilter(declaration, f);

        return new EntitiesFilter(f, declaration);
    }

    public EntitiesConditionedFilter RegisterAsConditionedFilter(IFilterDeclaration declaration)
    {
        var masks = _componentsPoolsController.GetMasks(declaration.GetWithTypes(), declaration.GetWithoutTypes());
        var f = new RegisteredFilter(_entitiesController, masks.With, masks.Without);

        SaveRegisteredFilter(declaration, f);

        return new EntitiesConditionedFilter(f, declaration, _componentsPoolsController);
    }

    public void ResizeMasks(int maxPoolIndex)
    {
        _allFilters.ForEach(i => i.ResizeMasks(maxPoolIndex));
    }

    private void SaveRegisteredFilter(IFilterDeclaration declaration, RegisteredFilter filter)
    {
        _allFilters.Add(filter);

        foreach (var type in declaration.GetWithTypes())
        {
            if (_filtersByPoolType.TryGetValue(type, out var filters)) filters.Add(filter);
            else _filtersByPoolType.Add(type, new HashSet<IRegisteredFilter> { filter });
        }
        foreach (var type in declaration.GetWithoutTypes())
        {
            if (_filtersByPoolType.TryGetValue(type, out var filters)) filters.Add(filter);
            else _filtersByPoolType.Add(type, new HashSet<IRegisteredFilter> { filter });
        }
    }
}