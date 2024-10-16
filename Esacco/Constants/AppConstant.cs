using DBL;

namespace Esacco.Constants
{
    public class AppConstant
    {
        public async static Task AddFlyoutMenusDetails()
        {
            AppShell.Current.FlyoutHeader = new FlyoutHeaderControl();
            var adminDashboardInfo = AppShell.Current.Items.Where(f => f.Route == nameof(AdminDashboardPage)).FirstOrDefault();
            if (adminDashboardInfo != null) AppShell.Current.Items.Remove(adminDashboardInfo);

            var parcelCollectionCenterDashboardInfo = AppShell.Current.Items.Where(f => f.Route == nameof(ParcelCollectionCenterDashboardPage)).FirstOrDefault();
            if (parcelCollectionCenterDashboardInfo != null) AppShell.Current.Items.Remove(parcelCollectionCenterDashboardInfo);

            var parcelCollectionCourierDashboardInfo = AppShell.Current.Items.Where(f => f.Route == nameof(ParcelCollectionCourierDashboardPage)).FirstOrDefault();
            if (parcelCollectionCourierDashboardInfo != null) AppShell.Current.Items.Remove(parcelCollectionCourierDashboardInfo);

            var parcelCollectionCustomerDashboardInfo = AppShell.Current.Items.Where(f => f.Route == nameof(ParcelCollectionCustomerDashboardPage)).FirstOrDefault();
            if (parcelCollectionCustomerDashboardInfo != null) AppShell.Current.Items.Remove(parcelCollectionCustomerDashboardInfo);

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
            else if (App.UserDetails.Usermodel.Rolename == "Parcel Collection Center")
            {
                var flyoutItem = new FlyoutItem()
                {
                    Title = "Dashboard",
                    Route = nameof(ParcelCollectionCenterDashboardPage),
                    FlyoutDisplayOptions = FlyoutDisplayOptions.AsMultipleItems,
                    Items =
                    {
                        new ShellContent
                        {
                            Icon = Icons.Dashboard,
                            Title = "Dashboard",
                            ContentTemplate = new DataTemplate(typeof(ParcelCollectionCenterDashboardPage)),
                        },
                        //new ShellContent
                        //{
                        //    Icon = Icons.user,
                        //    Title = "Profile",
                        //    ContentTemplate = new DataTemplate(() => new UserProfilePage(new UserProfilePageViewModel(bl))),
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
                            await Shell.Current.GoToAsync($"//{nameof(ParcelCollectionCenterDashboardPage)}");
                        });
                    }
                    else
                    {
                        await Shell.Current.GoToAsync($"//{nameof(ParcelCollectionCenterDashboardPage)}");
                    }
                }
            }
            else if (App.UserDetails.Usermodel.Rolename == "Parcel Drop Courier")
            {
                var flyoutItem = new FlyoutItem()
                {
                    Title = "Dashboard",
                    Route = nameof(ParcelCollectionCourierDashboardPage),
                    FlyoutDisplayOptions = FlyoutDisplayOptions.AsMultipleItems,
                    Items =
                    {
                        new ShellContent
                        {
                            Icon = Icons.Dashboard,
                            Title = "Dashboard",
                            ContentTemplate = new DataTemplate(typeof(ParcelCollectionCourierDashboardPage)),
                        },
                        new ShellContent
                        {
                            Icon = Icons.user,
                            Title = "Profile",
                            ContentTemplate = new DataTemplate(() => new UserProfilePage(new LoginPageViewModel(bl))),
                        },
                        new ShellContent
                        {
                            Icon = Icons.user,
                            Title = "Drop Centers",
                            ContentTemplate = new DataTemplate(() => new CollectionDropCentersPage(new ParcelDropViewModel(bl))),
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
                            await Shell.Current.GoToAsync($"//{nameof(ParcelCollectionCourierDashboardPage)}");
                        });
                    }
                    else
                    {
                        await Shell.Current.GoToAsync($"//{nameof(ParcelCollectionCourierDashboardPage)}");
                    }
                }
            }
            else
            {
                var flyoutItem = new FlyoutItem()
                {
                    Title = "Dashboard",
                    Route = nameof(ParcelCollectionCustomerDashboardPage),
                    FlyoutDisplayOptions = FlyoutDisplayOptions.AsMultipleItems,
                    Items =
                    {
                        new ShellContent
                        {
                            Icon = Icons.Dashboard,
                            Title = "Dashboard",
                            ContentTemplate = new DataTemplate(typeof(ParcelCollectionCustomerDashboardPage)),
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
                            await Shell.Current.GoToAsync($"//{nameof(ParcelCollectionCustomerDashboardPage)}");
                        });
                    }
                    else
                    {
                        await Shell.Current.GoToAsync($"//{nameof(ParcelCollectionCustomerDashboardPage)}");
                    }
                }
            }
        }
    }
}
