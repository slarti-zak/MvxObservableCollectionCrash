using System;
using System.Collections.Generic;

namespace MvxObservableCollectionCrash.Collections
{
    public class BackingCollection<T> : InternalObservableCollection<T>
    {
        public BackingCollection() : base()
        {
        }

        public BackingCollection(IEnumerable<T> initialData) : base(initialData)
        {
        }

        public void Add(T item)
        {
            lock (_lock)
            {
                _items.Add(item);
            }
        }

        public void Insert(int index, T item)
        {
            lock (_lock)
            {
                _items.Insert(index, item);
            }
        }

        public bool Remove(T item)
        {
            lock (_lock)
            {
                return _items.Remove(item);
            }
        }

        public void RemoveAt(int index)
        {
            lock (_lock)
            {
                _items.RemoveAt(index);
            }
        }

        public void Synchronize(IEnumerable<T> target, IEqualityComparer<T> comparer = null, Action<T> addCallback = null)
        {
            comparer = comparer ?? EqualityComparer<T>.Default;
            lock (_lock)
            {
                var itemIndex = 0;
                var count = _items.Count;

                foreach (var item in target)
                {
                    if (itemIndex >= count)
                    {
                        addCallback?.Invoke(item);
                        _items.Add(item);
                    }
                    else if (!comparer.Equals(_items[itemIndex], item))
                    {
                        addCallback?.Invoke(item);
                        _items[itemIndex] = item;
                    }

                    itemIndex++;
                }

                while (count > itemIndex)
                {
                    _items.RemoveAt(--count);
                }
            }
        }
    }
}
