using RelatedECS.Maintenance.Utilities;
using RelatedECS.Systems;
using RelatedECS.Tests.Dummies;
using RelatedECS.Tests.Dummies.MaintenanceDataStructures;
using RelatedECS.Tests.Dummies.Messages;

namespace RelatedECS.Tests.Utilities;

[TestClass]
public class MessageBusTests
{
    [TestMethod]
    public void CorrectTransientAdding()
    {
        IMessageBus bus = new MessageBus(new WorldDummy());

        bus.Add(new TransientInt() { Message = 10 });
        Assert.AreEqual(1, bus.Count<TransientInt>());
        bus.Add(new TransientInt() { Message = 10 });
        Assert.AreEqual(2, bus.Count<TransientInt>());

        Assert.AreEqual(0, bus.Count<TransientDouble>());
    }

    [TestMethod]
    public void CorrectTransientHas()
    {
        IMessageBus bus = new MessageBus(new WorldDummy());
        bus.Add(new TransientInt() { Message = 10 });
        Assert.IsTrue(bus.Has<TransientInt>());

        Assert.IsFalse(bus.Has<TransientDouble>());
        bus.Add(new TransientDouble() { Message = 2.4 });
        Assert.IsTrue(bus.Has<TransientDouble>());
    }

    [TestMethod]
    public void CorrectTransientTryPop()
    {
        IMessageBus bus = new MessageBus(new WorldDummy());
        bus.Add(new TransientInt() { Message = 10 });

        TransientInt? message;
        Assert.IsTrue(bus.TryPop<TransientInt>(out message));
        Assert.IsNotNull(message);
        Assert.AreEqual(10, message.Message);

        bus.Add(new TransientInt() { Message = 12 });
        bus.Add(new TransientInt() { Message = 13 });
        bus.Add(new TransientInt() { Message = 14 });

        Assert.IsTrue(bus.TryPop<TransientInt>(out message));
        Assert.IsNotNull(message);
        Assert.AreEqual(14, message.Message);

        Assert.IsTrue(bus.TryPop<TransientInt>(out message));
        Assert.IsNotNull(message);
        Assert.AreEqual(13, message.Message);

        Assert.IsTrue(bus.TryPop<TransientInt>(out message));
        Assert.IsNotNull(message);
        Assert.AreEqual(12, message.Message);

        Assert.IsFalse(bus.TryPop<TransientInt>(out message));
        Assert.IsNull(message);

        Assert.IsFalse(bus.TryPop<TransientDouble>(out var notExistingMessage));
        Assert.IsNull(notExistingMessage);
    }

    [TestMethod]
    public void CorrectTransientTryPeek()
    {
        IMessageBus bus = new MessageBus(new WorldDummy());
        bus.Add(new TransientInt() { Message = 10 });

        TransientInt? message;
        Assert.IsTrue(bus.TryPeek<TransientInt>(out message));
        Assert.IsNotNull(message);
        Assert.AreEqual(10, message.Message);
        Assert.AreEqual(1, bus.Count<TransientInt>());

        bus.Add(new TransientInt() { Message = 12 });
        Assert.IsTrue(bus.TryPeek<TransientInt>(out message));
        Assert.IsNotNull(message);
        Assert.AreEqual(12, message.Message);
        Assert.AreEqual(2, bus.Count<TransientInt>());
    }

    [TestMethod]
    public void CorrectTransientTryPopFirst()
    {
        IMessageBus bus = new MessageBus(new WorldDummy());
        bus.Add(new TransientInt() { Message = 10 });

        TransientInt? message;
        Assert.IsTrue(bus.TryPopFirst<TransientInt>(out message));
        Assert.IsNotNull(message);
        Assert.AreEqual(10, message.Message);
        Assert.AreEqual(0, bus.Count<TransientInt>());

        bus.Add(new TransientInt() { Message = 12 });
        bus.Add(new TransientInt() { Message = 13 });
        bus.Add(new TransientInt() { Message = 14 });

        Assert.IsTrue(bus.TryPopFirst<TransientInt>(out message));
        Assert.IsNotNull(message);
        Assert.AreEqual(12, message.Message);

        Assert.IsTrue(bus.TryPopFirst<TransientInt>(out message));
        Assert.IsNotNull(message);
        Assert.AreEqual(13, message.Message);

        Assert.IsTrue(bus.TryPopFirst<TransientInt>(out message));
        Assert.IsNotNull(message);
        Assert.AreEqual(14, message.Message);

        Assert.IsFalse(bus.TryPopFirst<TransientInt>(out message));
        Assert.IsNull(message);
    }

