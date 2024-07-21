using CommunityToolkit.Mvvm.ComponentModel;
using Mainapp.Controls;

namespace Mainapp.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private string _title;

        public BaseViewModel()
        {
            // Initialize common properties or methods if needed
        }

        protected async Task NavigateToAsync(string route)
        {
            // Ensure route is correctly formatted and valid
            await Shell.Current.GoToAsync(route);
        }

        protected void SetFlyoutHeader()
        {
            // Ensure FlyoutHeaderControl is accessible and correctly implemented
            AppShell.Current.FlyoutHeader = new FlyoutHeaderControl();
        }
    }
}
