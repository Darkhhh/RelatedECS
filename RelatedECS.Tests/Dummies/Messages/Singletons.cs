using RelatedECS.Maintenance.Utilities;

namespace RelatedECS.Tests.Dummies.Messages;

public class SingletonInt : ISingletonMessage
{
    public int Message { get; set; }
}

public class SingletonString : ISingletonMessage
{
    public string Message { get; set; } = string.Empty;
}

public class SingletonDouble : ISingletonMessage
{
    public double Message { get; set; }
}

public class SingletonBool : ISingletonMessage
{
    public bool Message { get; set; }
}
