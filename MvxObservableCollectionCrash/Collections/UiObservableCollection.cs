using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using MvvmCross.Base;
using MvvmCross.ViewModels;
using MvvmCross.WeakSubscription;

namespace MvxObservableCollectionCrash.Collections
{
    public class UiObservableCollection<T> : ObservableCollection<T>, IDisposable
    {
        private readonly object _lock = new object();
        private readonly Queue<NotifyCollectionChangedEventArgs> _changes = new Queue<NotifyCollectionChangedEventArgs>();
        private readonly IMvxMainThreadAsyncDispatcher _dispatcher;

        private MvxNotifyCollectionChangedEventSubscription _subscription;
        private WeakReference<InternalObservableCollection<T>> _collection;

        public UiObservableCollection(IMvxMainThreadAsyncDispatcher dispatcher)
        {
            _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
        }

        public void Connect(InternalObservableCollection<T> collection)
        {
            lock (_lock)
            {
                if (_collection != null)
                {
                    throw new InvalidOperationException("Collection already connected to another collection!");
                }

                _collection = new WeakReference<InternalObservableCollection<T>>(collection);
                _dispatcher.ExecuteOnMainThreadAsync(() =>
                {
                    collection.LockedAction(() =>
                    {
                        AddRange(collection.Copy());
                        _subscription = collection.WeakSubscribe(UiCollectionChanged);
                    });
                });
            }
        }

        private void UiCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            lock (_lock)
            {
                _changes.Enqueue(e);
            }
            _dispatcher.ExecuteOnMainThreadAsync(() => ExecuteChanges());
        }

        private void AddRange(IEnumerable<T> range)
        {
            foreach (var item in range)
            {
                Add(item);
            }
        }

        private void ReplaceRange(IEnumerable<T> items, int firstIndex, int oldSize)
        {
            var lastIndex = firstIndex + oldSize - 1;
            var itemsCount = items.Count();

            // If there are more items in the previous list, remove them.
            while (firstIndex + itemsCount <= lastIndex)
            {
                RemoveAt(lastIndex--);
            }

            foreach (var item in items)
            {
                if (firstIndex <= lastIndex)
                {
                    this[firstIndex++] = item;
                }
                else
                {
                    Insert(firstIndex++, item);
                }
            }
        }

        private void ExecuteChanges()
        {
            lock (_lock)
            {
                while (_changes.Count > 0)
                {
                    var e = _changes.Dequeue();
                    HandleEvent(e);
                }
            }
        }

        private void HandleEvent(NotifyCollectionChangedEventArgs e)
        {
            try
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        var newItems = e.NewItems.Cast<T>().ToList();
                        for (var i = 0; i < newItems.Count; i++)
                        {
                            Insert(e.NewStartingIndex + i, newItems[i]);
                        }
                        break;

                    case NotifyCollectionChangedAction.Move:
                        Move(e.OldStartingIndex, e.NewStartingIndex);
                        break;

                    case NotifyCollectionChangedAction.Remove:
                        for (int i = e.OldStartingIndex + e.OldItems.Count; i-- > e.OldStartingIndex;)
                        {
                            RemoveAt(i);
                        }
                        break;

                    case NotifyCollectionChangedAction.Replace:
                        var replacingViewModels = e.NewItems.Cast<T>();
                        ReplaceRange(replacingViewModels, e.OldStartingIndex, e.OldItems.Count);
                        break;

                    case NotifyCollectionChangedAction.Reset:
                        Clear();
                        break;
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                Clear();
                if (_collection.TryGetTarget(out var collection))
                {
                    AddRange(collection.Copy());
                }
            }
        }

        public void Dispose()
        {
            if (_subscription == null)
            {
                throw new ObjectDisposedException(nameof(UiObservableCollection<T>));
            }
            _subscription.Dispose();
            _subscription = null;
            _collection = null;
        }
    }
}
