﻿using DBL;
using Esacco.Controls;
using Esacco.ViewModels;
using Esacco.Views;
using Esacco.Views.Dashboards;

namespace Esacco.Constants
{
    public class AppConstant
    {
        public async static Task AddFlyoutMenusDetails()
        {
            AppShell.Current.FlyoutHeader = new FlyoutHeaderControl();

            var bl = App.Current.Handler.MauiContext.Services.GetService<BL>();

            if (App.UserDetails.Usermodel.Rolename == "Super Admin" || App.UserDetails.Usermodel.Rolename == "System Admin")
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
                            Title = "Dashboard",
                            ContentTemplate = new DataTemplate(typeof(AdminDashboardPage)),
                        },
                        new ShellContent
                        {
                            Icon = Icons.user,
                            Title = "Profile",
                            ContentTemplate = new DataTemplate(() => new UserProfilePage(new UserManagementViewModel(bl))),
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
            else if (App.UserDetails.Usermodel.Rolename == "Sacco Administrator")
            {
                var flyoutItem = new FlyoutItem()
                {
                    Title = "Dashboard",
                    Route = nameof(SaccoAdministratorPage),
                    FlyoutDisplayOptions = FlyoutDisplayOptions.AsMultipleItems,
                    Items =
                    {
                        new ShellContent
                        {
                            Icon = Icons.Dashboard,
                            Title = "Dashboard",
                            ContentTemplate = new DataTemplate(typeof(SaccoAdministratorPage)),
                        },
                        new ShellContent
                        {
                            Icon = Icons.user,
                            Title = "Profile",
                            ContentTemplate = new DataTemplate(() => new UserProfilePage(new UserManagementViewModel(bl))),
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
                            await Shell.Current.GoToAsync($"//{nameof(SaccoAdministratorPage)}");
                        });
                    }
                    else
                    {
                        await Shell.Current.GoToAsync($"//{nameof(SaccoAdministratorPage)}");
                    }
                }
            }
            else if (App.UserDetails.Usermodel.Rolename == "Sacco Driver")
            {
                var flyoutItem = new FlyoutItem()
                {
                    Title = "Dashboard",
                    Route = nameof(SaccoDriverPage),
                    FlyoutDisplayOptions = FlyoutDisplayOptions.AsMultipleItems,
                    Items =
                    {
                        new ShellContent
                        {
                            Icon = Icons.Dashboard,
                            Title = "Dashboard",
                            ContentTemplate = new DataTemplate(typeof(SaccoDriverPage)),
                        },
                        new ShellContent
                        {
                            Icon = Icons.user,
                            Title = "Profile",
                            ContentTemplate = new DataTemplate(() => new UserProfilePage(new UserManagementViewModel(bl))),
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
                            await Shell.Current.GoToAsync($"//{nameof(SaccoDriverPage)}");
                        });
                    }
                    else
                    {
                        await Shell.Current.GoToAsync($"//{nameof(SaccoDriverPage)}");
                    }
                }
            }
            else
            {
                var flyoutItem = new FlyoutItem()
                {
                    Title = "Dashboard",
                    Route = nameof(SaccoAdministratorPage),
                    FlyoutDisplayOptions = FlyoutDisplayOptions.AsMultipleItems,
                    Items =
                    {
                        new ShellContent
                        {
                            Icon = Icons.Dashboard,
                            Title = "Dashboard",
                            ContentTemplate = new DataTemplate(typeof(SaccoAdministratorPage)),
                        },
                        new ShellContent
                        {
                            Icon = Icons.user,
                            Title = "Profile",
                            ContentTemplate = new DataTemplate(() => new UserProfilePage(new UserManagementViewModel(bl))),
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
                            await Shell.Current.GoToAsync($"//{nameof(SaccoAdministratorPage)}");
                        });
                    }
                    else
                    {
                        await Shell.Current.GoToAsync($"//{nameof(SaccoAdministratorPage)}");
                    }
                }
            }
        }
    }
}
