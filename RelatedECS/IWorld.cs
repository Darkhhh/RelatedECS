using RelatedECS.Filters;

namespace RelatedECS;

public interface IWorld
{
    public IEntitiesFilter RegisterAsEntitiesFilter(IFilterDeclaration declaration);
    public IIndicesFilter RegisterAsIndicesFilter(IFilterDeclaration declaration);
}
