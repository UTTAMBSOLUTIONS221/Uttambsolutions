using Mainapp.Controls;
using Mainapp.Costants;
using Mainapp.Pages;

namespace Mainapp.Constants
{
    public class AppConstant
    {
        public async static Task AddFlyoutMenusDetails()
        {
            // Set the FlyoutHeader
            AppShell.Current.FlyoutHeader = new FlyoutHeaderControl();

            var currentRoute = Shell.Current.CurrentState.Location.OriginalString;

            // Remove general dashboard item if it exists
            var generalDashboardItem = AppShell.Current.Items
                .OfType<FlyoutItem>()
                .FirstOrDefault(item => item.Route == nameof(DashBoardPage));
            if (generalDashboardItem != null)
            {
                AppShell.Current.Items.Remove(generalDashboardItem);
            }

            // Create and add FlyoutItem for DashBoardPage
            var flyoutItem = new FlyoutItem()
            {
                Title = "Dashboard",
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

            // Add FlyoutItem if it is not already present
            if (!AppShell.Current.Items.Contains(flyoutItem))
            {
                AppShell.Current.Items.Insert(0, flyoutItem);

                if (DeviceInfo.Platform == DevicePlatform.WinUI)
                {
                    await AppShell.Current.Dispatcher.DispatchAsync(async () =>
                    {
                        await Shell.Current.GoToAsync($"//{nameof(DashBoardPage)}");
                    });
                }
                else
                {
                    await Shell.Current.GoToAsync($"//{nameof(DashBoardPage)}");
                }
            }
        }
    }
}
