// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;

namespace MvxObservableCollectionCrash.iOS
{
    [Register("CrashTableViewCell")]
    partial class CrashTableViewCell
    {
        [Outlet]
        UIKit.UILabel CrashLabel { get; set; }

        void ReleaseDesignerOutlets()
        {
            if (CrashLabel != null)
            {
                CrashLabel.Dispose();
                CrashLabel = null;
            }
        }
    }
}
