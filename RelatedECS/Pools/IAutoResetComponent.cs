namespace RelatedECS.Pools;

public delegate void AutoResetHandle<T>(ref T value);

public interface IAutoResetComponent<T> where T : struct
{
    public static abstract void AutoReset(ref T instance);
}
