using System;

namespace MvxObservableCollectionCrash.ViewModels
{
    public class ListItem : IComparable<ListItem>
    {
        public ListItem(long value)
        {
            Value = value;
        }

        public long Value { get; }

        public int CompareTo(ListItem other) => Value.CompareTo(other.Value);
    }
}
