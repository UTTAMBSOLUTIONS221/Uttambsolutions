using DBL;
using Parceldrop.Controls;

namespace Parceldrop.Constants
{
    public class AppConstant
    {
        public async static Task AddFlyoutMenusDetails()
        {
            AppShell.Current.FlyoutHeader = new FlyoutHeaderControl();
            var adminDashboardInfo = AppShell.Current.Items.Where(f => f.Route == nameof(AdminDashboardPage)).FirstOrDefault();
            if (adminDashboardInfo != null) AppShell.Current.Items.Remove(adminDashboardInfo);
            var bl = App.Current.Handler.MauiContext.Services.GetService<BL>();
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
    }
}
