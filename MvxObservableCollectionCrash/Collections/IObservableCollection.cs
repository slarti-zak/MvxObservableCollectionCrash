using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace MvxObservableCollectionCrash.Collections
{
    public interface IObservableCollection<T> : INotifyCollectionChanged, INotifyPropertyChanged, IList, IList<T>
    {
        void AddRange(IEnumerable<T> items);
        void RemoveItems(IEnumerable<T> items);
        void RemoveRange(int start, int count);
        void ReplaceRange(IEnumerable<T> items, int firstIndex, int oldSize);
        void ReplaceWith(IEnumerable<T> items);
        void SwitchTo(IEnumerable<T> items);
    }
}
