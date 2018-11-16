using MvvmCross;
using MvvmCross.Platforms.Ios.Core;
using MvvmCross.Views;
using MvxObservableCollectionCrash.ViewModels;

namespace MvxObservableCollectionCrash.iOS
{
    public class IOSSetup : MvxIosSetup<App>
    {
        protected override void InitializeLastChance()
        {
            base.InitializeLastChance();

            var viewsContainer = Mvx.IoCProvider.Resolve<IMvxViewsContainer>();
            viewsContainer.Add<MvxObservableCollectionViewModel, CollectionViewController>();
            viewsContainer.Add<UiObservableCollectionViewModel, CollectionViewController>();
        }
    }
}
