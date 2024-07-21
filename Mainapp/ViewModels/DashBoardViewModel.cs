using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mainapp.Miniapps.Apps.Church;

namespace Mainapp.ViewModels
{
    public partial class DashBoardViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string userName;

        [ObservableProperty]
        private string memberNumber;

        [RelayCommand]
        private async Task OpenChurchAppCommand()
        {
            await Shell.Current.GoToAsync(nameof(ChurchDashBoardPage));
        }

        [RelayCommand]
        private async Task OpenWeatherAppCommand()
        {
            await Shell.Current.GoToAsync(nameof(ChurchDashBoardPage));
        }

        [RelayCommand]
        private async Task OpenNewsAppCommand()
        {
            await Shell.Current.GoToAsync(nameof(ChurchDashBoardPage));
        }

        [RelayCommand]
        private async Task OpenFinanceAppCommand()
        {
            await Shell.Current.GoToAsync(nameof(ChurchDashBoardPage));
        }

        [RelayCommand]
        private async Task OpenSportsAppCommand()
        {
            await Shell.Current.GoToAsync(nameof(ChurchDashBoardPage));
        }

        [RelayCommand]
        private async Task OpenHealthAppCommand()
        {
            await Shell.Current.GoToAsync(nameof(ChurchDashBoardPage));
        }

        [RelayCommand]
        private async Task OpenEducationAppCommand()
        {
            await Shell.Current.GoToAsync(nameof(ChurchDashBoardPage));
        }

        [RelayCommand]
        private async Task OpenEntertainmentAppCommand()
        {
            await Shell.Current.GoToAsync(nameof(ChurchDashBoardPage));
        }

        [RelayCommand]
        private async Task OpenTravelAppCommand()
        {
            await Shell.Current.GoToAsync(nameof(ChurchDashBoardPage));
        }

        [RelayCommand]
        private async Task OpenShoppingAppCommand()
        {
            await Shell.Current.GoToAsync(nameof(ChurchDashBoardPage));
        }

        // Constructor
        public DashBoardViewModel()
        {
            UserName = App.UserDetails.Fullname;
            MemberNumber = "23456";
        }
    }
}