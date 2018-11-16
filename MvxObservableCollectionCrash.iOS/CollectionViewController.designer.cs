// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;

namespace MvxObservableCollectionCrash.iOS
{
    [Register("CollectionViewController")]
    partial class CollectionViewController
    {
        [Outlet]
        UIKit.UIButton StartButton { get; set; }

        [Outlet]
        UIKit.UIButton StopButton { get; set; }

        [Outlet]
        UIKit.UITableView Table { get; set; }

        void ReleaseDesignerOutlets()
        {
            if (StartButton != null)
            {
                StartButton.Dispose();
                StartButton = null;
            }

            if (StopButton != null)
            {
                StopButton.Dispose();
                StopButton = null;
            }

            if (Table != null)
            {
                Table.Dispose();
                Table = null;
            }
        }
    }
}
