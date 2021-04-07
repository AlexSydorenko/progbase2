using System;

class ArraySetInt : ISetInt
{
    private int[] _items;
    private int _size;
    public ArraySetInt()
    {
        _items = new int[16];
        _size = 0;
    }

    public bool Add(int value)
    {
        if (this.Contains(value))
        {
            return false;
        }

        if (_size == _items.Length)
        {
            Array.Resize(ref _items, _size * 2);
        }

        for (int i = 0; i < this._size; i++)
        {
            if (value < _items[i])
            {
                for (int j = this._size - 1; j >= i; j--)
                {
                    _items[j+1] = _items[j];
                }
                _items[i] = value;
                _size++;
                return true;
            }
        }

        _items[_size] = value;
        _size++;
        return true;
    }

    public void Clear()
    {
        for (int i = 0; i < _items.Length; i++)
        {
            _items[i] = 0;
        }
        _size = 0;
    }

    private int FindIndex(int value)
    {
        for (int i = 0; i < _size; i++)
        {
            if (_items[i] == value)
            {
                return i;
            }
            if (_items[i] > value)
            {
                return -1;
            }
        }
        return -1;
    }

    public bool Contains(int value)
    {
        return this.FindIndex(value) >= 0;
    }

    public void CopyTo(int[] array)
    {
        if (array.Length < _size)
        {
            throw new ArgumentException();
        }
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = _items[i];
        }
    }

    public int GetCount()
    {
        return _size;
    }

    public bool Remove(int value)
    {
        int index = this.FindIndex(value);
        if (index == -1)
        {
            return false;
        }

        for (int i = index; i < _size - 1; i++)
        {
            _items[i] = _items[i + 1];
        }
        _items[_size - 1] = 0;
        _size--;
        return true;
    }

    public bool SetEquals(ISetInt other)
    {
        if (this._size != other.GetCount())
        {
            return false;
        }
        if (this.GetCount() == 0 && other.GetCount() == 0)
        {
            return true;
        }
        for (int i = 0; i < this._size; i++)
        {
            if (other.Contains(this._items[i]))
            {
                continue;
            }
            return false;
        }
        return true;
    }

    public void UnionWith(ISetInt other)
    {
        int[] otherSet = new int[other.GetCount()];
        other.CopyTo(otherSet);
        foreach (int item in otherSet)
        {
            this.Add(item);
        }
    }
}
