using Android.App;
using Android.Content.PM;
using Android.OS;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvxObservableCollectionCrash.ViewModels;

namespace MvxObservableCollectionCrash.Droid
{
    [MvxActivityPresentation]
    [Activity(Label = "SelectionActivity",
              Icon = "@mipmap/icon",
              Theme = "@style/MainTheme",
              MainLauncher = true,
              ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class SelectionActivity : MvxAppCompatActivity<SelectionViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.view_selection);
        }
    }
}