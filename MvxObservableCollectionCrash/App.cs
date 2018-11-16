using MvvmCross.ViewModels;
using MvxObservableCollectionCrash.ViewModels;

namespace MvxObservableCollectionCrash
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            base.Initialize();

            RegisterAppStart<SelectionViewModel>();
        }
    }
}
