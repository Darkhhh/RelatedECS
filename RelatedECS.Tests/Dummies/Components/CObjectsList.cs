using RelatedECS.Pools;

namespace RelatedECS.Tests.Dummies.Components;

public struct CObjectsList
{
    public List<DummyObject> Items = [];

    public CObjectsList() { }
}

public struct CObjectsResetList : IAutoResetComponent<CObjectsResetList>
{
    public List<DummyObject> Items = [];

    public CObjectsResetList() { }

    public static void AutoReset(ref CObjectsResetList instance)
    {
        instance.Items = [];
    }
}

public class DummyObject
{
    public int Id { get; set; }
    public double Volume { get; set; }
}
