using Mainapp.Constants;
using Mainapp.Controls;
using Mainapp.Miniapps.Ecommerce.Views;
using System.Windows.Input;

namespace Mainapp.ViewModels
{
    public class DashboardPageViewModel : BaseViewModel
    {
        public ICommand OpenChurchAppCommand { get; }
        public ICommand OpenWeatherAppCommand { get; }
        public ICommand OpenNewsAppCommand { get; }
        public ICommand OpenSportsAppCommand { get; }
        public ICommand OpenHealthAppCommand { get; }
        public ICommand OpenEducationAppCommand { get; }
        public ICommand OpenEntertainmentAppCommand { get; }
        public ICommand OpenTravelAppCommand { get; }
        public ICommand OpenShoppingAppCommand { get; }

        public DashboardPageViewModel()
        {
            OpenChurchAppCommand = new Command(async () => await OpenChurchAppAsync());
            OpenWeatherAppCommand = new Command(async () => await OpenWeatherAppAsync());
            OpenNewsAppCommand = new Command(async () => await OpenNewsAppAsync());
            OpenSportsAppCommand = new Command(async () => await OpenSportsAppAsync());
            OpenHealthAppCommand = new Command(async () => await OpenHealthAppAsync());
            OpenEducationAppCommand = new Command(async () => await OpenEducationAppAsync());
            OpenEntertainmentAppCommand = new Command(async () => await OpenEntertainmentAppAsync());
            OpenTravelAppCommand = new Command(async () => await OpenTravelAppAsync());
            OpenShoppingAppCommand = new Command(async () => await OpenShoppingAppAsync());

            // Set Flyout Header
            AppShell.Current.FlyoutHeader = new FlyoutHeaderControl();
        }

        private async Task OpenChurchAppAsync() => await Task.CompletedTask;
        private async Task OpenWeatherAppAsync() => await Task.CompletedTask;
        private async Task OpenNewsAppAsync() => await Task.CompletedTask;
        private async Task OpenSportsAppAsync() => await Task.CompletedTask;
        private async Task OpenHealthAppAsync() => await Task.CompletedTask;
        private async Task OpenEducationAppAsync() => await Task.CompletedTask;
        private async Task OpenEntertainmentAppAsync() => await Task.CompletedTask;
        private async Task OpenTravelAppAsync() => await Task.CompletedTask;

        private async Task OpenShoppingAppAsync()
        {
            var route = nameof(SokojijiDashboardPage);
            await AppConstant.AddFlyoutMenusDetails(route);

            // Use absolute navigation to ensure smooth transition
            await Shell.Current.GoToAsync($"///{route}");
        }
    }
}
