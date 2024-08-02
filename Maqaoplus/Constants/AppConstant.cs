using Maqaoplus.Controls;
using Maqaoplus.Views.Dashboards;

namespace Maqaoplus.Constants
{
    public class AppConstant
    {
        public async static Task AddFlyoutMenusDetails()
        {
            AppShell.Current.FlyoutHeader = new FlyoutHeaderControl();
            var adminDashboardInfo = AppShell.Current.Items.Where(f => f.Route == nameof(AdminDashboardPage)).FirstOrDefault();
            if (adminDashboardInfo != null) AppShell.Current.Items.Remove(adminDashboardInfo);

            var customerDashboardInfo = AppShell.Current.Items.Where(f => f.Route == nameof(CustomerDashboardPage)).FirstOrDefault();
            if (customerDashboardInfo != null) AppShell.Current.Items.Remove(customerDashboardInfo);

            var userDashboardInfo = AppShell.Current.Items.Where(f => f.Route == nameof(UserDashboardPage)).FirstOrDefault();
            if (userDashboardInfo != null) AppShell.Current.Items.Remove(userDashboardInfo);

            if (App.UserDetails.Rolename == "Super Admin")
            {
                var flyoutItem = new FlyoutItem()
                {
                    Title = "Dashboard",
                    Route = nameof(AdminDashboardPage),
                    FlyoutDisplayOptions = FlyoutDisplayOptions.AsMultipleItems,
                    Items =
                    {
                                new ShellContent
                                {
                                    Icon = Icons.Dashboard,
                                    Title = "Admin Dashboard",
                                    ContentTemplate = new DataTemplate(typeof(AdminDashboardPage)),
                                },
                                new ShellContent
                                {
                                    Icon = Icons.AboutUs,
                                    Title = "Admin Profile",
                                    ContentTemplate = new DataTemplate(typeof(AdminDashboardPage)),
                                },
                   }
                };

                if (!AppShell.Current.Items.Contains(flyoutItem))
                {
                    AppShell.Current.Items.Add(flyoutItem);
                    if (DeviceInfo.Platform == DevicePlatform.WinUI)
                    {
                        AppShell.Current.Dispatcher.Dispatch(async () =>
                        {
                            await Shell.Current.GoToAsync($"//{nameof(AdminDashboardPage)}");
                        });
                    }
                    else
                    {
                        await Shell.Current.GoToAsync($"//{nameof(AdminDashboardPage)}");
                    }
                }
            }
            else if (App.UserDetails.Rolename == "Maqaoplus Property House Owner")
            {
                var flyoutItem = new FlyoutItem()
                {
                    Title = "Dashboard",
                    Route = nameof(CustomerDashboardPage),
                    FlyoutDisplayOptions = FlyoutDisplayOptions.AsMultipleItems,
                    Items =
                            {
                                new ShellContent
                                {
                                    Icon = Icons.Dashboard,
                                    Title = "Dashboard",
                                    ContentTemplate = new DataTemplate(typeof(CustomerDashboardPage)),
                                },
                                new ShellContent
                                {
                                    Icon = Icons.People,
                                    Title = "Bible",
                                    ContentTemplate = new DataTemplate(typeof(CustomerDashboardPage)),
                                },
                                new ShellContent
                                {
                                    Icon = Icons.People,
                                    Title = "Forums",
                                    ContentTemplate = new DataTemplate(typeof(CustomerDashboardPage)),
                                },
                                new ShellContent
                                {
                                    Icon = Icons.People,
                                    Title = "Groups",
                                    ContentTemplate = new DataTemplate(typeof(CustomerDashboardPage)),
                                },
                                new ShellContent
                                {
                                    Icon = Icons.AboutUs,
                                    Title = "Profile",
                                    ContentTemplate = new DataTemplate(typeof(CustomerDashboardPage)),
                                },
                            }
                };
                if (!AppShell.Current.Items.Contains(flyoutItem))
                {
                    AppShell.Current.Items.Add(flyoutItem);
                    if (DeviceInfo.Platform == DevicePlatform.WinUI)
                    {
                        AppShell.Current.Dispatcher.Dispatch(async () =>
                        {
                            await Shell.Current.GoToAsync($"//{nameof(CustomerDashboardPage)}");
                        });
                    }
                    else
                    {
                        await Shell.Current.GoToAsync($"//{nameof(CustomerDashboardPage)}");
                    }
                }
            }
            else
            {
                var flyoutItem = new FlyoutItem()
                {
                    Title = "Dashboard",
                    Route = nameof(UserDashboardPage),
                    FlyoutDisplayOptions = FlyoutDisplayOptions.AsMultipleItems,
                    Items =
                            {
                                new ShellContent
                                {
                                    Icon = Icons.Dashboard,
                                    Title = "Dashboard",
                                    ContentTemplate = new DataTemplate(typeof(UserDashboardPage)),
                                },
                                new ShellContent
                                {
                                    Icon = Icons.People,
                                    Title = "Bible",
                                    ContentTemplate = new DataTemplate(typeof(UserDashboardPage)),
                                },
                                new ShellContent
                                {
                                    Icon = Icons.People,
                                    Title = "Forums",
                                    ContentTemplate = new DataTemplate(typeof(UserDashboardPage)),
                                },
                                new ShellContent
                                {
                                    Icon = Icons.People,
                                    Title = "Groups",
                                    ContentTemplate = new DataTemplate(typeof(UserDashboardPage)),
                                },
                                new ShellContent
                                {
                                    Icon = Icons.AboutUs,
                                    Title = "Profile",
                                    ContentTemplate = new DataTemplate(typeof(UserDashboardPage)),
                                },
                            }
                };
                if (!AppShell.Current.Items.Contains(flyoutItem))
                {
                    AppShell.Current.Items.Add(flyoutItem);
                    if (DeviceInfo.Platform == DevicePlatform.WinUI)
                    {
                        AppShell.Current.Dispatcher.Dispatch(async () =>
                        {
                            await Shell.Current.GoToAsync($"//{nameof(UserDashboardPage)}");
                        });
                    }
                    else
                    {
                        await Shell.Current.GoToAsync($"//{nameof(UserDashboardPage)}");
                    }
                }
            }
        }
    }
}
