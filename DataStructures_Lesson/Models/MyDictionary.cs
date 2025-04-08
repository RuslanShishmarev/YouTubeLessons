using System.Collections;

namespace DataStructures_Lesson.Models;

public class MyDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
{
    private int _count = 100;
    private int _elementCount = 0;
    private const double LOAD_FACTOR = 0.9;

    private int?[] _backets;
    private MyEntry<TKey, TValue>[] _elements;

    private Stack<int> _freeIndexes;

    public MyDictionary()
    {
        _backets = new int?[_count];
        _elements = new MyEntry<TKey, TValue>[_count];
        _freeIndexes = new Stack<int>();
    }

    public void Add(TKey key, TValue value)
    {
        if ((double)_elementCount / _count >= LOAD_FACTOR)
        {
            RebaseHash();
        }
        Add(key, value, _backets, _elements, ref _elementCount);
    }

    public void Add(
        TKey key,
        TValue value,
        int?[] buckets,
        MyEntry<TKey, TValue>[] elements,
        ref int elementCount)
    {
        (int hashCode, int index) = GetHashIndex(key);

        int? bucket = buckets[index];

        int freeIndex = GetFreeIndex(elements);

        if (bucket.HasValue)
        {
            var existedElement = FindValue(bucket.Value, key);

            if (existedElement.HashCode != hashCode)
            {
                buckets[index] = freeIndex;

                elements[freeIndex] = new MyEntry<TKey, TValue>(key, value, hashCode);
                elements[freeIndex].Set();
                elements[freeIndex].Next = bucket.Value;

                elementCount++;
            }
            else
            {
                elements[bucket.Value].Value = value;
            }
        }
        else
        {
            elements[freeIndex] = new MyEntry<TKey, TValue>(key, value, hashCode);
            elements[freeIndex].Set();
            buckets[index] = freeIndex;
            elementCount++;
        }
    }

    private int GetFreeIndex(MyEntry<TKey, TValue>[] elements)
    {
        if (_freeIndexes.Count > 0)
        {
            return _freeIndexes.Pop();
        }

        for (int i = 0; i < elements.Length; i++)
        {
            if (elements[i].IsEmpty) return i;
        }

        throw new OutOfMemoryException();
    }

    private (int hashCode, int index) GetHashIndex(TKey key)
    {
        int hashCode = key.GetHashCode() & 0x7FFFFFFF;
        int index = hashCode % _count;
        return (hashCode, index);
    }

    private void RebaseHash()
    {
        int oldCount = _count;

        _count *= 2;

        var newBuckets = new int?[_count];
        var newElements = new MyEntry<TKey, TValue>[_count];
        _freeIndexes = new Stack<int>();

        int newCount = 0;

        foreach (var element in _elements)
        {
            if (element.IsEmpty) continue;
            Add(element.Key, element.Value, newBuckets, newElements, ref newCount);
        }

        _elements = newElements;
        _backets = newBuckets;
        _elementCount = newCount;
    }

    public TValue? Get(TKey key)
    {
        (_, int index) = GetHashIndex(key);

        int? bucket = _backets[index];

        if (!bucket.HasValue) return default;

        var element = FindValue(bucket.Value, key);

        if (element.IsEmpty) return default;

        return element.Value;
    }

    private MyEntry<TKey, TValue> FindValue(int bucketIndex, TKey key)
    {
        var element = _elements[bucketIndex];

        if (element.Key.Equals(key))
        {
            return element;
        }
        if (element.Next is null) return element;
        else
            return FindValue(element.Next.Value, key);
    }

    public void Remove(TKey key)
    {
        if (_elementCount == 0) throw new Exception("Disionary is empty");

        (_, int index) = GetHashIndex(key);

        int? bucket = _backets[index];

        if (bucket == null) throw new NullReferenceException("Value is null");

        _elements[bucket.Value] = default;
        _backets[index] = null;
        _elementCount--;

        _freeIndexes.Push(bucket.Value);
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        foreach (var element in _elements)
        {
            if (element.IsEmpty) continue;
            yield return new KeyValuePair<TKey, TValue>(element.Key, element.Value);
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public TValue this[TKey key]
    {
        get => Get(key);
        set
        {
            Add(key, value);
        }
    }
}

public struct MyEntry<TKey, TValue>
{
    public int? Next { get; set; }

    public int HashCode { get; }

    public TKey Key { get; }

    public TValue? Value { get; set; }

    private bool _withValue;

    public readonly bool IsEmpty =>!_withValue;

    public MyEntry(TKey key, TValue value, int hashCode)
    {
        Key = key;
        Value = value;
        HashCode = hashCode;
    }

    public void Set() => _withValue = true;
}
