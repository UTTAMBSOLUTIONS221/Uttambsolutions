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
        private void EditProfile()
        {
            // Implement the edit profile logic
        }

        // Commands for opening mini-apps
        [RelayCommand]
        private async Task OpenChurchAppCommand()
        {
            await Shell.Current.GoToAsync(nameof(ChurchDashBoardPage));
        }

        [RelayCommand]
        private void OpenWeatherApp()
        {
            // Implement navigation to Weather App
        }

        [RelayCommand]
        private void OpenNewsApp()
        {
            // Implement navigation to News App
        }

        [RelayCommand]
        private void OpenFinanceApp()
        {
            // Implement navigation to Finance App
        }

        [RelayCommand]
        private void OpenSportsApp()
        {
            // Implement navigation to Sports App
        }

        [RelayCommand]
        private void OpenHealthApp()
        {
            // Implement navigation to Health App
        }

        [RelayCommand]
        private void OpenEducationApp()
        {
            // Implement navigation to Education App
        }

        [RelayCommand]
        private void OpenEntertainmentApp()
        {
            // Implement navigation to Entertainment App
        }

        [RelayCommand]
        private void OpenTravelApp()
        {
            // Implement navigation to Travel App
        }

        [RelayCommand]
        private void OpenShoppingApp()
        {
            // Implement navigation to Shopping App
        }

        // Constructor
        public DashBoardViewModel()
        {
            // Initialize properties, e.g.,
            UserName = App.UserDetails.Fullname;
            MemberNumber = "23456";
        }
    }
}