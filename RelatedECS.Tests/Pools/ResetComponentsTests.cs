using RelatedECS.Pools;
using RelatedECS.Tests.Dummies.Components;

namespace RelatedECS.Tests.Pools;

[TestClass]
public class ResetComponentsTests
{
    [TestMethod]
    public void CorrectDefaultComponentReseting()
    {
        var pool = new ComponentsPool<Volume>(0, (_,_,_) => { });

        var e1 = 14;
        pool.Add(e1);
        ref var e1Volume = ref pool.Get(e1);

        Assert.AreEqual(0, e1Volume.Value);

        e1Volume.Value = 6;

        pool.Delete(e1);
        ref var e1Volume2 = ref pool.Add(e1);

        Assert.AreEqual(0, e1Volume2.Value);
    }

    [TestMethod]
    public void CorrectAutoResetComponentReseting()
    {
        var pool = new ComponentsPool<AutoResetVolume>(0, (_, _, _) => { });

        var e1 = 14;
        pool.Add(e1);
        ref var e1Volume = ref pool.Get(e1);

        Assert.AreEqual(AutoResetVolume.InitialValue, e1Volume.Value);

        e1Volume.Value = 6;

        pool.Delete(e1);
        ref var e1Volume2 = ref pool.Add(e1);

        Assert.AreEqual(AutoResetVolume.InitialValue, e1Volume2.Value);
    }

    [TestMethod]
    public void CorrectAutoResetForCollectionComponent()
    {
        var defaultPool = new ComponentsPool<CObjectsList>(0, (_, _, _) => { });
        var resetPool = new ComponentsPool<CObjectsResetList>(1, (_, _, _) => { });


        var entity = 14;

        ref var c1 = ref defaultPool.Add(entity);
        Assert.IsNull(c1.Items);

        ref var c2 = ref resetPool.Add(entity);
        Assert.IsNotNull(c2.Items);
    }
}
