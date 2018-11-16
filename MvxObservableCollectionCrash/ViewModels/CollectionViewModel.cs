using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace MvxObservableCollectionCrash.ViewModels
{
    public abstract class CollectionViewModel : MvxViewModel
    {
        public abstract ICollection Items { get; }

        public MvxCommand StartCommand { get; }
        public MvxCommand StopCommand { get; }

        private readonly CancellationTokenSource _cancellationSource = new CancellationTokenSource();
        public CancellationToken Cancellation => _cancellationSource.Token;

        private long _counter = 0;
        private const int _elementCount = 50;
        private const int _updateCount = 6;

        private volatile bool _running = false;

        protected CollectionViewModel()
        {
            StartCommand = new MvxCommand(StartModifications);
            StopCommand = new MvxCommand(StopModifications);
        }

        public override async Task Initialize()
        {
            await base.Initialize().ConfigureAwait(false);

            InitializeCollection(_elementCount);

            Task.Run(ModifyItems, Cancellation);
        }

        protected abstract void InitializeCollection(int count);

        public override void ViewDestroy(bool viewFinishing = true)
        {
            if (viewFinishing)
            {
                _cancellationSource.Cancel();
            }
            base.ViewDestroy(viewFinishing);
        }

        private void StartModifications() { _running = true; }
        private void StopModifications() { _running = false; }

        private async Task ModifyItems()
        {
            var random = new Random();

            while (!Cancellation.IsCancellationRequested)
            {
                if (_running)
                {
                    RemoveItems(random, _updateCount);
                    AddItems(random, _updateCount);
                }
                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);
            }
        }

        protected abstract void RemoveItems(Random random, int count);

        protected abstract void AddItems(Random random, int count);

        protected ListItem CreateListItem()
        {
            var count = Interlocked.Increment(ref _counter);
            return new ListItem(count);
        }
    }
}
