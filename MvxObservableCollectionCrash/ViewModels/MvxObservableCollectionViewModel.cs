using System;
using System.Collections;
using System.Collections.Generic;
using MvvmCross.Base;
using MvvmCross.ViewModels;

namespace MvxObservableCollectionCrash.ViewModels
{
    public class MvxObservableCollectionViewModel : CollectionViewModel
    {
        private readonly MvxObservableCollection<ListItem> _backingCollection = new MvxObservableCollection<ListItem>();
        public override ICollection Items => _backingCollection;

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
                _backingCollection.DisposeIfDisposable();
            }
            base.ViewDestroy(viewFinishing);
        }

        protected override void RemoveItems(Random random, int count)
        {
            for (var i = 0; i < count; i++)
            {
                var whereToRemove = random.Next(_backingCollection.Count);
                _backingCollection.RemoveAt(whereToRemove);
            }
        }

        protected override void AddItems(Random random, int count)
        {
            for (var i = 0; i < count; i++)
            {
                var whereToAdd = random.Next(_backingCollection.Count + 1);
                _backingCollection.Insert(whereToAdd, CreateListItem());
            }
        }
    }
}