    [TestMethod]
    public void CorrectTransientTryPeekFirst()
    {
        IMessageBus bus = new MessageBus(new WorldDummy());
        bus.Add(new TransientInt() { Message = 10 });

        TransientInt? message;
        Assert.IsTrue(bus.TryPeekFirst<TransientInt>(out message));
        Assert.IsNotNull(message);
        Assert.AreEqual(10, message.Message);
        Assert.AreEqual(1, bus.Count<TransientInt>());

        bus.Add(new TransientInt() { Message = 12 });
        Assert.IsTrue(bus.TryPeekFirst<TransientInt>(out message));
        Assert.IsNotNull(message);
        Assert.AreEqual(10, message.Message);
        Assert.AreEqual(2, bus.Count<TransientInt>());
    }

    [TestMethod]
    public void CorrectRemoveMessage()
    {
        IMessageBus bus = new MessageBus(new WorldDummy());
        var message = new TransientInt() { Message = 10 };
        bus.Add(message);
        Assert.AreEqual(1, bus.Count<TransientInt>());
        bus.RemoveMessage(message);
        Assert.AreEqual(0, bus.Count<TransientInt>());
    }

    [TestMethod]
    public void CorrectSingletonAdd()
    {
        IMessageBus bus = new MessageBus(new WorldDummy());

        bus.AddSingleton(new SingletonInt() { Message = 10 });
        Assert.AreEqual(1, bus.Count<SingletonInt>());

        Assert.ThrowsException<Exception>(() =>
        {
            bus.AddSingleton(new SingletonInt() { Message = 10 });
        });
    }

    [TestMethod]
    public void CorrectSingletonGet()
    {
        IMessageBus bus = new MessageBus(new WorldDummy());

        bus.AddSingleton(new SingletonInt() { Message = 10 });

        Assert.ThrowsException<Exception>(() =>
        {
            bus.GetSingleton<SingletonDouble>();
        });

        Assert.AreEqual(10, bus.GetSingleton<SingletonInt>().Message);
    }

    [TestMethod]
    public void CorrectSingletonHas()
    {
        IMessageBus bus = new MessageBus(new WorldDummy());

        bus.AddSingleton(new SingletonInt() { Message = 10 });

        Assert.IsTrue(bus.HasSingleton<SingletonInt>());
        Assert.IsFalse(bus.HasSingleton<SingletonDouble>());
    }

    [TestMethod]
    public void CorrectSingletonRemove()
    {
        IMessageBus bus = new MessageBus(new WorldDummy());

        bus.AddSingleton(new SingletonInt() { Message = 10 });
        Assert.IsTrue(bus.HasSingleton<SingletonInt>());

        bus.RemoveSingleton<SingletonInt>();
        Assert.IsFalse(bus.HasSingleton<SingletonInt>());

        bus.RemoveSingleton<SingletonBool>();
    }

