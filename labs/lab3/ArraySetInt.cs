using System;

namespace lab3
{
    public class ArraySetInt : ISetInt
    {
        private int[] _items;
        private int _size;

        public ArraySetInt()
        {
            _items = new int[16];
            _size = 0;
        }
        
        public int GetCount
        {
            get
            {
                return _size;
            }
        }

        public bool Add(int value)
        {
            if(this.Contains(value))
            {
                return false;
            }

            if(_size == _items.Length)
            {
                Array.Resize(ref _items, _size * 2);
            }
            _items[_size] = value;
            _size++;
            // insertion sort
            int key = 0;
            int j = 0;
            for(int i = 0; i < _size; i++)
            {
                key = _items[i];
                j = i - 1;
                while(j >= 0 && _items[j] > key)
                {
                    _items[j+1] = _items[j];
                    j = j - 1;
                }
                _items[j+1] = key;
            }
            return true;
        }

        public void Clear()
        {
            for(int i = 0; i < _size; i++)
            {
                _items[i] = default;
            }
            _size = 0;
        }

        private int FindIndex(int value)
        {
            // binary search
            int mid = 0;
            int low = 0;
            int high = _size;
            while(low <= high)
            {
                mid = (low + high) / 2;
                if(_items[mid] == value)
                {
                    return mid;
                }
                else if(value < _items[mid])
                {
                    high = mid - 1;
                }
                else
                {
                    low = mid + 1;
                }
            }
            return -1;
        }

        public bool Contains(int value)
        {
            int index = this.FindIndex(value);
            return index >= 0;
        }

        public void CopyTo(int[] array)
        {
            if(array.Length < _size)
            {
                throw new ArgumentException("array length is not cool :(");
            }
            for(int i = 0; i < _size; i++)
            {
                array[i] = _items[i];
            }
        }

        public bool Remove(int value)
        {
            int index = this.FindIndex(value);
            if(index == -1)
            {
                return false;
            }
            for(int i = index; i < _size-1; i++)
            {
                _items[i] = _items[i + 1];
            }
            _items[_size] = default;
            this._size -= 1;
            return true;
        }

        public bool Overlaps(ISetInt other)
        {
            for(int o = 0; o < this.GetCount; o++)
            {
                if(other.Contains(this._items[o]))
                {
                    return true;
                }
            }
            return false;
        }

        public void SymmetricExceptWith(ISetInt other)
        {
            int[] otherArray = new int[other.GetCount];
            other.CopyTo(otherArray);
            for(int y = 0; y < otherArray.Length; y++)
            {
                if(this.Contains(otherArray[y]))
                {
                    this.Remove(otherArray[y]);
                }
                else
                {
                    this.Add(otherArray[y]);
                }
            }
        }
    }
}