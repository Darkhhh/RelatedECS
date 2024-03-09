using RelatedECS.Maintenance.Utilities;

namespace RelatedECS.Entities;

public class Entity : IEntity, IAutoReset
{
    public int Id => throw new NotImplementedException();

    public IReadOnlyList<IEntity> Relations => throw new NotImplementedException();

    public ref T Add<T>() where T : struct
    {
        throw new NotImplementedException();
    }

    public void Delete<T>() where T : struct
    {
        throw new NotImplementedException();
    }

    public ref T Get<T>() where T : struct
    {
        throw new NotImplementedException();
    }

    public bool Has<T>() where T : struct
    {
        throw new NotImplementedException();
    }

    public void Init()
    {
        throw new NotImplementedException();
    }

    public void Reset()
    {
        throw new NotImplementedException();
    }
}
