using System.Collections.Generic;
using System.Reflection;
using MvvmCross;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Views;
using MvxObservableCollectionCrash.ViewModels;

namespace MvxObservableCollectionCrash.Droid
{
    public class DroidSetup : MvxAppCompatSetup<App>
    {
        protected override IEnumerable<Assembly> AndroidViewAssemblies =>
            new List<Assembly>(base.AndroidViewAssemblies)
            {
                typeof(MvxRecyclerView).Assembly
            };

        protected override void InitializeLastChance()
        {
            base.InitializeLastChance();

            var viewsContainer = Mvx.IoCProvider.Resolve<IMvxViewsContainer>();
            viewsContainer.Add<MvxObservableCollectionViewModel, CollectionActivity>();
            viewsContainer.Add<UiObservableCollectionViewModel, CollectionActivity>();
        }
    }
}
