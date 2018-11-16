using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Base;
using MvvmCross.ViewModels;
using MvvmCross.WeakSubscription;

namespace MvxObservableCollectionCrash.Collections
{
    public class UiObservableCollection<T> : IObservableCollection<T>
    {
        private readonly BatchObservableCollection<T> _items = new BatchObservableCollection<T>();

        private readonly object _lock = new object();
        private readonly Queue<NotifyCollectionChangedEventArgs> _changes = new Queue<NotifyCollectionChangedEventArgs>();
        private readonly IMvxMainThreadAsyncDispatcher _dispatcher;

        public UiObservableCollection(IMvxMainThreadAsyncDispatcher dispatcher)
        {
            _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
        }

        public async Task<IDisposable> Connect(InternalObservableCollection<T> collection)
        {
            MvxNotifyCollectionChangedEventSubscription subscription = null;
            await _dispatcher.ExecuteOnMainThreadAsync(() =>
            {
                lock (_lock)
                {
                    collection.LockedAction(content =>
                    {
                        _items.ReplaceWith(content);
                        subscription = content.WeakSubscribe(UiCollectionChanged);
                    });
                }
            }).ConfigureAwait(false);
            return subscription;
        }

        private void UiCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            lock (_lock)
            {
                _changes.Enqueue(e);
            }
            _dispatcher.ExecuteOnMainThreadAsync(() => ExecuteChanges());
        }

        private void ExecuteChanges()
        {
            lock (_lock)
            {
                while (_changes.Count > 0)
                {
                    var changeEvent = _changes.Dequeue();
                    HandleEvent(changeEvent);
                }
            }
        }

        private void HandleEvent(NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    var newItems = e.NewItems.Cast<T>().ToList();
                    for (var i = 0; i < newItems.Count; i++)
                    {
                        _items.Insert(e.NewStartingIndex + i, newItems[i]);
                    }
                    break;

                case NotifyCollectionChangedAction.Move:
                    _items.Move(e.OldStartingIndex, e.NewStartingIndex);
                    break;

                case NotifyCollectionChangedAction.Remove:
                    for (int i = e.OldStartingIndex + e.OldItems.Count; i-- > e.OldStartingIndex;)
                    {
                        _items.RemoveAt(i);
                    }
                    break;

                case NotifyCollectionChangedAction.Replace:
                    var replacingViewModels = e.NewItems.Cast<T>();
                    _items.ReplaceRange(replacingViewModels, e.OldStartingIndex, e.OldItems.Count);
                    break;

                case NotifyCollectionChangedAction.Reset:
                    _items.Clear();
                    break;
            }
        }

        private void EnsureUiThread()
        {
            if (!_dispatcher.IsOnMainThread)
            {
                throw new InvalidOperationException("Can only be called on ui thread!");
            }
        }

        #region Delegates

        public void AddRange(IEnumerable<T> items)
        {
            EnsureUiThread();
            _items.AddRange(items);
        }

        public void RemoveItems(IEnumerable<T> items)
        {
            EnsureUiThread();
            _items.RemoveItems(items);
        }

        public void RemoveRange(int start, int count)
        {
            EnsureUiThread();
            _items.RemoveRange(start, count);
        }

        public void ReplaceRange(IEnumerable<T> items, int firstIndex, int oldSize)
        {
            EnsureUiThread();
            _items.ReplaceRange(items, firstIndex, oldSize);
        }

        public void ReplaceWith(IEnumerable<T> items)
        {
            EnsureUiThread();
            _items.ReplaceWith(items);
        }

        public void SwitchTo(IEnumerable<T> items)
        {
            EnsureUiThread();
            _items.SwitchTo(items);
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add => _items.CollectionChanged += value;
            remove => _items.CollectionChanged -= value;
        }

        public event PropertyChangedEventHandler PropertyChanged
        {
            add => ((INotifyPropertyChanged)_items).PropertyChanged += value;
            remove => ((INotifyPropertyChanged)_items).PropertyChanged -= value;
        }

        int IList.Add(object item)
        {
            EnsureUiThread();
            return ((IList)_items).Add(item);
        }

        void IList.Clear()
        {
            EnsureUiThread();
            _items.Clear();
        }

        bool IList.Contains(object item)
        {
            EnsureUiThread();
            return ((IList)_items).Contains(item);
        }

        int IList.IndexOf(object item)
        {
            EnsureUiThread();
            return ((IList)_items).IndexOf(item);
        }

        void IList.Insert(int index, object item)
        {
            EnsureUiThread();
            ((IList)_items).Insert(index, item);
        }

        void IList.Remove(object item)
        {
            EnsureUiThread();
            ((IList)_items).Remove(item);
        }

        void IList.RemoveAt(int index)
        {
            EnsureUiThread();
            _items.RemoveAt(index);
        }

        bool IList.IsFixedSize => false;

        bool IList.IsReadOnly => false;

        object IList.this[int index]
        {
            get
            {
                EnsureUiThread();
                return _items[index];
            }
            set
            {
                EnsureUiThread();
                ((IList)_items)[index] = value;
            }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            EnsureUiThread();
            ((ICollection)_items).CopyTo(array, index);
        }

        int ICollection.Count
        {
            get
            {
                EnsureUiThread();
                return _items.Count;
            }
        }

        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot => null;

        public int IndexOf(T item)
        {
            EnsureUiThread();
            return _items.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            EnsureUiThread(); _items.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            EnsureUiThread();
            _items.RemoveAt(index);
        }

        public T this[int index]
        {
            get
            {
                EnsureUiThread();
                return _items[index];
            }
            set
            {
                EnsureUiThread();
                _items[index] = value;
            }

        }

        public void Add(T item)
        {
            EnsureUiThread();
            _items.Add(item);
        }

        public void Clear()
        {
            EnsureUiThread();
            _items.Clear();
        }

        public bool Contains(T item)
        {
            EnsureUiThread();
            return _items.Contains(item);
        }

        public void CopyTo(T[] array, int index)
        {
            EnsureUiThread(); _items.CopyTo(array, index);
        }

        public bool Remove(T item)
        {
            EnsureUiThread(); return _items.Remove(item);
        }

        public int Count
        {
            get
            {

                EnsureUiThread();
                return _items.Count;
            }
        }

        public bool IsReadOnly => false;

        public IEnumerator<T> GetEnumerator()
        {
            EnsureUiThread();
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            EnsureUiThread();
            return _items.GetEnumerator();
        }

        #endregion
    }
}