    [TestMethod]
    public void CorrectClearMessagesOfType()
    {
        IMessageBus bus = new MessageBus(new WorldDummy());

        bus.Add(new TransientBool() { Message = true });
        bus.Add(new TransientBool() { Message = true });
        bus.Add(new TransientBool() { Message = true });
        bus.Add(new TransientBool() { Message = true });
        Assert.AreEqual(4, bus.Count<TransientBool>());

        bus.Clear<TransientBool>();
        Assert.AreEqual(0, bus.Count<TransientBool>());

        bus.AddSingleton(new SingletonBool() { Message = true });
        Assert.IsTrue(bus.HasSingleton<SingletonBool>());

        bus.Clear<SingletonBool>();
        Assert.IsFalse(bus.HasSingleton<SingletonBool>());

        bus.Add(new SingletonBool() { Message = true });
        bus.Add(new SingletonBool() { Message = true });
        bus.Add(new SingletonBool() { Message = true });
        bus.Add(new SingletonBool() { Message = true });
        bus.AddSingleton(new SingletonBool() { Message = true });
        Assert.AreEqual(4, bus.Count<SingletonBool>());
        Assert.IsTrue(bus.HasSingleton<SingletonBool>());

        bus.Clear<SingletonBool>();
        Assert.AreEqual(0, bus.Count<SingletonBool>());
        Assert.IsFalse(bus.HasSingleton<SingletonBool>());
    }

    [TestMethod]
    public void CorrectClearAll()
    {
        IMessageBus bus = new MessageBus(new WorldDummy());

        bus.Add(new TransientInt() { Message = 1 });
        bus.Add(new TransientInt() { Message = 2 });
        bus.Add(new TransientInt() { Message = 3 });
        bus.Add(new TransientInt() { Message = 4 });

        bus.Add(new TransientBool() { Message = true });
        bus.Add(new TransientBool() { Message = true });
        bus.Add(new TransientBool() { Message = true });
        bus.Add(new TransientBool() { Message = true });

        bus.AddSingleton(new SingletonInt() { Message = 1 });
        bus.AddSingleton(new SingletonBool() { Message= true });

        bus.ClearAll();

        Assert.AreEqual(0, bus.Count<TransientInt>());
        Assert.AreEqual(0, bus.Count<TransientBool>());
        Assert.IsFalse(bus.HasSingleton<SingletonInt>());
        Assert.IsFalse(bus.HasSingleton<SingletonBool>());
    }

    [TestMethod]
    public void CorrectGetClearSystem()
    {
        var world = new WorldDummy();
        var collection = new SystemsCollection(world);
        IMessageBus bus = new MessageBus(world);
        var system1 = bus.GetClearSystemFor<TransientInt>();
        var system2 = bus.GetClearSystemFor<TransientBool>();
        var system3 = bus.GetClearSystemFor<SingletonInt>();
        var system4 = bus.GetClearSystemFor<SingletonBool>();

        bus.Add(new TransientInt() { Message = 1 });
        bus.Add(new TransientInt() { Message = 2 });
        bus.Add(new TransientInt() { Message = 3 });
        bus.Add(new TransientInt() { Message = 4 });

        bus.Add(new TransientBool() { Message = true });
        bus.Add(new TransientBool() { Message = true });
        bus.Add(new TransientBool() { Message = true });
        bus.Add(new TransientBool() { Message = true });

        bus.AddSingleton(new SingletonInt() { Message = 1 });
        bus.AddSingleton(new SingletonBool() { Message = true });

        system1.FrameDispose(collection);
        system2.FrameDispose(collection);
        system3.FrameDispose(collection);
        system4.FrameDispose(collection);

        Assert.AreEqual(0, bus.Count<TransientInt>());
        Assert.AreEqual(0, bus.Count<TransientBool>());
        Assert.IsFalse(bus.HasSingleton<SingletonInt>());
        Assert.IsFalse(bus.HasSingleton<SingletonBool>());
    }

    [TestMethod]
    public void CorrectSubscriptionForTransientType()
    {
        var data = new StringData();
        IMessageBus bus = new MessageBus(new WorldDummy());

        bus.SubscribeTo<TransientInt>((_) =>
        {
            if (bus.TryPeek<TransientInt>(out var message))
            {
                data.Append(message.Message.ToString());
            }
        });
        bus.SubscribeTo<TransientInt>((_) =>
        {
            if (bus.TryPeek<TransientInt>(out var message))
            {
                data.Append(message.Message.ToString());
            }
        });

        bus.Add(new TransientInt() { Message = 1 });
        Assert.AreEqual("11", data.ToString());
        bus.Add(new TransientInt() { Message = 2 });
        Assert.AreEqual("1122", data.ToString());
    }

