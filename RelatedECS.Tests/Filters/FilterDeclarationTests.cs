using RelatedECS.Filters;
using RelatedECS.Tests.Dummies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelatedECS.Tests.Filters;

[TestClass]
public class FilterDeclarationTests
{
    [TestMethod]
    public void CorrectGetWithTypes()
    {
        var filter = new FilterDeclaration(new WorldDummy());
        filter.With<Component1>().With<Component2>();
        var withTypes = filter.GetWithTypes();
        Assert.AreEqual(2, withTypes.Length);
        Assert.IsTrue(withTypes.Contains(typeof(Component1)));
        Assert.IsTrue(withTypes.Contains(typeof(Component2)));

        filter.With<Component3>().With<Component4>();
        var withTypes2 = filter.GetWithTypes();
        Assert.AreEqual(4, withTypes2.Length);
        Assert.IsTrue(withTypes2.Contains(typeof(Component1)));
        Assert.IsTrue(withTypes2.Contains(typeof(Component2)));
        Assert.IsTrue(withTypes2.Contains(typeof(Component3)));
        Assert.IsTrue(withTypes2.Contains(typeof(Component4)));
    }

    [TestMethod]
    public void CorrectGetWithoutTypes()
    {
        var filter = new FilterDeclaration(new WorldDummy());
        filter.Without<Component1>().Without<Component2>();
        var withoutTypes = filter.GetWithoutTypes();
        Assert.AreEqual(2, withoutTypes.Length);
        Assert.IsTrue(withoutTypes.Contains(typeof(Component1)));
        Assert.IsTrue(withoutTypes.Contains(typeof(Component2)));

        filter.Without<Component3>().Without<Component4>();
        var withoutTypes2 = filter.GetWithoutTypes();
        Assert.AreEqual(4, withoutTypes2.Length);
        Assert.IsTrue(withoutTypes2.Contains(typeof(Component1)));
        Assert.IsTrue(withoutTypes2.Contains(typeof(Component2)));
        Assert.IsTrue(withoutTypes2.Contains(typeof(Component3)));
        Assert.IsTrue(withoutTypes2.Contains(typeof(Component4)));
    }

    [TestMethod]
    public void CorrectDuplicateTypesAdding()
    {
        var filter = new FilterDeclaration(new WorldDummy());
        filter.With<Component1>().With<Component1>();
        var types = filter.GetWithTypes();
        Assert.AreEqual(1, types.Length);
        Assert.IsTrue(types.Contains(typeof(Component1)));

        filter.Without<Component2>().Without<Component2>();
        types = filter.GetWithoutTypes();
        Assert.AreEqual(1, types.Length);
        Assert.IsFalse(types.Contains(typeof(Component1)));
        Assert.IsTrue(types.Contains(typeof(Component2)));
    }

    [TestMethod]
    public void CorrectTypesIntersectionHandling()
    {
        var f1 = new FilterDeclaration(new WorldDummy());
        f1.With<Component1>();
        Assert.ThrowsException<Exception>(() =>
        {
            f1.Without<Component1>();
        });

        var f2 = new FilterDeclaration(new WorldDummy());
        f2.Without<Component2>().Without<Component5>();
        Assert.ThrowsException<Exception>(() =>
        {
            f2.With<Component5>();
        });
    }

    [TestMethod]
    public void CorrectMultipleWithTypesAdding()
    {
        var f1 = new FilterDeclaration(new WorldDummy());
        f1.WithTypes(typeof(Component2), typeof(Component3), typeof(Component4));
        var types = f1.GetWithTypes();
        Assert.AreEqual(3, types.Length);
        Assert.IsTrue(types.Contains(typeof(Component2)));
        Assert.IsTrue(types.Contains(typeof(Component3)));
        Assert.IsTrue(types.Contains(typeof(Component4)));

        f1.WithTypes(typeof(Component1), typeof(Component5), typeof(Component5));
        types = f1.GetWithTypes();
        Assert.AreEqual(2, types.Length);
        Assert.IsTrue(types.Contains(typeof(Component1)));
        Assert.IsTrue(types.Contains(typeof(Component5)));
    }

