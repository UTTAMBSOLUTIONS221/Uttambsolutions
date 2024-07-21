using Mainapp.Controls;
using Mainapp.Costants;
using Mainapp.Miniapps.Church;
using Mainapp.Miniapps.Church.Pages;
using Mainapp.Miniapps.News;
using Mainapp.Miniapps.Weather;

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

            FlyoutItem flyoutItem = null;

            if (currentRoute.Contains(nameof(ChurchDashBoardPage)))
            {
                flyoutItem = new FlyoutItem()
                {
                    Title = "Dashboard Page",
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
                    Title = "Dashboard Page",
                    Route = nameof(NewsDashBoardPage),
                    FlyoutDisplayOptions = FlyoutDisplayOptions.AsMultipleItems,
                    Items =
                    {
                        new ShellContent
                        {
                            Icon = Icons.Dashboard,
                            Title = "Teacher Dashboard",
                            ContentTemplate = new DataTemplate(typeof(NewsDashBoardPage)),
                        },
                        new ShellContent
                        {
                            Icon = Icons.AboutUs,
                            Title = "Teacher Profile",
                            ContentTemplate = new DataTemplate(typeof(NewsDashBoardPage)),
                        },
                    }
                };
            }
            else if (currentRoute.Contains(nameof(WeatherDashBoardPage)))
            {
                flyoutItem = new FlyoutItem()
                {
                    Title = "Dashboard Page",
                    Route = nameof(WeatherDashBoardPage),
                    FlyoutDisplayOptions = FlyoutDisplayOptions.AsMultipleItems,
                    Items =
                    {
                        new ShellContent
                        {
                            Icon = Icons.Dashboard,
                            Title = "Admin Dashboard",
                            ContentTemplate = new DataTemplate(typeof(WeatherDashBoardPage)),
                        },
                        new ShellContent
                        {
                            Icon = Icons.AboutUs,
                            Title = "Admin Profile",
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