    [TestMethod]
    public void CorrectSubscriptionForSingletonType()
    {
        var data = new StringData();
        IMessageBus bus = new MessageBus(new WorldDummy());

        bus.SubscribeTo<SingletonInt>((_) =>
        {
            data.Append(bus.GetSingleton<SingletonInt>().Message.ToString());
        });

        bus.AddSingleton(new SingletonInt() { Message = 1 });
        Assert.AreEqual("1", data.ToString());
        bus.RemoveSingleton<SingletonInt>();
        bus.AddSingleton(new SingletonInt() { Message = 4 });
        Assert.AreEqual("14", data.ToString());
    }

    [TestMethod]
    public void CorrectUnsubscribeFromTransientType()
    {
        var data = new StringData();
        IMessageBus bus = new MessageBus(new WorldDummy());

        bus.SubscribeTo<TransientInt>((_) =>
        {
            if (bus.TryPeek<TransientInt>(out var message))
            {
                data.Append(message.Message.ToString());
            }
        });
        Action<IWorld> action = (_) =>
        {
            if (bus.TryPeek<TransientInt>(out var message))
            {
                data.Append(message.Message.ToString());
            }
        };
        bus.SubscribeTo<TransientInt>(action);

        bus.Add(new TransientInt() { Message = 1 });
        Assert.AreEqual("11", data.ToString());

        bus.UnsubscribeFrom<TransientInt>(action);

        bus.Add(new TransientInt() { Message = 2 });
        Assert.AreEqual("112", data.ToString());

        bus.SubscribeTo<TransientInt>(Handle);

        bus.Add(new TransientInt() { Message = 3 });
        Assert.AreEqual("11233", data.ToString());

        bus.UnsubscribeFrom<TransientInt>(Handle);
        bus.Add(new TransientInt() { Message = 4 });
        Assert.AreEqual("112334", data.ToString());

        void Handle(IWorld world)
        {
            if (bus.TryPeek<TransientInt>(out var message))
            {
                data.Append(message.Message.ToString());
            }
        }
    }

    [TestMethod]
    public void CorrectUnsubscribeFromSingletonType()
    {
        var data = new StringData();
        IMessageBus bus = new MessageBus(new WorldDummy());

        Action<IWorld> action = (_) =>
        {
            data.Append(bus.GetSingleton<SingletonInt>().Message.ToString());
        };

        bus.SubscribeTo<SingletonInt>(action);

        bus.AddSingleton(new SingletonInt() { Message = 1 });
        Assert.AreEqual("1", data.ToString());
        bus.RemoveSingleton<SingletonInt>();
        bus.UnsubscribeFrom<SingletonInt>(action);
        bus.AddSingleton(new SingletonInt() { Message = 4 });
        Assert.AreEqual("1", data.ToString());
    }

    [TestMethod]
    public void CorrectAllUnsubcribeForTransientType()
    {
        var data = new StringData();
        IMessageBus bus = new MessageBus(new WorldDummy());

        bus.SubscribeTo<TransientInt>((_) =>
        {
            if (bus.TryPeek<TransientInt>(out var message))
            {
                data.Append(message.Message.ToString());
            }
        });
        Action<IWorld> action = (_) =>
        {
            if (bus.TryPeek<TransientInt>(out var message))
            {
                data.Append(message.Message.ToString());
            }
        };
        bus.SubscribeTo<TransientInt>(action);

        bus.Add(new TransientInt() { Message = 1 });
        Assert.AreEqual("11", data.ToString());

        bus.UnsubscribeAllFrom<TransientInt>();
        bus.Add(new TransientInt() { Message = 4 });
        bus.Add(new TransientInt() { Message = 6 });
        Assert.AreEqual("11", data.ToString());
    }

