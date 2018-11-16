using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace MvxObservableCollectionCrash.Collections
{
    /// <summary>
    /// A collection that implements <see cref="INotifyPropertyChanging"/> in a thread safe manner.
    /// This base class can only be accessed in a read only manner. Every method call is locked.
    /// Subclasses should be careful not to break the thread safety.
    /// </summary>
    public abstract class InternalObservableCollection<T> : INotifyCollectionChanged, INotifyPropertyChanged
    {
        protected readonly BatchObservableCollection<T> _items;
        protected readonly object _lock = new object();

        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add
            {
                lock (_lock)
                {
                    _items.CollectionChanged += value;
                }
            }
            remove
            {
                lock (_lock)
                {
                    _items.CollectionChanged -= value;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                lock (_lock)
                {
                    ((INotifyPropertyChanged)_items).PropertyChanged += value;
                }
            }
            remove
            {
                lock (_lock)
                {
                    ((INotifyPropertyChanged)_items).PropertyChanged -= value;
                }
            }
        }

        protected InternalObservableCollection()
        {
            _items = new BatchObservableCollection<T>();
        }

        protected InternalObservableCollection(IEnumerable<T> initialData)
        {
            _items = new BatchObservableCollection<T>(initialData);
        }

        public void LockedAction(Action<BatchObservableCollection<T>> action)
        {
            lock (_lock)
            {
                action(_items);
            }
        }

        public List<T> Copy()
        {
            lock (_lock)
            {
                return new List<T>(_items);
            }
        }

        public bool Contains(T item)
        {
            lock (_lock)
            {
                return _items.Contains(item);
            }
        }

        public int Count
        {
            get
            {
                lock (_lock)
                {
                    return _items.Count;
                }
            }
        }
    }
}
