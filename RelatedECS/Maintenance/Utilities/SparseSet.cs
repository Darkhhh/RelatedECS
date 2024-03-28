using System.Runtime.CompilerServices;

namespace RelatedECS.Maintenance.Utilities;

public struct SparseSet
{
    private int[] _sparse;
    private int[] _dense;
    private int _numberOfElements;
    private int _capacity;
    private int _maxValue;

    public SparseSet(int maxValue, int capacity)
    {
        _sparse = new int[maxValue + 1];
        _dense = new int[capacity];
        _capacity = capacity;
        _maxValue = maxValue;
        _numberOfElements = 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly int Find(int item)
    {
        if (item > _maxValue) return -1;
        if (_sparse[item] < _numberOfElements && _dense[_sparse[item]] == item) return _sparse[item];

        return -1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Insert(int item)
    {
        if (_numberOfElements == _capacity || item > _maxValue) return false;

        var indexOfDenseItem = _sparse[item];
        if (indexOfDenseItem < _numberOfElements && _dense[indexOfDenseItem] == item) return false;

        _dense[_numberOfElements] = item;
        _sparse[item] = _numberOfElements;
        _numberOfElements++;
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Delete(int item)
    {
        if (item > _maxValue) return;

        var indexOfDenseItem = _sparse[item];
        if (indexOfDenseItem >= _numberOfElements || _dense[indexOfDenseItem] != item) return;

        _numberOfElements--;
        _dense[indexOfDenseItem] = _dense[_numberOfElements];
        _sparse[_dense[_numberOfElements]] = indexOfDenseItem;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AllocateSparse(int newMaxValue)
    {
        if (newMaxValue <= _maxValue) return;
        Array.Resize(ref _sparse, newMaxValue);
        _maxValue = newMaxValue;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AllocateDense(int newCapacity)
    {
        if (newCapacity <= _capacity) return;
        Array.Resize(ref _dense, newCapacity);
        _capacity = newCapacity;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly int Count() => _numberOfElements;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Full() => _numberOfElements == _capacity;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly int GetByIndex(int index) => _dense[index];
}
