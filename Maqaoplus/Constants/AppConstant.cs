using Maqaoplus.Controls;
using Maqaoplus.Views;
using Maqaoplus.Views.Dashboards;
using Maqaoplus.Views.OwnerBillsandPayments;
using Maqaoplus.Views.PropertyHouse;
using Maqaoplus.Views.PropertyHouseAgent;
using Maqaoplus.Views.PropertyHouseTenantAgreement;
using Maqaoplus.Views.PropertyHouseTenants;
using Maqaoplus.Views.TenantBillsandPayments;

namespace Maqaoplus.Constants
{
    public class AppConstant
    {
        public async static Task AddFlyoutMenusDetails()
        {
            AppShell.Current.FlyoutHeader = new FlyoutHeaderControl();
            var adminDashboardInfo = AppShell.Current.Items.Where(f => f.Route == nameof(AdminDashboardPage)).FirstOrDefault();
            if (adminDashboardInfo != null) AppShell.Current.Items.Remove(adminDashboardInfo);

            var propertyOwnerDashboardInfo = AppShell.Current.Items.Where(f => f.Route == nameof(PropertyOwnerDashboardPage)).FirstOrDefault();
            if (propertyOwnerDashboardInfo != null) AppShell.Current.Items.Remove(propertyOwnerDashboardInfo);

            var agentDashboardPageInfo = AppShell.Current.Items.Where(f => f.Route == nameof(AgentDashboardPage)).FirstOrDefault();
            if (agentDashboardPageInfo != null) AppShell.Current.Items.Remove(agentDashboardPageInfo);

            var userDashboardInfo = AppShell.Current.Items.Where(f => f.Route == nameof(UserDashboardPage)).FirstOrDefault();
            if (userDashboardInfo != null) AppShell.Current.Items.Remove(userDashboardInfo);

            var serviceProvider = App.Current.Handler.MauiContext.Services.GetService<Services.ServiceProvider>();

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
                            Title = "Admin Dashboard",
                            ContentTemplate = new DataTemplate(typeof(AdminDashboardPage)),
                        },
                        new ShellContent
                        {
                            Icon = Icons.user,
                            Title = "Profile",
                            ContentTemplate = new DataTemplate(typeof(UserProfilePage)),
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
            else if (App.UserDetails.Usermodel.Rolename == "Maqaoplus Property House Owner")
            {
                var flyoutItem = new FlyoutItem()
                {
                    Title = "Dashboard",
                    Route = nameof(PropertyOwnerDashboardPage),
                    FlyoutDisplayOptions = FlyoutDisplayOptions.AsMultipleItems,
                    Items =
                    {
                        new ShellContent
                        {
                            Icon = Icons.Dashboard,
                            Title = "Dashboard",
                            ContentTemplate = new DataTemplate(typeof(PropertyOwnerDashboardPage)),
                        },
                        new ShellContent
                        {
                            Icon = Icons.user,
                            Title = "Profile",
                            ContentTemplate = new DataTemplate(typeof(UserProfilePage)),
                        },
                        new ShellContent
                        {
                            Icon = Icons.house,
                            Title = "Houses",
                            ContentTemplate = new DataTemplate(() => new PropertyHousesPage(serviceProvider)),
                        },
                        new ShellContent
                        {
                            Icon = Icons.groupusers,
                            Title = "Tenants",
                            ContentTemplate = new DataTemplate(() => new PropertyHousesRoomTenantsPage(serviceProvider)),
                        },
                         new ShellContent
                        {
                            Icon = Icons.invoice,
                            Title = "Bills",
                            ContentTemplate = new DataTemplate(() => new PropertyHousesOwnerBillsPage(serviceProvider)),
                        },
                            new ShellContent
                        {
                            Icon = Icons.dollar,
                            Title = "Payments",
                            ContentTemplate = new DataTemplate(() => new PropertyHousesOwnerPaymentsPage(serviceProvider)),
                        },
                        //new ShellContent
                        //{
                        //    Icon = Icons.reports,
                        //    Title = "Reports",
                        //    ContentTemplate = new DataTemplate(typeof(SystemPropertyOwnerReportsPage)),
                        //},
                    }
                };
                if (!AppShell.Current.Items.Contains(flyoutItem))
                {
                    AppShell.Current.Items.Add(flyoutItem);
                    if (DeviceInfo.Platform == DevicePlatform.WinUI)
                    {
                        AppShell.Current.Dispatcher.Dispatch(async () =>
                        {
                            await Shell.Current.GoToAsync($"//{nameof(PropertyOwnerDashboardPage)}");
                        });
                    }
                    else
                    {
                        await Shell.Current.GoToAsync($"//{nameof(PropertyOwnerDashboardPage)}");
                    }
                }
            }
            else if (App.UserDetails.Usermodel.Rolename == "Maqaoplus Property House Agent")
            {
                var flyoutItem = new FlyoutItem()
                {
                    Title = "Dashboard",
                    Route = nameof(AgentDashboardPage),
                    FlyoutDisplayOptions = FlyoutDisplayOptions.AsMultipleItems,
                    Items =
                    {
                        new ShellContent
                        {
                            Icon = Icons.Dashboard,
                            Title = "Dashboard",
                            ContentTemplate = new DataTemplate(typeof(AgentDashboardPage)),
                        },
                        new ShellContent
                        {
                            Icon = Icons.user,
                            Title = "Profile",
                            ContentTemplate = new DataTemplate(typeof(UserProfilePage)),
                        },
                        new ShellContent
                        {
                            Icon = Icons.house,
                            Title = "Houses",
                            ContentTemplate = new DataTemplate(() => new PropertyAgentHousesPage(serviceProvider)),
                        },
                        new ShellContent
                        {
                            Icon = Icons.groupusers,
                            Title = "Tenants",
                            ContentTemplate = new DataTemplate(() => new PropertyHousesRoomTenantsPage(serviceProvider)),
                        },
                         new ShellContent
                        {
                            Icon = Icons.invoice,
                            Title = "Bills",
                            ContentTemplate = new DataTemplate(() => new PropertyHousesOwnerBillsPage(serviceProvider)),
                        },
                            new ShellContent
                        {
                            Icon = Icons.dollar,
                            Title = "Payments",
                            ContentTemplate = new DataTemplate(() => new PropertyHousesOwnerPaymentsPage(serviceProvider)),
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
                            await Shell.Current.GoToAsync($"//{nameof(AgentDashboardPage)}");
                        });
                    }
                    else
                    {
                        await Shell.Current.GoToAsync($"//{nameof(AgentDashboardPage)}");
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
                            Title = " Dashboard",
                            ContentTemplate = new DataTemplate(typeof(UserDashboardPage)),
                        },
                        new ShellContent
                        {
                            Icon = Icons.user,
                            Title = "Profile",
                            ContentTemplate = new DataTemplate(typeof(UserProfilePage)),
                        },
                        new ShellContent
                        {
                            Icon = Icons.agreement,
                            Title = "Agreement",
                            ContentTemplate = new DataTemplate(() => new PropertyHousesTenantAgreementsPage(serviceProvider)),
                        },
                        new ShellContent
                        {
                            Icon = Icons.invoice,
                            Title = "Bills",
                            ContentTemplate = new DataTemplate(() => new PropertyHousesTenantBillsPage(serviceProvider)),
                        },
                        new ShellContent
                        {
                            Icon = Icons.dollar,
                            Title = "Payments",
                            ContentTemplate = new DataTemplate(() => new PropertyHousesTenantPaymentsPage(serviceProvider)),
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
