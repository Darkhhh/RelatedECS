using RelatedECS.Systems;
using System.Diagnostics.CodeAnalysis;

namespace RelatedECS.Maintenance.Utilities;

public interface IMessage;
public interface ISingletonMessage : IMessage;


public interface IMessageBus
{
    public void Add<T>(T message) where T : IMessage;

    public bool Has<T>() where T : IMessage;

    public bool TryPop<T>([MaybeNullWhen(false)] out T message) where T : IMessage;

    public bool TryPeek<T>([MaybeNullWhen(false)] out T message) where T : IMessage;

    public bool TryPopFirst<T>([MaybeNullWhen(false)] out T message) where T : IMessage;

    public bool TryPeekFirst<T>([MaybeNullWhen(false)] out T message) where T : IMessage;

    public void RemoveMessage<T>(T message) where T : IMessage;

    public int Count<T>() where T : IMessage;

    public void ClearAll<T>() where T : IMessage;

    public void AddSingleton<T>(T message) where T : ISingletonMessage;

    public bool HasSingleton<T>() where T : ISingletonMessage;

    public T GetSingleton<T>() where T : ISingletonMessage;

    public void RemoveSingleton<T>() where T : ISingletonMessage;

    public IFrameDisposeSystem GetClearSystemFor<T>() where T : IMessage;

    public IEnumerable<T> GetMessages<T>() where T : IMessage;

    public void SubscribeTo<T>(Action<IWorld> action) where T : IMessage;

    public void UnsubscribeFrom<T>(Action<IWorld> action) where T : IMessage;

    public void ClearAllSubscriptions<T>() where T : IMessage;

    public void ClearAllSubscriptions();

    public int CountSubsFor<T>() where T : IMessage;
}

public class MessageBus(IWorld world) : IMessageBus
{
    private readonly Dictionary<Type, HashSet<IMessage>> _transientMessages = new();
    private readonly Dictionary<Type, ISingletonMessage?> _singletonMessages = new();

    private readonly Dictionary<Type, HashSet<Action<IWorld>>> _subscribers = new();


    #region Transient

    public void Add<T>(T message) where T : IMessage
    {
        var type = typeof(T);
        if (_transientMessages.TryGetValue(type, out var value))
        {
            value.Add(message);
        }
        else
        {
            _transientMessages.Add(type, [message]);
        }

        if (!_subscribers.TryGetValue(type, out var subs)) return;

        foreach (var item in subs) item.Invoke(world);
    }

    public IEnumerable<T> GetMessages<T>() where T : IMessage
    {
        return _transientMessages.TryGetValue(typeof(T), out var value) ? value.Cast<T>() : Enumerable.Empty<T>();
    }

    public bool Has<T>() where T : IMessage
    {
        return _transientMessages.TryGetValue(typeof(T), out var value) && value.Count > 0;
    }

    public void RemoveMessage<T>(T message) where T : IMessage
    {
        if (!_transientMessages.TryGetValue(typeof(T),out var value)) return;
        value.Remove(message);
    }

    public bool TryPop<T>([MaybeNullWhen(false)] out T message) where T : IMessage
    {
        message = default;
        if (!_transientMessages.TryGetValue(typeof(T), out var list)) return false;
        if (list.Count == 0) return false;
        var temp = list.Last();
        list.Remove(temp);
        message = (T) temp;
        return true;
    }

    public bool TryPeek<T>([MaybeNullWhen(false)] out T message) where T : IMessage
    {
        message = default;
        if (!_transientMessages.TryGetValue(typeof(T), out var list)) return false;
        if (list.Count == 0) return false;
        message = (T) list.Last();
        return true;
    }

    public bool TryPopFirst<T>([MaybeNullWhen(false)] out T message) where T : IMessage
    {
        message = default;
        if (!_transientMessages.TryGetValue(typeof(T), out var list)) return false;
        if (list.Count == 0) return false;
        var temp = list.First();
        list.Remove(temp);
        message = (T)temp;
        return true;
    }

    public bool TryPeekFirst<T>([MaybeNullWhen(false)] out T message) where T : IMessage
    {
        message = default;
        if (!_transientMessages.TryGetValue(typeof(T), out var list)) return false;
        if (list.Count == 0) return false;
        message = (T) list.First();
        return true;
    }

    #endregion


    #region Singleton

    public void AddSingleton<T>(T message) where T : ISingletonMessage
    {
        var type = typeof(T);
        if (_singletonMessages.TryGetValue(type, out var value))
        {
            if (value is not null) throw new Exception($"Single message of type {type.Name} already exists");
            _singletonMessages[type] = message;
        }
        else
        {
            _singletonMessages.Add(type, message);
        }

        if (!_subscribers.TryGetValue(type, out var subs)) return;

        foreach (var item in subs) item.Invoke(world);
    }

    public T GetSingleton<T>() where T : ISingletonMessage
    {
        if (_singletonMessages.TryGetValue(typeof(T), out var value))
        {
            if (value is not null) return (T) value;
        }
        throw new Exception($"Single message of type {typeof(T).Name} do not exist");
    }

    public bool HasSingleton<T>() where T : ISingletonMessage
    {
        return _singletonMessages.TryGetValue(typeof(T), out var value) && value is not null;
    }

    public void RemoveSingleton<T>() where T : ISingletonMessage
    {
        if (_singletonMessages.TryGetValue(typeof(T), out var value) && value is not null)
        {
            _singletonMessages[typeof(T)] = null;
        }
    }

    #endregion


    #region Subscriptions

    public void SubscribeTo<T>(Action<IWorld> action) where T : IMessage
    {
        if (_subscribers.TryGetValue(typeof(T), out var value)) value.Add(action);
        else _subscribers.Add(typeof(T), [action]);
    }

    public void UnsubscribeFrom<T>(Action<IWorld> action) where T : IMessage
    {
        if (!_subscribers.TryGetValue(typeof(T), out var value)) return;
        value.Remove(action);
    }

    public void ClearAllSubscriptions<T>() where T : IMessage
    {
        if (!_subscribers.TryGetValue(typeof(T), out var value)) return;
        value.Clear();
    }

    public void ClearAllSubscriptions()
    {
        foreach (var subscriber in _subscribers.Values) subscriber.Clear();
        _subscribers.Clear();
    }

    #endregion


    #region Utilities

    public IFrameDisposeSystem GetClearSystemFor<T>() where T : IMessage => new ClearSystem<T>(this);

    public void ClearAll<T>() where T : IMessage
    {
        if (_transientMessages.TryGetValue(typeof(T), out var value)) value.Clear();
        if (_singletonMessages.TryGetValue(typeof(T), out var _)) _singletonMessages[typeof(T)] = null;
    }

    #endregion


    #region Properties

    public int Count<T>() where T : IMessage
    {
        return _transientMessages.TryGetValue(typeof(T), out var value) ? value.Count : 
            (_singletonMessages.TryGetValue(typeof(T), out var single) ? (single is null ? 0 : 1) : 0);
    }

    public int CountSubsFor<T>() where T : IMessage
    {
        return _subscribers.TryGetValue(typeof(T), out var subs) ? subs.Count : 0;
    }

    #endregion


    private class ClearSystem<T>(IMessageBus bus) : IFrameDisposeSystem where T : IMessage
    {
        public void FrameDispose(IWorld world) => bus.ClearAll<T>();
    }
}
