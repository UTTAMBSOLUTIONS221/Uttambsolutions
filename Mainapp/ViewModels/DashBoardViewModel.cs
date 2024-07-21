using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mainapp.Miniapps.News.Pages;
using Mainapp.Miniapps.Weather.Pages;
using Mainapp.Pages.Users;
using System.Windows.Input;

namespace Mainapp.ViewModels
{
    public class DashBoardViewModel : ObservableObject
    {
        public ICommand OpenWeatherAppCommand { get; }
        public ICommand OpenNewsAppCommand { get; }
        public ICommand SignOutCommand { get; }

        public DashBoardViewModel()
        {
            OpenWeatherAppCommand = new AsyncRelayCommand(OpenWeatherAppAsync);
            OpenNewsAppCommand = new AsyncRelayCommand(OpenNewsAppAsync);
            SignOutCommand = new AsyncRelayCommand(SignOutAsync);
        }

        private async Task OpenWeatherAppAsync()
        {
            await Shell.Current.GoToAsync($"//{nameof(WeatherPage)}");
        }

        private async Task OpenNewsAppAsync()
        {
            await Shell.Current.GoToAsync($"//{nameof(NewsPage)}");
        }

        private async Task SignOutAsync()
        {
            if (Preferences.ContainsKey(nameof(App.UserDetails)))
            {
                Preferences.Remove(nameof(App.UserDetails));
            }
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}
