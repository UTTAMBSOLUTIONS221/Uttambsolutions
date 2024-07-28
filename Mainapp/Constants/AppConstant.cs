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

            var flyoutItem = new FlyoutItem()
            {
                Title = "Dashboard Page",
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

            if (!AppShell.Current.Items.Contains(flyoutItem))
            {
                AppShell.Current.Items.Add(flyoutItem);
            }

            await Shell.Current.GoToAsync($"//{pageName}");
        }
    }
}
