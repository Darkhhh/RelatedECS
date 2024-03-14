using RelatedECS.Filters;

namespace RelatedECS.Tests.Dummies;

internal class WorldDummy : IWorld
{
    public IEntitiesFilter RegisterAsEntitiesFilter(IFilterDeclaration declaration)
    {
        return null;
    }

    public IIndicesFilter RegisterAsIndicesFilter(IFilterDeclaration declaration)
    {
        return null;
    }
}
