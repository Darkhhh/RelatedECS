using System.Diagnostics.CodeAnalysis;

namespace RelatedECS.Entities;

public readonly struct EntityPack
{
    private readonly Entity _reference;
    private readonly int _packGeneration;

    internal EntityPack(Entity reference, int generation)
    {
        _reference = reference;
        _packGeneration = generation;
    }

    public bool TryGet([MaybeNullWhen(false)] out IEntity entity)
    {
        entity = null;
        if (!_reference.IsAlive || _packGeneration != _reference.Generation) return false;
        entity = _reference;
        return true;
    }
}
