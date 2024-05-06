namespace RelatedECS.Entities.Relations;

internal class Relation
{
    public required Type RelationType { get; init; }
    public required IEntity EntityFrom { get; init; }
    public required IEntity EntityTo { get; init; }

    public override bool Equals(object? obj)
    {
        return obj is Relation relation &&
               EqualityComparer<Type>.Default.Equals(RelationType, relation.RelationType) &&
               EqualityComparer<IEntity>.Default.Equals(EntityFrom, relation.EntityFrom) &&
               EqualityComparer<IEntity>.Default.Equals(EntityTo, relation.EntityTo);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(RelationType, EntityFrom, EntityTo);
    }
}

internal class RelationDescription
{
    public required Relation Instance { get; init; }
    public required object Description { get; init; }
}
