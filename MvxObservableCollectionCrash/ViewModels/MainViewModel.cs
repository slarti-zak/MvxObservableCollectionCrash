using System;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace MvxObservableCollectionCrash.ViewModels
{
    public class MainViewModel : MvxViewModel
    {
        public MvxObservableCollection<ListItem> Items { get; } = new MvxObservableCollection<ListItem>();

        public MvxCommand StartCommand { get; }
        public MvxCommand StopCommand { get; }

        private readonly CancellationTokenSource _cancellationSource = new CancellationTokenSource();
        public CancellationToken Cancellation => _cancellationSource.Token;

        private long _counter = 0;
        private const int _elementCount = 50;
        private const int _updateCount = 6;

        private volatile bool _running = false;

        public MainViewModel()
        {
            StartCommand = new MvxCommand(Start);
            StopCommand = new MvxCommand(Stop);
        }

        public override async Task Initialize()
        {
            await base.Initialize().ConfigureAwait(false);

            for (int i = 0; i < _elementCount; i++)
            {
                Items.Add(CreateListItem());
            }

            Task.Run(ModifyItems, Cancellation);
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            if (viewFinishing)
            {
                _cancellationSource.Cancel();
            }
            base.ViewDestroy(viewFinishing);
        }

        private void Start() { _running = true; }
        private void Stop() { _running = false; }

        private async Task ModifyItems()
        {
            var random = new Random();

            while (!_cancellationSource.IsCancellationRequested)
            {
                if (_running)
                {
                    RemoveItems(random);
                    AddItems(random);
                }
                await Task.Delay(TimeSpan.FromMilliseconds(50)).ConfigureAwait(false);
            }
        }

        private void RemoveItems(Random random)
        {
            for (var i = 0; i < _updateCount; i++)
            {
                var whereToRemove = random.Next(Items.Count);
                Items.RemoveAt(whereToRemove);
            }
        }

        private void AddItems(Random random)
        {
            for (var i = 0; i < _updateCount; i++)
            {
                var whereToAdd = random.Next(Items.Count + 1);
                Items.Insert(whereToAdd, CreateListItem());
            }
        }

        private ListItem CreateListItem()
        {
            var count = Interlocked.Increment(ref _counter);
            return new ListItem(count.ToString());
        }
    }
}
