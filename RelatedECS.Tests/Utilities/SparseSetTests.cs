using RelatedECS.Maintenance.Utilities;

namespace RelatedECS.Tests.Utilities;

[TestClass]
public class SparseSetTests
{
    [TestMethod]
    public void CorrectCreating()
    {
        var set = new SparseSet(1000, 10);
        Assert.AreEqual(0, set.Length);
    }

    [TestMethod]
    public void CorrectInsert()
    {
        var set = new SparseSet(1000, 2);
        Assert.AreEqual(true, set.Insert(520));
        Assert.AreEqual(false, set.Insert(520));
        Assert.AreEqual(1, set.Length);
        Assert.AreEqual(false, set.Insert(1520));
        Assert.AreEqual(true, set.Insert(400));
        Assert.IsTrue(set.Full);
        Assert.AreEqual(false, set.Insert(321));
        Assert.AreEqual(2, set.Length);
    }

    [TestMethod]
    public void CorrectDelete()
    {
        var set = new SparseSet(200, 2);
        set.Insert(100);
        set.Insert(72);

        set.Delete(60);
        Assert.AreEqual(2, set.Length);
        set.Delete(260);
        Assert.AreEqual(2, set.Length);
        set.Delete(100);
        Assert.AreEqual(1, set.Length);
        set.Delete(72);
        Assert.AreEqual(0, set.Length);
    }

    [TestMethod]
    public void CorrectFind()
    {
        var set = new SparseSet(200, 4);
        set.Insert(100);
        set.Insert(72);

        Assert.AreEqual(-1, set.Find(400));
        Assert.AreEqual(-1, set.Find(35));

        Assert.AreEqual(0, set.Find(100));
        Assert.AreEqual(1, set.Find(72));

        set.Delete(100);
        Assert.AreEqual(0, set.Find(72));
        Assert.AreNotEqual(1, set.Find(72));

        set.Insert(26);
        set.Insert(54);
        Assert.AreEqual(1, set.Find(26));
        Assert.AreEqual(2, set.Find(54));

        set.Delete(72);
        Assert.AreEqual(1, set.Find(26));
        Assert.AreEqual(0, set.Find(54));
    }

    [TestMethod]
    public void CorrectDenseAllocation()
    {
        var set = new SparseSet(200, 4);
        set.Insert(24);
        set.Insert(72);
        set.Insert(7);
        set.Insert(86);
        Assert.AreEqual(false, set.Insert(46));

        set.AllocateDense(6);
        Assert.AreEqual(true, set.Insert(65));
        Assert.AreEqual(true, set.Insert(26));
        Assert.AreEqual(false, set.Insert(4));

        set.Delete(7);
        Assert.AreEqual(2, set.Find(26));
    }

    [TestMethod]
    public void CorrectSparseAllocation()
    {
        var set = new SparseSet(200, 4);
        Assert.AreEqual(true, set.Insert(64));
        Assert.AreEqual(false, set.Insert(256));

        set.AllocateSparse(512);
        Assert.AreEqual(0, set.Find(64));
        Assert.AreEqual(true, set.Insert(256));
        Assert.AreEqual(1, set.Find(256));
    }

    [TestMethod]
    public void CorrectIndexator()
    {
        var set = new SparseSet(200, 4);
        Assert.IsTrue(set.Insert(64));
        Assert.IsTrue(set.Insert(15));
        Assert.IsTrue(set.Insert(128));
        Assert.IsTrue(set.Insert(3));
        Assert.IsTrue(Compare(ref set, [64, 15, 128, 3]));
    }

    private bool Compare(ref SparseSet set, int[] array)
    {
        if (set.Length != array.Length) return false;
        for (int i = 0; i < set.Length; i++)
        {
            if (set[i] != array[i]) return false;
        }
        return true;
    }
}
