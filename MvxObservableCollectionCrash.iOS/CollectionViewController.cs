using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using MvxObservableCollectionCrash.ViewModels;

namespace MvxObservableCollectionCrash.iOS
{
    [MvxFromStoryboard("CollectionViewController")]
    [MvxChildPresentation]
    public partial class CollectionViewController : MvxViewController<CollectionViewModel>
    {
        protected CollectionViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var tableSource = new MvxStandardTableViewSource(Table, CrashTableViewCell.Key);
            Table.Source = tableSource;
            Table.RegisterNibForCellReuse(CrashTableViewCell.Nib, CrashTableViewCell.Key);

            var set = this.CreateBindingSet<CollectionViewController, CollectionViewModel>();
            set.Bind(StartButton)
               .To(vm => vm.StartCommand);
            set.Bind(StopButton)
               .To(vm => vm.StopCommand);
            set.Bind(tableSource)
               .For(t => t.ItemsSource)
               .To(vm => vm.Items);
            set.Apply();
        }
    }
}
