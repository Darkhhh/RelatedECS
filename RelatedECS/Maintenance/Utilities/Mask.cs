namespace RelatedECS.Maintenance.Utilities;

public class Mask
{
    internal const int BitSize = 64;
    private ulong[] _array = new ulong[2];

    public void Set(int index, bool value)
    {
        var arrayIndex = index / BitSize;
        var numIndex = index - arrayIndex * BitSize;
        Resize(index);

        if (value == GetBit(_array[arrayIndex], numIndex)) return;
        _array[arrayIndex] = SetBit(_array[arrayIndex], numIndex, value);
    }

    public bool Get(int index)
    {
        var arrayIndex = index / BitSize;
        var numIndex = index - arrayIndex * BitSize;
        Resize(index);

        return GetBit(_array[arrayIndex], numIndex);
    }

    public bool Compare(Mask other)
    {
        if (other is null || other._array.Length != _array.Length) return false;
        for (int i = 0; i < _array.Length; i++)
        {
            if (other._array[i] != _array[i]) return false;
        }
        return true;
    }

    public void Resize(int maxIndex)
    {
        if (maxIndex < 0 || maxIndex <= _array.Length * BitSize) return;
        var arrayIndex = maxIndex / BitSize;
        Array.Resize(ref _array, arrayIndex + 1);
    }

    public ulong this[int arrayIndex]
    {
        get => _array[arrayIndex];
        set => _array[arrayIndex] = value;
    }

    public void Clear()
    {
        for (int i = 0; i < _array.Length; i++) _array[i] = 0;
    }

    public bool IsEmpty => _array.All(t => t == 0);
    public int Length => _array.Length;

    private static ulong SetBit(ulong val, int index, bool bit)
    {
        if (index is < 0 or > BitSize) throw new Exception($"Incorrect bit index {index}");

        ulong tempValue = 1;
        tempValue <<= index;
        val &= ~tempValue;
        if (bit)
        {
            val |= tempValue;
        }
        return val;
    }

    private static bool GetBit(ulong val, int index)
    {
        if (index is > BitSize or < 0) throw new Exception($"Incorrect bit index {index}");
        return ((val >> index) & 1) > 0;
    }
}