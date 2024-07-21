using Mainapp.Common;
using Mainapp.Miniapps.Apps.Church;
using Mainapp.Miniapps.Controls;
namespace Mainapp.Miniapps.Constants
{
    public class AppConstant
    {
        public async static Task AddFlyoutMenusDetails()
        {
            // Set the custom flyout header
            AppShell.Current.FlyoutHeader = new FlyoutHeaderControl();

            // Remove existing menu items for different dashboards
            var dashboardRoutes = new[]
            {
                nameof(ChurchDashBoardPage),
                nameof(ChurchDashBoardPage),
                nameof(ChurchDashBoardPage)
            };

            foreach (var route in dashboardRoutes)
            {
                var existingItem = AppShell.Current.Items.FirstOrDefault(f => f.Route == route);
                if (existingItem != null)
                {
                    AppShell.Current.Items.Remove(existingItem);
                }
            }

            // Define menu items for different roles
            var roleId = App.UserDetails.Roleid;
            FlyoutItem flyoutItem = null;

            switch (roleId)
            {
                case 1:
                    flyoutItem = new FlyoutItem
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
                        ContentTemplate = new DataTemplate(typeof(ChurchDashBoardPage)),
                    },
                    new ShellContent
                    {
                        Icon = Icons.People,
                        Title = "Forums",
                        ContentTemplate = new DataTemplate(typeof(ChurchDashBoardPage)),
                    },
                    new ShellContent
                    {
                        Icon = Icons.People,
                        Title = "Groups",
                        ContentTemplate = new DataTemplate(typeof(ChurchDashBoardPage)),
                    },
                    new ShellContent
                    {
                        Icon = Icons.AboutUs,
                        Title = "Profile",
                        ContentTemplate = new DataTemplate(typeof(ChurchDashBoardPage)),
                    }
                }
                    };
                    break;

                case 2:
                    flyoutItem = new FlyoutItem
                    {
                        Title = "Dashboard Page",
                        Route = nameof(ChurchDashBoardPage),
                        FlyoutDisplayOptions = FlyoutDisplayOptions.AsMultipleItems,
                        Items =
                {
                    new ShellContent
                    {
                        Icon = Icons.Dashboard,
                        Title = "Teacher Dashboard",
                        ContentTemplate = new DataTemplate(typeof(ChurchDashBoardPage)),
                    },
                    new ShellContent
                    {
                        Icon = Icons.AboutUs,
                        Title = "Teacher Profile",
                        ContentTemplate = new DataTemplate(typeof(ChurchDashBoardPage)),
                    }
                }
                    };
                    break;

                case 3:
                    flyoutItem = new FlyoutItem
                    {
                        Title = "Dashboard Page",
                        Route = nameof(ChurchDashBoardPage),
                        FlyoutDisplayOptions = FlyoutDisplayOptions.AsMultipleItems,
                        Items =
                {
                    new ShellContent
                    {
                        Icon = Icons.Dashboard,
                        Title = "Admin Dashboard",
                        ContentTemplate = new DataTemplate(typeof(ChurchDashBoardPage)),
                    },
                    new ShellContent
                    {
                        Icon = Icons.AboutUs,
                        Title = "Admin Profile",
                        ContentTemplate = new DataTemplate(typeof(ChurchDashBoardPage)),
                    }
                }
                    };
                    break;
            }

            // Add the new flyout item and navigate
            if (flyoutItem != null && !AppShell.Current.Items.Contains(flyoutItem))
            {
                AppShell.Current.Items.Add(flyoutItem);

                if (DeviceInfo.Platform == DevicePlatform.WinUI)
                {
                    await AppShell.Current.Dispatcher.DispatchAsync(async () =>
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
