using System.Windows.Input;

namespace Mainapp.ViewModels
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
