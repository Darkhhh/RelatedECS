using RelatedECS.Maintenance.Utilities;
using RelatedECS.Tests.Dummies.Messages;

namespace RelatedECS.Tests.Additional.ImplementationsTests;

[TestClass]
public class MessageBusCountTests
{
    [TestMethod]
    public void TestTransient()
    {
        Dictionary<Type, HashSet<IMessage>> transientMessages = new();
        Dictionary<Type, ISingletonMessage?> singletonMessages = new();

        transientMessages.Add(typeof(TransientInt), [new TransientInt { Message = 10 }]);
        transientMessages[typeof(TransientInt)].Add(new TransientInt { Message = 10 });
        transientMessages[typeof(TransientInt)].Add(new TransientInt { Message = 10 });

        Assert.AreEqual(3, Count<TransientInt>(transientMessages, singletonMessages));
        Assert.AreEqual(0, Count<TransientDouble>(transientMessages, singletonMessages));

        transientMessages.Clear();

        Assert.AreEqual(0, Count<TransientInt>(transientMessages, singletonMessages));
        Assert.AreEqual(0, Count<TransientDouble>(transientMessages, singletonMessages));
    }

    [TestMethod]
    public void TestSingletons()
    {
        Dictionary<Type, HashSet<IMessage>> transientMessages = new();
        Dictionary<Type, ISingletonMessage?> singletonMessages = new();

        singletonMessages.Add(typeof(SingletonInt), new SingletonInt { Message = 10 });

        Assert.AreEqual(1, Count<SingletonInt>(transientMessages, singletonMessages));

        singletonMessages[typeof(SingletonInt)] = null;


        Assert.AreEqual(0, Count<SingletonInt>(transientMessages, singletonMessages));


        transientMessages.Add(typeof(SingletonInt), [new SingletonInt { Message = 10 }]);

        Assert.AreEqual(1, Count<SingletonInt>(transientMessages, singletonMessages));
    }

    private static int Count<T>(Dictionary<Type, HashSet<IMessage>> transient, Dictionary<Type, ISingletonMessage?> singleton) where T : IMessage
    {
        return transient.TryGetValue(typeof(T), out var value) ? value.Count :
            (singleton.TryGetValue(typeof(T), out var single) ? (single is null ? 0 : 1) : 0);
    }
}
