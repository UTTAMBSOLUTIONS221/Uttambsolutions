using CommunityToolkit.Mvvm.ComponentModel;
using Mainapp.Miniapps.News.Pages;
using Mainapp.Miniapps.Weather.Pages;
using Mainapp.Pages.Users;
using System.Windows.Input;

namespace Mainapp.ViewModels
{
    public class DashBoardViewModel : ObservableObject
    {
        private readonly INavigation _navigation;

        public ICommand OpenWeatherAppCommand => new Command(async () => await OpenWeatherAppAsync());
        public ICommand OpenNewsAppCommand => new Command(async () => await OpenNewsAppAsync());
        public ICommand LogoutCommand => new Command(async () => await LogoutAsync());

        public DashBoardViewModel(INavigation navigation)
        {
            _navigation = navigation;
        }

        private async Task OpenWeatherAppAsync()
        {
            await _navigation.PushAsync(new WeatherPage());
        }

        private async Task OpenNewsAppAsync()
        {
            await _navigation.PushAsync(new NewsPage());
        }
        private async Task LogoutAsync()
        {
            if (Preferences.ContainsKey(nameof(App.UserDetails)))
            {
                Preferences.Remove(nameof(App.UserDetails));
            }
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}