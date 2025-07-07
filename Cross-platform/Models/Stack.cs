using System;
using System.Collections.Generic;

namespace TeachingAidMac.Models
{
    public class Stack<T>
    {
        private List<T> _items;
        private int _pointer;

        public Stack()
        {
            _items = new List<T>();
            _pointer = -1;
        }

        public void Push(T item)
        {
            _items.Add(item);
            _pointer++;
        }

        public T? Pop()
        {
            if (IsEmpty())
            {
                return default(T);
            }
            else
            {
                T poppedItem = _items[_pointer];
                _items.RemoveAt(_pointer);
                _pointer--;
                return poppedItem;
            }
        }

        public bool IsEmpty()
        {
            return _pointer == -1;
        }

        public int Count => _pointer + 1;
    }
}
