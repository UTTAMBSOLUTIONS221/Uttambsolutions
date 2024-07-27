using Mainapp.Constants;
using Mainapp.Controls;
using Mainapp.Miniapps.Ecommerce.Views;
using System.Windows.Input;

namespace Mainapp.ViewModels
{
    public partial class DashboardPageViewModel : BaseViewModel
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

            AppShell.Current.FlyoutHeader = new FlyoutHeaderControl();
        }

        private async Task OpenChurchAppAsync()
        {
            // Implement navigation or logic to open the Church App
            await Task.CompletedTask;
        }

        private async Task OpenWeatherAppAsync()
        {
            // Implement navigation or logic to open the Weather App
            await Task.CompletedTask;
        }

        private async Task OpenNewsAppAsync()
        {
            // Implement navigation or logic to open the News App
            await Task.CompletedTask;
        }

        private async Task OpenSportsAppAsync()
        {
            // Implement navigation or logic to open the Sports App
            await Task.CompletedTask;
        }

        private async Task OpenHealthAppAsync()
        {
            // Implement navigation or logic to open the Health App
            await Task.CompletedTask;
        }

        private async Task OpenEducationAppAsync()
        {
            // Implement navigation or logic to open the Education App
            await Task.CompletedTask;
        }

        private async Task OpenEntertainmentAppAsync()
        {
            // Implement navigation or logic to open the Entertainment App
            await Task.CompletedTask;
        }

        private async Task OpenTravelAppAsync()
        {
            // Implement navigation or logic to open the Travel App
            await Task.CompletedTask;
        }

        private async Task OpenShoppingAppAsync()
        {
            await AppConstant.AddFlyoutMenusDetails();
            await Shell.Current.GoToAsync(nameof(SokojijiDashboardPage));
        }
    }
}
