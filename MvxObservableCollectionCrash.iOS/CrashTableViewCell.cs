using System;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using MvxObservableCollectionCrash.ViewModels;
using UIKit;

namespace MvxObservableCollectionCrash.iOS
{
    public partial class CrashTableViewCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("CrashTableViewCell");
        public static readonly UINib Nib;

        static CrashTableViewCell()
        {
            Nib = UINib.FromName("CrashTableViewCell", NSBundle.MainBundle);
        }

        protected CrashTableViewCell(IntPtr handle) : base(handle)
        {
            this.DelayBind(CreateBindings);
        }

        private void CreateBindings()
        {
            var set = this.CreateBindingSet<CrashTableViewCell, ListItem>();
            set.Bind(CrashLabel)
               .To(vm => vm.Title);
            set.Apply();
        }
    }
}
