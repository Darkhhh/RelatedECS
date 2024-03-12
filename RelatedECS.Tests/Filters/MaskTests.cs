using RelatedECS.Maintenance.Utilities;
using System.Threading.Tasks;

namespace RelatedECS.Tests.Filters;

[TestClass]
public class MaskTests
{
    private const int BitSize = 64;

    [TestMethod]
    public void CorrectSet()
    {
        var mask = new Mask();

        mask.Set(42, true);
        Assert.AreEqual<ulong>(4_398_046_511_104, mask[0]);
        Assert.AreEqual<ulong>(0, mask[1]);
        mask.Set(42, false);
        Assert.AreEqual<ulong>(0, mask[0]);
        Assert.IsTrue(mask.IsEmpty);
    }

    [TestMethod]
    public void CorrectResizeMethod()
    {
        var mask = new Mask();

        mask.Resize(BitSize + 1);
        Assert.AreEqual(2, mask.Length);

        mask.Resize(2 * BitSize + 1);
        Assert.AreEqual(3, mask.Length);

        mask.Resize(BitSize + 1);
        Assert.AreEqual(3, mask.Length);
    }

    [TestMethod]
    public void CorrectSetWithIndexOutsideOfArray()
    {
        var mask = new Mask();

        mask.Set(145, true);
        Assert.AreEqual(3, mask.Length);
        Assert.AreEqual<ulong>(131_072, mask[2]);
        mask.Set(145, false);
        Assert.AreEqual<ulong>(0, mask[2]);
        Assert.IsTrue(mask.IsEmpty);
    }

    [TestMethod]
    public void CorrectGetMethod()
    {
        var mask = new Mask();
        mask.Set(42, true);
        Assert.IsTrue(mask.Get(42));

        mask.Set(42, false);
        Assert.IsFalse(mask.Get(42));

        Assert.IsFalse(mask.Get(2 * BitSize + 1));
    }

    [TestMethod]
    public void CorrectCompareMethod()
    {
        var mask1 = new Mask();

        Assert.IsFalse(mask1.Compare(null));

        var mask2 = new Mask();

        Assert.IsTrue(mask1.Compare(mask2));
        Assert.IsTrue(mask2.Compare(mask1));

        mask1.Set(BitSize + 1, true);
        Assert.IsFalse(mask2.Compare(mask1));
        mask2.Set(BitSize + 1, true);
        Assert.IsTrue(mask2.Compare(mask1));

        mask1.Resize(2 *  BitSize + 1);
        Assert.IsFalse(mask2.Compare(mask1));
        mask2.Resize(2 * BitSize + 1);
        Assert.IsTrue(mask2.Compare(mask1));
    }

    [TestMethod]
    public void CorrectClearMethod()
    {
        var mask = new Mask();

        mask.Set(42, true);
        Assert.IsFalse(mask.IsEmpty);
        mask.Set(64, true);
        mask.Set(129, true);

        mask.Clear();
        Assert.IsTrue(mask.IsEmpty);
    }

    [TestMethod]
    public void CorrectIndexator()
    {
        var mask1 = new Mask();
        var mask2 = new Mask();
        mask1.Set(BitSize + 1, true);
        mask2.Set(BitSize + 1, true);
        Assert.IsTrue(mask1[0] == mask2[0]);
        Assert.IsTrue(mask1[1] == mask2[1]);
    }
}
