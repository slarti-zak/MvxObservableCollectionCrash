// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace MvxObservableCollectionCrash.iOS
{
	[Register ("SelectionViewController")]
	partial class SelectionViewController
	{
		[Outlet]
		UIKit.UIButton MvxObservableCollectionButton { get; set; }

		[Outlet]
		UIKit.UIButton UiObservableCollectionButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (MvxObservableCollectionButton != null) {
				MvxObservableCollectionButton.Dispose ();
				MvxObservableCollectionButton = null;
			}

			if (UiObservableCollectionButton != null) {
				UiObservableCollectionButton.Dispose ();
				UiObservableCollectionButton = null;
			}
		}
	}
}
