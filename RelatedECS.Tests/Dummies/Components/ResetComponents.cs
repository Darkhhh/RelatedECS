using RelatedECS.Pools;

namespace RelatedECS.Tests.Dummies.Components;

public struct Volume
{
    public int Value;
}

public struct AutoResetVolume : IAutoResetComponent<AutoResetVolume>
{
    public const int InitialValue = 24;

    public int Value;

    public static void AutoReset(ref AutoResetVolume instance)
    {
        instance.Value = InitialValue;
    }
}
