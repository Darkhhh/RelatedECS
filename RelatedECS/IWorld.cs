using RelatedECS.Filters;
using RelatedECS.Maintenance.Utilities;

namespace RelatedECS;

public interface IWorld
{
    public IMessageBus Bus { get; }

    public ISharedBag Bag { get; }

    public IEntitiesFilter RegisterAsEntitiesFilter(IFilterDeclaration declaration);
    public IIndicesFilter RegisterAsIndicesFilter(IFilterDeclaration declaration);
}
