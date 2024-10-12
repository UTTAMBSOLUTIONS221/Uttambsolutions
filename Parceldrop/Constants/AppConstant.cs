using DBL;
using Parceldrop.Controls;
using Parceldrop.Views.Dashboards;

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
