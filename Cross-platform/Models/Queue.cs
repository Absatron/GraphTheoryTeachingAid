using System;
using System.Collections.Generic;

namespace TeachingAidMac.Models
{
    public class Queue<T>
    {
        private List<T> _items;
        private int _endPointer;

        public Queue()
        {
            _items = new List<T>();
            _endPointer = -1;
        }

        public void Push(T item)
        {
            _items.Add(item);
            _endPointer++;
        }

        public T? Pop()
        {
            if (IsEmpty())
            {
                return default(T);
            }
            else
            {
                T poppedItem = _items[0];
                ShuffleForwards();
                return poppedItem;
            }
        }

        public bool IsEmpty()
        {
            return _endPointer == -1;
        }

        private void ShuffleForwards()
        {
            for (int i = 0; i < _endPointer; i++)
            {
                _items[i] = _items[i + 1];
            }
            _items.RemoveAt(_endPointer);
            _endPointer--;
        }

        public int Count => _endPointer + 1;
    }
}
