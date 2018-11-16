using System;
using System.Collections;
using System.Threading.Tasks;
using MvvmCross.Base;
using MvxObservableCollectionCrash.Collections;

namespace MvxObservableCollectionCrash.ViewModels
{
    public class UiObservableCollectionViewModel : CollectionViewModel
    {
        private readonly BackingCollection<ListItem> _backingCollection = new BackingCollection<ListItem>();
        private readonly UiObservableCollection<ListItem> _uiCollection;
        public override ICollection Items => _uiCollection;

        private IDisposable _uiSubscription;

        public UiObservableCollectionViewModel(IMvxMainThreadAsyncDispatcher dispatcher)
        {
            _uiCollection = new UiObservableCollection<ListItem>(dispatcher);
        }

        public override async Task Initialize()
        {
            await base.Initialize().ConfigureAwait(false);
            _uiSubscription = await _uiCollection.Connect(_backingCollection).ConfigureAwait(false);
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
                _uiSubscription?.Dispose();
            }
            base.ViewDestroy(viewFinishing);
        }

        protected override void RemoveItems(Random random, int count)
        {
            for (var i = 0; i < count; i++)
            {
                _backingCollection.LockedAction(content =>
                {
                    var whereToRemove = random.Next(content.Count);
                    content.RemoveAt(whereToRemove);
                });
            }
        }

        protected override void AddItems(Random random, int count)
        {
            for (var i = 0; i < count; i++)
            {
                _backingCollection.LockedAction(content =>
                {
                    var whereToAdd = random.Next(content.Count + 1);
                    content.Insert(whereToAdd, CreateListItem());
                });
            }
        }
    }
}
