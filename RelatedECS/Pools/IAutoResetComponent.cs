namespace RelatedECS.Pools;

public delegate void AutoResetHandle<T>(ref T value) where T : struct;

public interface IAutoResetComponent<T> where T : struct
{
    public static abstract void AutoReset(ref T instance);
}