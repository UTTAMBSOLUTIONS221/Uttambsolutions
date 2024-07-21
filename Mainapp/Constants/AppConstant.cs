using Mainapp.Controls;
using Mainapp.Pages;

namespace Mainapp.Constants
{
    public class AppConstant
    {
        public async static Task AddFlyoutMenusDetails()
        {
            AppShell.Current.FlyoutHeader = new FlyoutHeaderControl();

            var currentRoute = Shell.Current.CurrentState.Location.OriginalString;

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