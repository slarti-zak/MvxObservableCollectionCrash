using Android.App;
using Android.Content.PM;
using Android.OS;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvxObservableCollectionCrash.ViewModels;

namespace MvxObservableCollectionCrash.Droid
{
    [MvxActivityPresentation]
    [Activity(Label = "CollectionActivity",
              Icon = "@mipmap/icon",
              Theme = "@style/MainTheme",
              ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class CollectionActivity : MvxAppCompatActivity<CollectionViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.view_collection);

            var recyclerView = FindViewById<MvxRecyclerView>(Resource.Id.recyclerview);
            recyclerView.ItemsSource = ViewModel?.Items;
        }
    }
}