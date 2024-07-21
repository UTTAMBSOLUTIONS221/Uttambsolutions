using Mainapp.Controls;
using Mainapp.Costants;
using Mainapp.Miniapps.Church;
using Mainapp.Miniapps.Church.Pages;
using Mainapp.Miniapps.News;
using Mainapp.Miniapps.Weather;
using Mainapp.Pages;

namespace Mainapp.Constants
{
    public class AppConstant
    {
        public async static Task AddFlyoutMenusDetails()
        {
            AppShell.Current.FlyoutHeader = new FlyoutHeaderControl();

            var currentRoute = Shell.Current.CurrentState.Location.OriginalString;

            // Remove existing dashboard info
            var pagesToRemove = new[] { nameof(ChurchDashBoardPage), nameof(NewsDashBoardPage), nameof(WeatherDashBoardPage) };
            foreach (var page in pagesToRemove)
            {
                var pageInfo = AppShell.Current.Items.Where(f => f.Route == page).FirstOrDefault();
                if (pageInfo != null) AppShell.Current.Items.Remove(pageInfo);
            }

            // Add the general DashboardPage accessible to all logins
            var generalDashboardItem = new FlyoutItem()
            {
                Title = "General Dashboard",
                Route = nameof(DashBoardPage),
                FlyoutDisplayOptions = FlyoutDisplayOptions.AsSingleItem,
                Items =
                {
                    new ShellContent
                    {
                        Icon = Icons.Dashboard,
                        Title = "Dashboard",
                        ContentTemplate = new DataTemplate(typeof(DashBoardPage)),
                    }
                }
            };

            if (!AppShell.Current.Items.Contains(generalDashboardItem))
            {
                AppShell.Current.Items.Insert(0, generalDashboardItem);
                if (DeviceInfo.Platform == DevicePlatform.WinUI)
                {
                    AppShell.Current.Dispatcher.Dispatch(async () =>
                    {
                        await Shell.Current.GoToAsync($"//{nameof(DashBoardPage)}");
                    });
                }
                else
                {
                    await Shell.Current.GoToAsync($"//{nameof(DashBoardPage)}");
                }
            }

            FlyoutItem flyoutItem = null;

            if (currentRoute.Contains(nameof(ChurchDashBoardPage)))
            {
                flyoutItem = new FlyoutItem()
                {
                    Title = "Church Dashboard Page",
                    Route = nameof(ChurchDashBoardPage),
                    FlyoutDisplayOptions = FlyoutDisplayOptions.AsMultipleItems,
                    Items =
                    {
                        new ShellContent
                        {
                            Icon = Icons.Dashboard,
                            Title = "Dashboard",
                            ContentTemplate = new DataTemplate(typeof(ChurchDashBoardPage)),
                        },
                        new ShellContent
                        {
                            Icon = Icons.People,
                            Title = "Bible",
                            ContentTemplate = new DataTemplate(typeof(ChurchDetailPage)),
                        },
                        new ShellContent
                        {
                            Icon = Icons.People,
                            Title = "Forums",
                            ContentTemplate = new DataTemplate(typeof(ChurchDetailPage)),
                        },
                        new ShellContent
                        {
                            Icon = Icons.People,
                            Title = "Groups",
                            ContentTemplate = new DataTemplate(typeof(ChurchDetailPage)),
                        },
                        new ShellContent
                        {
                            Icon = Icons.AboutUs,
                            Title = "Profile",
                            ContentTemplate = new DataTemplate(typeof(ChurchDetailPage)),
                        },
                    }
                };
            }
            else if (currentRoute.Contains(nameof(NewsDashBoardPage)))
            {
                flyoutItem = new FlyoutItem()
                {
                    Title = "News Dashboard Page",
                    Route = nameof(NewsDashBoardPage),
                    FlyoutDisplayOptions = FlyoutDisplayOptions.AsMultipleItems,
                    Items =
                    {
                        new ShellContent
                        {
                            Icon = Icons.Dashboard,
                            Title = "News Dashboard",
                            ContentTemplate = new DataTemplate(typeof(NewsDashBoardPage)),
                        },
                        new ShellContent
                        {
                            Icon = Icons.AboutUs,
                            Title = "News Profile",
                            ContentTemplate = new DataTemplate(typeof(NewsDashBoardPage)),
                        },
                    }
                };
            }
            else if (currentRoute.Contains(nameof(WeatherDashBoardPage)))
            {
                flyoutItem = new FlyoutItem()
                {
                    Title = "Weather Dashboard Page",
                    Route = nameof(WeatherDashBoardPage),
                    FlyoutDisplayOptions = FlyoutDisplayOptions.AsMultipleItems,
                    Items =
                    {
                        new ShellContent
                        {
                            Icon = Icons.Dashboard,
                            Title = "Weather Dashboard",
                            ContentTemplate = new DataTemplate(typeof(WeatherDashBoardPage)),
                        },
                        new ShellContent
                        {
                            Icon = Icons.AboutUs,
                            Title = "Weather Profile",
                            ContentTemplate = new DataTemplate(typeof(WeatherDashBoardPage)),
                        },
                    }
                };
            }

            if (flyoutItem != null && !AppShell.Current.Items.Contains(flyoutItem))
            {
                AppShell.Current.Items.Add(flyoutItem);
                if (DeviceInfo.Platform == DevicePlatform.WinUI)
                {
                    AppShell.Current.Dispatcher.Dispatch(async () =>
                    {
                        await Shell.Current.GoToAsync($"//{flyoutItem.Route}");
                    });
                }
                else
                {
                    await Shell.Current.GoToAsync($"//{flyoutItem.Route}");
                }
            }
        }
    }
}
