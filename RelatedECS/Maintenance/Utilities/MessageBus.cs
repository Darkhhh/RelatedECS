using RelatedECS.Systems;
using System.Diagnostics.CodeAnalysis;

namespace RelatedECS.Maintenance.Utilities;

public interface IMessage;
public interface ISingletonMessage : IMessage;


public interface IMessageBus
{
    /// <summary>
    /// Adding new message instance.
    /// </summary>
    /// <typeparam name="T">Message type.</typeparam>
    /// <param name="message">Message instance.</param>
    public void Add<T>(T message) where T : IMessage;

    /// <summary>
    /// Check if bus have messages of required type.
    /// </summary>
    /// <typeparam name="T">Required message type.</typeparam>
    /// <returns>True if there more than zero messages of required type, otherwise - False.</returns>
    public bool Has<T>() where T : IMessage;

    /// <summary>
    /// Getting last added message of type with removing.
    /// </summary>
    /// <typeparam name="T">Required message type.</typeparam>
    /// <param name="message">Message instance. Maybe null when return is false.</param>
    /// <returns>True if element was found, False if there is no messages of required type.</returns>
    public bool TryPop<T>([MaybeNullWhen(false)] out T message) where T : IMessage;

    /// <summary>
    /// Getting last added message of type without removing.
    /// </summary>
    /// <typeparam name="T">Required message type.</typeparam>
    /// <param name="message">Message instance. Maybe null when return is false.</param>
    /// <returns>True if element was found, False if there is no messages of required type.</returns>
    public bool TryPeek<T>([MaybeNullWhen(false)] out T message) where T : IMessage;

    /// <summary>
    /// Getting first added message of type with removing.
    /// </summary>
    /// <typeparam name="T">Required message type.</typeparam>
    /// <param name="message">Message instance. Maybe null when return is false.</param>
    /// <returns>True if element was found, False if there is no messages of required type.</returns>
    public bool TryPopFirst<T>([MaybeNullWhen(false)] out T message) where T : IMessage;

    /// <summary>
    /// Getting first added message of type without removing.
    /// </summary>
    /// <typeparam name="T">Required message type.</typeparam>
    /// <param name="message">Message instance. Maybe null when return is false.</param>
    /// <returns>True if element was found, False if there is no messages of required type.</returns>
    public bool TryPeekFirst<T>([MaybeNullWhen(false)] out T message) where T : IMessage;

    /// <summary>
    /// Removing message instance from bus. For singleton message use RemoveSingleton.
    /// </summary>
    /// <typeparam name="T">Message type.</typeparam>
    /// <param name="message">Message instance.</param>
    public void RemoveMessage<T>(T message) where T : IMessage;

    /// <summary>
    /// Count messages of required type. Singleton compatible.
    /// </summary>
    /// <typeparam name="T">Messages type.</typeparam>
    /// <returns>Number of messages of required type.</returns>
    public int Count<T>() where T : IMessage;

    /// <summary>
    /// Removing all messages of required type. Singleton compatible.
    /// </summary>
    /// <typeparam name="T">Messages type.</typeparam>
    public void Clear<T>() where T : IMessage;

    /// <summary>
    /// Clearing all messages.
    /// </summary>
    public void ClearAll();

    /// <summary>
    /// Adding single instance of message. Throws exception when message already added.
    /// </summary>
    /// <typeparam name="T">Message type.</typeparam>
    /// <param name="message">Message instance.</param>
    public void AddSingleton<T>(T message) where T : ISingletonMessage;

    /// <summary>
    /// Check if single message already added.
    /// </summary>
    /// <typeparam name="T">Message type.</typeparam>
    /// <returns>True if instance added, otherwise false.</returns>
    public bool HasSingleton<T>() where T : ISingletonMessage;

    /// <summary>
    /// Get single message. Throws an exception if message does not exist.
    /// </summary>
    /// <typeparam name="T">Message type.</typeparam>
    /// <returns>Message instance.</returns>
    public T GetSingleton<T>() where T : ISingletonMessage;

    /// <summary>
    /// Removing single message of required type. If message does not exist does nothing.
    /// </summary>
    /// <typeparam name="T">Message type.</typeparam>
    public void RemoveSingleton<T>() where T : ISingletonMessage;

    /// <summary>
    /// Get automatic clear system for messages of required type. Singleton compatible.
    /// </summary>
    /// <typeparam name="T">Message type.</typeparam>
    /// <returns>IFrameDispose automatic clear system.</returns>
    public IFrameDisposeSystem GetClearSystemFor<T>() where T : IMessage;

    /// <summary>
    /// Get all messages of required type.
    /// </summary>
    /// <typeparam name="T">Message type.</typeparam>
    /// <returns>Messages collection. May be empty if there is no messages of type exist.</returns>
    public IEnumerable<T> GetMessages<T>() where T : IMessage;

    /// <summary>
    /// Add action that will be called when new message of required type is added. Singleton compatible.
    /// Only unique (by hash) actions will be added, no duplicates allowed.
    /// </summary>
    /// <typeparam name="T">Message type.</typeparam>
    /// <param name="action">Subscription action.</param>
    public void SubscribeTo<T>(Action<IWorld> action) where T : IMessage;

    /// <summary>
    /// Remove action from call list.
    /// </summary>
    /// <typeparam name="T">Message type.</typeparam>
    /// <param name="action">Subscription action.</param>
    public void UnsubscribeFrom<T>(Action<IWorld> action) where T : IMessage;

    /// <summary>
    /// Remove all subscriptions to messages of type.
    /// </summary>
    /// <typeparam name="T">Message type.</typeparam>
    public void UnsubscribeAllFrom<T>() where T : IMessage;

    /// <summary>
    /// Remove all subscriptions.
    /// </summary>
    public void ClearAllSubscriptions();

    /// <summary>
    /// Count all action-subscriptions to required message type.
    /// </summary>
    /// <typeparam name="T">Message type.</typeparam>
    /// <returns>Number of actions.</returns>
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

    public void UnsubscribeAllFrom<T>() where T : IMessage
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

    public void Clear<T>() where T : IMessage
    {
        if (_transientMessages.TryGetValue(typeof(T), out var value)) value.Clear();
        if (_singletonMessages.TryGetValue(typeof(T), out var _)) _singletonMessages[typeof(T)] = null;
    }

    public void ClearAll()
    {
        foreach(var messages in _transientMessages.Values) messages.Clear();
        _transientMessages.Clear();
        _singletonMessages.Clear();
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
        public void FrameDispose(IWorld world) => bus.Clear<T>();
    }
}
