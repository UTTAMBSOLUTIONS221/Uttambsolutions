using App.Miniapps.Education.Pages;
using App.Miniapps.Finance.Pages;
using App.Miniapps.Health.Pages;
using App.Miniapps.News.Pages;
using App.Miniapps.Sports.Pages;
using App.Miniapps.Weather.Pages;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace App.ViewModels
{
    public class DashBoardViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private readonly INavigation _navigation;

        public DashBoardViewModel(INavigation navigation)
        {
            _navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));

            OpenWeatherAppCommand = new Command(async () => await NavigateToPageAsync(() => new WeatherPage()));
            OpenNewsAppCommand = new Command(async () => await NavigateToPageAsync(() => new NewsPage()));
            OpenFinanceAppCommand = new Command(async () => await NavigateToPageAsync(() => new FinancePage()));
            OpenSportsAppCommand = new Command(async () => await NavigateToPageAsync(() => new SportsPage()));
            OpenHealthAppCommand = new Command(async () => await NavigateToPageAsync(() => new HealthPage()));
            OpenEducationAppCommand = new Command(async () => await NavigateToPageAsync(() => new EducationPage()));
            //OpenEntertainmentAppCommand = new Command(async () => await NavigateToPageAsync(() => new EntertainmentPage()));
            //OpenTravelAppCommand = new Command(async () => await NavigateToPageAsync(() => new TravelPage()));
            //OpenTechnologyAppCommand = new Command(async () => await NavigateToPageAsync(() => new TechnologyPage()));
        }

        private async Task NavigateToPageAsync(Func<Page> pageFactory)
        {
            if (pageFactory == null) throw new ArgumentNullException(nameof(pageFactory));

            try
            {
                var page = pageFactory();
                await _navigation.PushAsync(page);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Navigation Error", $"An error occurred while navigating: {ex.Message}", "OK");
            }
        }

        public ICommand OpenWeatherAppCommand { get; }
        public ICommand OpenNewsAppCommand { get; }
        public ICommand OpenFinanceAppCommand { get; }
        public ICommand OpenSportsAppCommand { get; }
        public ICommand OpenHealthAppCommand { get; }
        public ICommand OpenEducationAppCommand { get; }
        //public ICommand OpenEntertainmentAppCommand { get; }
        //public ICommand OpenTravelAppCommand { get; }
        //public ICommand OpenTechnologyAppCommand { get; }
    }
}
