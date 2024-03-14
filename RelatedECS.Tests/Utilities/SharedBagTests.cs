using RelatedECS.Maintenance.Utilities;

namespace RelatedECS.Tests.Utilities;

[TestClass]
public class SharedBagTests
{
    [TestMethod]
    public void CorrectAddingByType()
    {
        var bag = new SharedBag();
        bag.Add(new Dummy1());
        Assert.AreEqual(1, bag.Count);

        Assert.ThrowsException<Exception>(() =>
        {
            bag.Add(new Dummy1());
        });

        Assert.AreEqual(1, bag.Count);
    }

    [TestMethod]
    public void CorrectAddingByTag()
    {
        var bag = new SharedBag();
        bag.Add(new Dummy1(), "d1");
        Assert.AreEqual(1, bag.CountTagged);
        Assert.ThrowsException<Exception>(() =>
        {
            bag.Add(new Dummy1(), "d1");
        });
        Assert.AreEqual(1, bag.CountTagged);
        bag.Add(new Dummy1(), "d2");
        Assert.AreEqual(2, bag.CountTagged);
    }

    [TestMethod]
    public void CorrectGetByType()
    {
        var bag = new SharedBag();
        bag.Add(new Dummy1());
        _ = bag.Get<Dummy1>();

        Assert.ThrowsException<Exception>(() =>
        {
            _ = bag.Get<Dummy2>();
        });
    }

    [TestMethod]
    public void CorrectGetByTag()
    {
        var bag = new SharedBag();
        bag.Add(new Dummy1(), "d1");
        _ = bag.Get<Dummy1>("d1");

        Assert.ThrowsException<Exception>(() =>
        {
            _ = bag.Get<Dummy1>("d2");
        });
    }


    [TestMethod]
    public void CorrectHasByType()
    {
        var bag = new SharedBag();
        bag.Add(new Dummy1());
        Assert.IsTrue(bag.Has<Dummy1>());
        Assert.IsFalse(bag.Has<Dummy2>());

        bag.Add(new Dummy3());
        Assert.IsTrue(bag.Has<Dummy3>());
        Assert.IsFalse(bag.Has<Dummy2>());

        bag.Add(new List<Dummy3>());
        Assert.IsTrue(bag.Has<List<Dummy3>>());
        Assert.IsFalse(bag.Has<List<Dummy2>>());
    }

    [TestMethod]
    public void CorrectHasByTag()
    {
        var bag = new SharedBag();
        bag.Add(new Dummy1(), "d1");
        Assert.IsFalse(bag.Has<Dummy1>());
        Assert.IsFalse(bag.Has("d2"));
        Assert.IsTrue(bag.Has("d1"));
    }

    private class Dummy1;
    private class Dummy2
    {
        public int Value { get; set; }
    }
    private class Dummy3 : Dummy2
    {
        public double Volume { get; set; }
    }
}