    [TestMethod]
    public void CorrectUnsubscribeAllForSingletonType()
    {
        var data = new StringData();
        IMessageBus bus = new MessageBus(new WorldDummy());

        Action<IWorld> action = (_) =>
        {
            data.Append(bus.GetSingleton<SingletonInt>().Message.ToString());
        };

        bus.SubscribeTo<SingletonInt>(action);
        bus.SubscribeTo<SingletonInt>((_) =>
        {
            data.Append(bus.GetSingleton<SingletonInt>().Message.ToString());
        });

        bus.AddSingleton(new SingletonInt() { Message = 1 });
        Assert.AreEqual("11", data.ToString());
        bus.RemoveSingleton<SingletonInt>();
        bus.AddSingleton(new SingletonInt() { Message = 4 });
        Assert.AreEqual("1144", data.ToString());

        bus.UnsubscribeAllFrom<SingletonInt>();
        bus.RemoveSingleton<SingletonInt>();
        bus.AddSingleton(new SingletonInt() { Message = 7 });
        Assert.AreEqual("1144", data.ToString());
    }

    [TestMethod]
    public void CorrectClearAllSubscriptions()
    {
        var data = new StringData();
        IMessageBus bus = new MessageBus(new WorldDummy());

        Action<IWorld> action = (_) =>
        {
            if (bus.TryPeek<TransientInt>(out var message))
            {
                data.Append(message.Message.ToString());
            }
        };
        Action<IWorld> singleAction = (_) =>
        {
            data.Append(bus.GetSingleton<SingletonInt>().Message.ToString());
        };

        bus.SubscribeTo<SingletonInt>(singleAction);
        bus.SubscribeTo<SingletonInt>((_) =>
        {
            data.Append(bus.GetSingleton<SingletonInt>().Message.ToString());
        });

        bus.SubscribeTo<TransientInt>(action);
        bus.SubscribeTo<TransientInt>((_) =>
        {
            if (bus.TryPeek<TransientInt>(out var message))
            {
                data.Append(message.Message.ToString());
            }
        });


        bus.Add(new TransientInt() { Message = 1 });
        bus.AddSingleton(new SingletonInt() { Message = 2 });
        Assert.AreEqual("1122", data.ToString());
        bus.RemoveSingleton<SingletonInt>();

        bus.ClearAllSubscriptions();

        bus.Add(new TransientInt() { Message = 1 });
        bus.AddSingleton(new SingletonInt() { Message = 2 });
        Assert.AreEqual("1122", data.ToString());
    }

    [TestMethod]
    public void CorrectCountSubsMethod()
    {
        var data = new StringData();
        IMessageBus bus = new MessageBus(new WorldDummy());

        Action<IWorld> action = (_) =>
        {
            if (bus.TryPeek<TransientInt>(out var message))
            {
                data.Append(message.Message.ToString());
            }
        };
        Action<IWorld> singleAction = (_) =>
        {
            data.Append(bus.GetSingleton<SingletonInt>().Message.ToString());
        };

        bus.SubscribeTo<SingletonInt>(singleAction);
        bus.SubscribeTo<SingletonInt>((_) =>
        {
            data.Append(bus.GetSingleton<SingletonInt>().Message.ToString());
        });

        bus.SubscribeTo<TransientInt>(action);
        bus.SubscribeTo<TransientInt>((_) =>
        {
            if (bus.TryPeek<TransientInt>(out var message))
            {
                data.Append(message.Message.ToString());
            }
        });

        Assert.AreEqual(2, bus.CountSubsFor<TransientInt>());
        Assert.AreEqual(2, bus.CountSubsFor<SingletonInt>());

        bus.UnsubscribeAllFrom<SingletonInt>();
        bus.UnsubscribeAllFrom<TransientInt>();

        Assert.AreEqual(0, bus.CountSubsFor<TransientInt>());
        Assert.AreEqual(0, bus.CountSubsFor<SingletonInt>());
    }
}
