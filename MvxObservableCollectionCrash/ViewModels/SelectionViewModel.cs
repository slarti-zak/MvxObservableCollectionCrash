using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace MvxObservableCollectionCrash.ViewModels
{
    public class SelectionViewModel : MvxNavigationViewModel
    {
        public ICommand OpenCrashCommand { get; }
        public ICommand OpenWorkingCommand { get; }

        public SelectionViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            OpenCrashCommand = new MvxAsyncCommand(NavigateToCrash);
            OpenWorkingCommand = new MvxAsyncCommand(NavigateToWorking);
        }

        private Task NavigateToCrash() => NavigationService.Navigate<MvxObservableCollectionViewModel>();
        private Task NavigateToWorking() => NavigationService.Navigate<UiObservableCollectionViewModel>();
    }
}
