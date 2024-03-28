using RelatedECS.Entities;
using RelatedECS.Maintenance.Utilities;

namespace RelatedECS.Filters;

internal class RegisteredFilter : IRegisteredFilter, IEntitiesProvider
{
    private const int SetCapacity = 256, SetMaxValue = 256;

    private readonly IEntitiesStorage _entitiesStorage;
    private readonly Mask _withMask, _withoutMask;
    private readonly SparseSet _entities;
    private readonly List<DelayedFilterOperation> _operations = new();

    private int _locks = 0;

    public RegisteredFilter(IEntitiesStorage entitiesStorage, Mask withMask, Mask withoutMask)
    {
        _entitiesStorage = entitiesStorage;
        _withMask = withMask;
        _withoutMask = withoutMask;
        _entities = new SparseSet(SetMaxValue, SetCapacity);
    }

    public int Count => _entities.Length;

    public bool IsLocked => _locks > 0;

    public void Lock() => _locks++;

    public void Unlock()
    {
        _locks--;
#if DEBUG
        if (_locks < 0) throw new Exception("Incorrect Lock/Unlock operations");
#endif
        if (_locks > 0) return;

        foreach (var operation in _operations)
        {
            switch (operation.Operation)
            {
                case FilterOperationType.Add:
                    Add(operation.Reference);
                    break;
                case FilterOperationType.Remove:
                    Remove(operation.Reference);
                    break;
            }
        }
        _operations.Clear();
    }

    public int CheckEntity(Entity entity)
    {
        var contains = _entities.Find(entity.Id) != -1;
        var mask = entity.GetMask();
#if DEBUG
        if (mask.Length != _withMask.Length || mask.Length != _withoutMask.Length)
            throw new Exception($"Masks are not updated: Entity={mask.Length} With={_withMask.Length} Without={_withoutMask.Length}");
#endif

        var masksApproved = true;

        for (int i = 0; i < mask.Length; i++)
        {
            var e = mask[i];
            var w = _withMask[i];
            var wo = _withoutMask[i];
            var entityInc = e & w;
            var entityDec = ~e & wo;

            if (!(entityInc == w && entityDec == wo)) masksApproved = false;
        }

        if (masksApproved && contains) return 0;

        if (masksApproved && !contains)
        {
            Add(entity);
        }
        else if (!masksApproved && contains)
        {
            Remove(entity);
        }

        throw new Exception($"Unhandled condition: Contains={contains}, Masks={masksApproved}");
    }

    public Entity Get(int index)
    {
        return (Entity)_entitiesStorage.GetById(_entities[index]);
    }

    public int GetInitialIndex() => -1;

    public bool Next(int previousIndex, out int currentIndex)
    {
        currentIndex = -1;
        if (previousIndex + 1 > _entities.Length) return false;
        currentIndex = previousIndex + 1;
        return true;
    }

    public void ResizeMasks(int maxPoolIndex)
    {
        _withMask.Resize(maxPoolIndex);
        _withoutMask.Resize(maxPoolIndex);
    }


    private void Add(Entity entity)
    {
        if (_locks > 0)
        {
            _operations.Add(new DelayedFilterOperation(entity, entity.Id, FilterOperationType.Add));
            return;
        }

        _entities.AllocateDense(_entities.Length + 1);
        _entities.AllocateSparse(entity.Id);

        _entities.Insert(entity.Id);
    }

    private void Remove(Entity entity)
    {
        if (_locks > 0)
        {
            _operations.Add(new DelayedFilterOperation(entity, entity.Id, FilterOperationType.Remove));
            return;
        }

        _entities.Delete(entity.Id);
    }
}
