using System;
using Android.App;
using Android.Runtime;
using MvvmCross.Droid.Support.V7.AppCompat;

namespace MvxObservableCollectionCrash.Droid
{
    [Application]
    public class MainApplication : MvxAppCompatApplication<DroidSetup, App>
    {
        public MainApplication(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }
    }
}
