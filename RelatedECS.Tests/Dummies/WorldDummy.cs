using RelatedECS.Filters;
using RelatedECS.Maintenance.Utilities;

namespace RelatedECS.Tests.Dummies;

internal class WorldDummy : IWorld
{
    public IMessageBus Bus { get; set; }

    public ISharedBag Bag { get; set; }

    public IEntitiesFilter RegisterAsEntitiesFilter(IFilterDeclaration declaration)
    {
        return null;
    }

    public IIndicesFilter RegisterAsIndicesFilter(IFilterDeclaration declaration)
    {
        return null;
    }
}
