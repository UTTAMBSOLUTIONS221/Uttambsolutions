using Mainapp.Controls;
using Mainapp.Miniapps.Ecommerce.Views;
using Mainapp.Views;

namespace Mainapp.Constants
{
    public static class AppConstant
    {
        public static async Task AddFlyoutMenusDetails(string pageName)
        {
            AppShell.Current.FlyoutHeader = new FlyoutHeaderControl();

            var itemsToRemove = AppShell.Current.Items.Where(f => f.Route == nameof(CommonDashboardPage) || f.Route == nameof(SokojijiDashboardPage)).ToList();
            foreach (var item in itemsToRemove)
            {
                AppShell.Current.Items.Remove(item);
            }

            FlyoutItem flyoutItem = null;

            if (pageName == nameof(SokojijiDashboardPage))
            {
                flyoutItem = new FlyoutItem()
                {
                    Title = "Sokojiji Dashboard",
                    Route = pageName,
                    FlyoutDisplayOptions = FlyoutDisplayOptions.AsMultipleItems,
                    Items =
                    {
                        new ShellContent
                        {
                            Icon = Icons.Dashboard,
                            Title = "Dashboard",
                            ContentTemplate = new DataTemplate(typeof(SokojijiDashboardPage)),
                        },
                        new ShellContent
                        {
                            Icon = Icons.People,
                            Title = "Bible",
                            ContentTemplate = new DataTemplate(typeof(SokojijiDashboardPage)),
                        },
                        new ShellContent
                        {
                            Icon = Icons.People,
                            Title = "Forums",
                            ContentTemplate = new DataTemplate(typeof(SokojijiDashboardPage)),
                        },
                        new ShellContent
                        {
                            Icon = Icons.People,
                            Title = "Groups",
                            ContentTemplate = new DataTemplate(typeof(SokojijiDashboardPage)),
                        },
                        new ShellContent
                        {
                            Icon = Icons.AboutUs,
                            Title = "Profile",
                            ContentTemplate = new DataTemplate(typeof(SokojijiDashboardPage)),
                        },
                    }
                };
            }
            else if (pageName == nameof(CommonDashboardPage))
            {
                flyoutItem = new FlyoutItem()
                {
                    Title = "Common Dashboard",
                    Route = pageName,
                    FlyoutDisplayOptions = FlyoutDisplayOptions.AsMultipleItems,
                    Items =
                    {
                        new ShellContent
                        {
                            Icon = Icons.Dashboard,
                            Title = "Dashboard",
                            ContentTemplate = new DataTemplate(typeof(CommonDashboardPage)),
                        },
                        new ShellContent
                        {
                            Icon = Icons.People,
                            Title = "Profile",
                            ContentTemplate = new DataTemplate(typeof(CommonDashboardPage)),
                        },
                    }
                };
            }

            if (flyoutItem != null && !AppShell.Current.Items.Contains(flyoutItem))
            {
                AppShell.Current.Items.Add(flyoutItem);
            }

            // Navigate to the requested page
            await Shell.Current.GoToAsync($"///{pageName}");
        }
    }
}
