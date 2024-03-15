using RelatedECS.Maintenance.Utilities;

namespace RelatedECS.Tests.Dummies.Messages;

public class TransientInt : IMessage
{
    public int Message { get; set; }
}

public class TransientString : IMessage
{
    public string Message { get; set; } = string.Empty;
}

public class TransientDouble : IMessage
{
    public double Message { get; set; }
}

public class TransientBool : IMessage
{
    public bool Message { get; set; }
}
