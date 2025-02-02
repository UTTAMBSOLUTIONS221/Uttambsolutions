﻿using DBL;
using Maqaoplus.Controls;
using Maqaoplus.ViewModels;
using Maqaoplus.ViewModels.Agreements;
using Maqaoplus.ViewModels.BillsandPayments;
using Maqaoplus.ViewModels.PropertyHouse;
using Maqaoplus.ViewModels.PropertyHouseTenants;
using Maqaoplus.ViewModels.TenantBillsandPayments;
using Maqaoplus.Views;
using Maqaoplus.Views.Agreements;
using Maqaoplus.Views.BillsandPayments;
using Maqaoplus.Views.Dashboards;
using Maqaoplus.Views.PropertyHouse;
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

            var propertyCaretakerDashboardInfo = AppShell.Current.Items.Where(f => f.Route == nameof(PropertyCaretakerDashboardPage)).FirstOrDefault();
            if (propertyCaretakerDashboardInfo != null) AppShell.Current.Items.Remove(propertyCaretakerDashboardInfo);


            var agentDashboardPageInfo = AppShell.Current.Items.Where(f => f.Route == nameof(AgentDashboardPage)).FirstOrDefault();
            if (agentDashboardPageInfo != null) AppShell.Current.Items.Remove(agentDashboardPageInfo);

            var userDashboardInfo = AppShell.Current.Items.Where(f => f.Route == nameof(UserDashboardPage)).FirstOrDefault();
            if (userDashboardInfo != null) AppShell.Current.Items.Remove(userDashboardInfo);

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
                            ContentTemplate = new DataTemplate(() => new UserProfilePage(new UserProfilePageViewModel(bl))),
                        },
                        new ShellContent
                        {
                            Icon = Icons.house,
                            Title = "Houses",
                            ContentTemplate = new DataTemplate(() => new AgentPropertyHousesPage(new PropertyHouseViewModel(bl))),
                        },
                        new ShellContent
                        {
                            Icon = Icons.groupusers,
                            Title = "Tenants",
                            ContentTemplate = new DataTemplate(() => new PropertyHousesRoomTenantsPage(new PropertyHousesRoomTenantsViewModel(bl))),
                        },
                         new ShellContent
                        {
                            Icon = Icons.invoice,
                            Title = "Bills",
                            ContentTemplate = new DataTemplate(() => new OwnerPropertyHousesBillsPage(new PropertyHousesBillsandPaymentsViewModel(bl))),
                        },
                        new ShellContent
                        {
                            Icon = Icons.dollar,
                            Title = "Payments",
                            ContentTemplate = new DataTemplate(() => new OwnerPropertyHousesPaymentsPage(new PropertyHousesBillsandPaymentsViewModel(bl))),
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
                            ContentTemplate = new DataTemplate(() => new UserProfilePage(new UserProfilePageViewModel(bl))),
                        },
                         new ShellContent
                        {
                            Icon = Icons.agreement,
                            Title = "Agreement",
                            ContentTemplate = new DataTemplate(() => new PropertyOwnerAgreementsPage(new SystemAgreementViewModel(bl))),
                        },
                        new ShellContent
                        {
                            Icon = Icons.house,
                            Title = "Houses",
                            ContentTemplate = new DataTemplate(() => new PropertyHousesPage(new PropertyHouseViewModel(bl))),
                        },
                        new ShellContent
                        {
                            Icon = Icons.user,
                            Title = "Caretaker",
                            ContentTemplate = new DataTemplate(() => new PropertyHousesCaretakerPage(new PropertyHouseViewModel(bl))),
                        },
                        new ShellContent
                        {
                            Icon = Icons.groupusers,
                            Title = "Tenants",
                            ContentTemplate = new DataTemplate(() => new PropertyHousesRoomTenantsPage(new PropertyHousesRoomTenantsViewModel(bl))),
                        },
                         new ShellContent
                        {
                            Icon = Icons.invoice,
                            Title = "Bills",
                            ContentTemplate = new DataTemplate(() => new OwnerPropertyHousesBillsPage(new PropertyHousesBillsandPaymentsViewModel(bl))),
                        },
                        new ShellContent
                        {
                            Icon = Icons.dollar,
                            Title = "Payments",
                            ContentTemplate = new DataTemplate(() => new OwnerPropertyHousesPaymentsPage(new PropertyHousesBillsandPaymentsViewModel(bl))),
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
                            await Shell.Current.GoToAsync($"//{nameof(PropertyOwnerDashboardPage)}");
                        });
                    }
                    else
                    {
                        await Shell.Current.GoToAsync($"//{nameof(PropertyOwnerDashboardPage)}");
                    }
                }
            }
            else if (App.UserDetails.Usermodel.Rolename == "Maqaoplus Property House CareTaker")
            {
                var flyoutItem = new FlyoutItem()
                {
                    Title = "Dashboard",
                    Route = nameof(PropertyCaretakerDashboardPage),
                    FlyoutDisplayOptions = FlyoutDisplayOptions.AsMultipleItems,
                    Items =
                    {
                        new ShellContent
                        {
                            Icon = Icons.Dashboard,
                            Title = "Dashboard",
                            ContentTemplate = new DataTemplate(typeof(PropertyCaretakerDashboardPage)),
                        },
                        new ShellContent
                        {
                            Icon = Icons.user,
                            Title = "Profile",
                            ContentTemplate = new DataTemplate(() => new UserProfilePage(new UserProfilePageViewModel(bl))),
                        },
                        new ShellContent
                        {
                            Icon = Icons.house,
                            Title = "Houses",
                            ContentTemplate = new DataTemplate(() => new PropertyHousesPage(new PropertyHouseViewModel(bl))),
                        },
                        new ShellContent
                        {
                            Icon = Icons.groupusers,
                            Title = "Tenants",
                            ContentTemplate = new DataTemplate(() => new PropertyHousesRoomTenantsPage(new PropertyHousesRoomTenantsViewModel(bl))),
                        },
                         new ShellContent
                        {
                            Icon = Icons.invoice,
                            Title = "Bills",
                            ContentTemplate = new DataTemplate(() => new OwnerPropertyHousesBillsPage(new PropertyHousesBillsandPaymentsViewModel(bl))),
                        },
                        new ShellContent
                        {
                            Icon = Icons.dollar,
                            Title = "Payments",
                            ContentTemplate = new DataTemplate(() => new OwnerPropertyHousesPaymentsPage(new PropertyHousesBillsandPaymentsViewModel(bl))),
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
                            await Shell.Current.GoToAsync($"//{nameof(PropertyCaretakerDashboardPage)}");
                        });
                    }
                    else
                    {
                        await Shell.Current.GoToAsync($"//{nameof(PropertyCaretakerDashboardPage)}");
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
                            ContentTemplate = new DataTemplate(() => new UserProfilePage(new UserProfilePageViewModel(bl))),
                        },
                        new ShellContent
                        {
                            Icon = Icons.agreement,
                            Title = "Agreement",
                            ContentTemplate = new DataTemplate(() => new PropertyAgentAgreementsPage(new SystemAgreementViewModel(bl))),
                        },
                        new ShellContent
                        {
                            Icon = Icons.house,
                            Title = "Houses",
                            ContentTemplate = new DataTemplate(() => new AgentPropertyHousesPage(new PropertyHouseViewModel(bl))),
                        },
                        new ShellContent
                        {
                            Icon = Icons.groupusers,
                            Title = "Tenants",
                            ContentTemplate = new DataTemplate(() => new AgentPropertyHousesRoomTenantsPage(new PropertyHousesRoomTenantsViewModel(bl))),
                        },
                         new ShellContent
                        {
                            Icon = Icons.invoice,
                            Title = "Bills",
                            ContentTemplate = new DataTemplate(() => new AgentPropertyHousesBillsPage(new PropertyHousesBillsandPaymentsViewModel(bl))),
                        },
                        new ShellContent
                        {
                            Icon = Icons.dollar,
                            Title = "Payments",
                            ContentTemplate = new DataTemplate(() => new AgentPropertyHousesPaymentsPage(new PropertyHousesBillsandPaymentsViewModel(bl))),
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
                            Title = "Dashboard",
                            ContentTemplate = new DataTemplate(typeof(UserDashboardPage)),
                        },
                        new ShellContent
                        {
                            Icon = Icons.user,
                            Title = "Profile",
                            ContentTemplate = new DataTemplate(() => new UserProfilePage(new UserProfilePageViewModel(bl))),
                        },
                        new ShellContent
                        {
                            Icon = Icons.agreement,
                            Title = "Agreement",
                            ContentTemplate = new DataTemplate(() => new PropertyTenantAgreementsPage(new SystemAgreementViewModel(bl))),
                        },
                        new ShellContent
                        {
                            Icon = Icons.invoice,
                            Title = "Bills",
                            ContentTemplate = new DataTemplate(() => new PropertyHousesTenantBillsPage(new PropertyHousesTenantBillsViewModel(bl))),
                        },
                        new ShellContent
                        {
                            Icon = Icons.dollar,
                            Title = "Payments",
                            ContentTemplate = new DataTemplate(() => new PropertyHousesTenantPaymentsPage(new PropertyHousesTenantPaymentsViewModel(bl))),
                        },
                        //new ShellContent
                        //{
                        //    Icon = Icons.dollar,
                        //    Title = "Services",
                        //    ContentTemplate = new DataTemplate(() => new ServiceOfferingPage(serviceProvider)),
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
