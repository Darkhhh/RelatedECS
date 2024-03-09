using RelatedECS.Maintenance.Utilities;

namespace RelatedECS.Entities;

public class EntitiesController
{
    private readonly ObjectPool<Entity> _recycledEntities = new();

    private int _currentEntityIndex = 0;


}
