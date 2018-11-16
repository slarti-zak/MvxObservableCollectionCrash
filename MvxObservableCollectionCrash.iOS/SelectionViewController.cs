using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Views;
using MvxObservableCollectionCrash.ViewModels;
using MvvmCross.Platforms.Ios.Presenters.Attributes;

namespace MvxObservableCollectionCrash.iOS
{
    [MvxFromStoryboard("SelectionViewController")]
    [MvxRootPresentation(WrapInNavigationController = true)]
    public partial class SelectionViewController : MvxViewController<SelectionViewModel>
    {
        public SelectionViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var set = this.CreateBindingSet<SelectionViewController, SelectionViewModel>();

            set.Bind(MvxObservableCollectionButton)
               .To(vm => vm.OpenCrashCommand);

            set.Bind(UiObservableCollectionButton)
               .To(vm => vm.OpenWorkingCommand);

            set.Apply();
        }
    }
}