    [TestMethod]
    public void CorrectMultipleWithoutTypesAdding()
    {
        var f1 = new FilterDeclaration(new WorldDummy());
        f1.WithoutTypes(typeof(Component2), typeof(Component3), typeof(Component4));
        var types = f1.GetWithoutTypes();
        Assert.AreEqual(3, types.Length);
        Assert.IsTrue(types.Contains(typeof(Component2)));
        Assert.IsTrue(types.Contains(typeof(Component3)));
        Assert.IsTrue(types.Contains(typeof(Component4)));

        f1.WithoutTypes(typeof(Component1), typeof(Component5), typeof(Component5));
        types = f1.GetWithoutTypes();
        Assert.AreEqual(2, types.Length);
        Assert.IsTrue(types.Contains(typeof(Component1)));
        Assert.IsTrue(types.Contains(typeof(Component5)));
    }

    [TestMethod]
    public void CorrectTypesIntersectionHandlingWithMultipleAdding()
    {
        var f1 = new FilterDeclaration(new WorldDummy());
        f1.WithTypes(typeof(Component2), typeof(Component3), typeof(Component4));

        Assert.ThrowsException<Exception>(() =>
        {
            f1.WithoutTypes(typeof(Component1), typeof(Component3), typeof(Component5));
        });

        var f2 = new FilterDeclaration(new WorldDummy());
        f2.WithoutTypes(typeof(Component2), typeof(Component3), typeof(Component4));

        Assert.ThrowsException<Exception>(() =>
        {
            f2.WithTypes(typeof(Component1), typeof(Component3), typeof(Component5));
        });
    }

    [TestMethod]
    public void CorrectComparisonEquals()
    {
        var f1 = new FilterDeclaration(new WorldDummy());
        f1.WithTypes(typeof(Component2), typeof(Component3), typeof(Component4));
        f1.WithoutTypes(typeof(Component1), typeof(Component5));

        var f2 = new FilterDeclaration(new WorldDummy());
        f2.WithTypes(typeof(Component2), typeof(Component3), typeof(Component4));
        f2.WithoutTypes(typeof(Component1), typeof(Component5));

        Assert.IsTrue(f1.EqualTo(f2));
        Assert.IsTrue(f2.EqualTo(f1));
    }

    [TestMethod]
    public void CorrectComparisonNotEquals()
    {
        var f1 = new FilterDeclaration(new WorldDummy());
        f1.WithTypes(typeof(Component2), typeof(Component3), typeof(Component4));
        f1.WithoutTypes(typeof(Component1), typeof(Component5));

        var f2 = new FilterDeclaration(new WorldDummy());
        f2.WithTypes(typeof(Component3), typeof(Component4));
        f2.WithoutTypes(typeof(Component1), typeof(Component5));

        var f3 = new FilterDeclaration(new WorldDummy());
        f3.WithTypes(typeof(Component2), typeof(Component3), typeof(Component4));
        f3.WithoutTypes(typeof(Component1));

        Assert.IsFalse(f1.EqualTo(f2));
        Assert.IsFalse(f1.EqualTo(f3));

        Assert.IsFalse(f2.EqualTo(f1));
        Assert.IsFalse(f2.EqualTo(f3));

        Assert.IsFalse(f3.EqualTo(f2));
        Assert.IsFalse(f3.EqualTo(f1));
    }

    [TestMethod]
    public void CorrectHasWithType()
    {
        var declaration = new FilterDeclaration(new WorldDummy());
        Assert.IsFalse(declaration.HasWithType(typeof(Component2)));
        declaration.With<Component2>();
        Assert.IsTrue(declaration.HasWithType(typeof(Component2)));
        Assert.IsFalse(declaration.HasWithoutType(typeof(Component2)));
    }

    [TestMethod]
    public void CorrectHasWithoutType()
    {
        var declaration = new FilterDeclaration(new WorldDummy());
        Assert.IsFalse(declaration.HasWithoutType(typeof(Component1)));
        declaration.Without<Component1>();
        Assert.IsTrue(declaration.HasWithoutType(typeof(Component1)));
        Assert.IsFalse(declaration.HasWithType(typeof(Component1)));
    }

    private struct Component1;
    private struct Component2;
    private struct Component3;
    private struct Component4;
    private struct Component5;
}
