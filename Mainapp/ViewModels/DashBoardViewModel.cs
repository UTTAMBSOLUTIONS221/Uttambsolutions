using App.Miniapps.News.Pages;
using App.Miniapps.Weather.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;

namespace App.ViewModels
{
    public class DashBoardViewModel : ObservableObject
    {
        private readonly INavigation _navigation;

        public ICommand OpenWeatherAppCommand => new Command(async () => await OpenWeatherAppAsync());
        public ICommand OpenNewsAppCommand => new Command(async () => await OpenNewsAppAsync());

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
    }

}
