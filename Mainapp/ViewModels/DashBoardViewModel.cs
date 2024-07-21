using CommunityToolkit.Mvvm.ComponentModel;
using Mainapp.Controls;
using Mainapp.Costants;
using Mainapp.Miniapps.News.Pages;
using Mainapp.Miniapps.Weather;
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
            // Ensure AppShell.Current is not null
            if (AppShell.Current == null)
            {
                throw new InvalidOperationException("AppShell.Current is not initialized.");
            }

            // Set Flyout Header
            AppShell.Current.FlyoutHeader = new FlyoutHeaderControl();

            // Update Flyout Menu Items
            await UpdateFlyoutMenuForWeatherAsync();

            // Navigate to WeatherDashBoardPage
            await _navigation.PushAsync(new WeatherDashBoardPage());
        }

        private async Task OpenNewsAppAsync()
        {
            // Ensure AppShell.Current is not null
            if (AppShell.Current == null)
            {
                throw new InvalidOperationException("AppShell.Current is not initialized.");
            }

            // Set Flyout Header
            AppShell.Current.FlyoutHeader = new FlyoutHeaderControl();

            // Update Flyout Menu Items
            await UpdateFlyoutMenuForNewsAsync();

            // Navigate to NewsPage
            await _navigation.PushAsync(new NewsPage());
        }


        private async Task UpdateFlyoutMenuForWeatherAsync()
        {
            // Ensure AppShell.Current and its Items are not null
            if (AppShell.Current == null || AppShell.Current.Items == null)
            {
                throw new InvalidOperationException("AppShell.Current or its Items are not initialized.");
            }

            // Clear previous items if needed
            var existingItems = AppShell.Current.Items.OfType<FlyoutItem>().ToList();
            foreach (var item in existingItems)
            {
                AppShell.Current.Items.Remove(item);
            }

            // Add specific Flyout items for Weather Dashboard
            var weatherFlyoutItem = new FlyoutItem()
            {
                Title = "Weather Dashboard",
                Route = nameof(WeatherDashBoardPage),
                FlyoutDisplayOptions = FlyoutDisplayOptions.AsSingleItem,
                Items =
                {
                    new ShellContent
                    {
                        Icon = Icons.Dashboard,
                        Title = "Weather Dashboard",
                        ContentTemplate = new DataTemplate(typeof(WeatherDashBoardPage)),
                    }
                }
            };

            if (!AppShell.Current.Items.Contains(weatherFlyoutItem))
            {
                AppShell.Current.Items.Add(weatherFlyoutItem);
                await Shell.Current.GoToAsync($"//{nameof(WeatherDashBoardPage)}");
            }
        }

        private async Task UpdateFlyoutMenuForNewsAsync()
        {
            // Ensure AppShell.Current and its Items are not null
            if (AppShell.Current == null || AppShell.Current.Items == null)
            {
                throw new InvalidOperationException("AppShell.Current or its Items are not initialized.");
            }

            // Clear previous items if needed
            var existingItems = AppShell.Current.Items.OfType<FlyoutItem>().ToList();
            foreach (var item in existingItems)
            {
                AppShell.Current.Items.Remove(item);
            }

            // Add specific Flyout items for News Dashboard
            var newsFlyoutItem = new FlyoutItem()
            {
                Title = "News Dashboard",
                Route = nameof(NewsPage),
                FlyoutDisplayOptions = FlyoutDisplayOptions.AsSingleItem,
                Items =
                {
                    new ShellContent
                    {
                        Icon = Icons.Dashboard,
                        Title = "News Dashboard",
                        ContentTemplate = new DataTemplate(typeof(NewsPage)),
                    }
                }
            };

            if (!AppShell.Current.Items.Contains(newsFlyoutItem))
            {
                AppShell.Current.Items.Add(newsFlyoutItem);
                await Shell.Current.GoToAsync($"//{nameof(NewsPage)}");
            }
        }
    }
}