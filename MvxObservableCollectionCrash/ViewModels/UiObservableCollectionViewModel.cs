using System;
using System.Collections;
using MvvmCross.Base;
using MvxObservableCollectionCrash.Collections;

namespace MvxObservableCollectionCrash.ViewModels
{
    public class UiObservableCollectionViewModel : CollectionViewModel
    {
        private readonly BackingCollection<ListItem> _backingCollection = new BackingCollection<ListItem>();
        private readonly UiObservableCollection<ListItem> _uiCollection;
        public override ICollection Items => _uiCollection;

        public UiObservableCollectionViewModel(IMvxMainThreadAsyncDispatcher dispatcher)
        {
            _uiCollection = new UiObservableCollection<ListItem>(dispatcher);
            _uiCollection.Connect(_backingCollection);
        }

        protected override void InitializeCollection(int count)
        {
            for (int i = 0; i < count; i++)
            {
                _backingCollection.Add(CreateListItem());
            }
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            if (viewFinishing)
            {
                _uiCollection.Dispose();
            }
            base.ViewDestroy(viewFinishing);
        }

        protected override void RemoveItems(Random random, int count)
        {
            for (var i = 0; i < count; i++)
            {
                _backingCollection.LockedAction(() =>
                {
                    var whereToRemove = random.Next(_backingCollection.Count);
                    _backingCollection.RemoveAt(whereToRemove);
                });
            }
        }

        protected override void AddItems(Random random, int count)
        {
            for (var i = 0; i < count; i++)
            {
                _backingCollection.LockedAction(() =>
                {
                    var whereToAdd = random.Next(_backingCollection.Count + 1);
                    _backingCollection.Insert(whereToAdd, CreateListItem());
                });
            }
        }
    }
}
