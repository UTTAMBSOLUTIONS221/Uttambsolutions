using Mainapp.Controls;
using Mainapp.Pages;
namespace Mainapp.Costants
{
    public class AppConstant
    {

        public async static Task AddFlyoutMenusDetails()
        {
            AppShell.Current.FlyoutHeader = new FlyoutHeaderControl();

            var studentDashboardInfo = AppShell.Current.Items.Where(f => f.Route == nameof(DashBoardPage)).FirstOrDefault();
            if (studentDashboardInfo != null) AppShell.Current.Items.Remove(studentDashboardInfo);

            var teacherDashboardInfo = AppShell.Current.Items.Where(f => f.Route == nameof(DashBoardPage)).FirstOrDefault();
            if (teacherDashboardInfo != null) AppShell.Current.Items.Remove(teacherDashboardInfo);

            var adminDashboardInfo = AppShell.Current.Items.Where(f => f.Route == nameof(DashBoardPage)).FirstOrDefault();
            if (adminDashboardInfo != null) AppShell.Current.Items.Remove(adminDashboardInfo);


            if (App.UserDetails.Roleid == (int)RoleDetails.Student)
            {
                var flyoutItem = new FlyoutItem()
                {
                    Title = "Dashboard Page",
                    Route = nameof(DashBoardPage),
                    FlyoutDisplayOptions = FlyoutDisplayOptions.AsMultipleItems,
                    Items =
                            {
                                new ShellContent
                                {
                                    Icon = Icons.Dashboard,
                                    Title = "Dashboard",
                                    ContentTemplate = new DataTemplate(typeof(DashBoardPage)),
                                },
                                new ShellContent
                                {
                                    Icon = Icons.People,
                                    Title = "Bible",
                                    ContentTemplate = new DataTemplate(typeof(DashBoardPage)),
                                },
                                new ShellContent
                                {
                                    Icon = Icons.People,
                                    Title = "Forums",
                                    ContentTemplate = new DataTemplate(typeof(DashBoardPage)),
                                },
                                new ShellContent
                                {
                                    Icon = Icons.People,
                                    Title = "Groups",
                                    ContentTemplate = new DataTemplate(typeof(DashBoardPage)),
                                },
                                new ShellContent
                                {
                                    Icon = Icons.AboutUs,
                                    Title = "Profile",
                                    ContentTemplate = new DataTemplate(typeof(DashBoardPage)),
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
                            await Shell.Current.GoToAsync($"//{nameof(DashBoardPage)}");
                        });
                    }
                    else
                    {
                        await Shell.Current.GoToAsync($"//{nameof(DashBoardPage)}");
                    }
                }

            }

            if (App.UserDetails.Roleid == (int)RoleDetails.Teacher)
            {
                var flyoutItem = new FlyoutItem()
                {
                    Title = "Dashboard Page",
                    Route = nameof(DashBoardPage),
                    FlyoutDisplayOptions = FlyoutDisplayOptions.AsMultipleItems,
                    Items =
                    {
                                new ShellContent
                                {
                                    Icon = Icons.Dashboard,
                                    Title = "Teacher Dashboard",
                                    ContentTemplate = new DataTemplate(typeof(DashBoardPage)),
                                },
                                new ShellContent
                                {
                                    Icon = Icons.AboutUs,
                                    Title = "Teacher Profile",
                                    ContentTemplate = new DataTemplate(typeof(DashBoardPage)),
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
                            await Shell.Current.GoToAsync($"//{nameof(DashBoardPage)}");
                        });
                    }
                    else
                    {
                        await Shell.Current.GoToAsync($"//{nameof(DashBoardPage)}");
                    }
                }
            }

            if (App.UserDetails.Roleid == (int)RoleDetails.Admin)
            {
                var flyoutItem = new FlyoutItem()
                {
                    Title = "Dashboard Page",
                    Route = nameof(DashBoardPage),
                    FlyoutDisplayOptions = FlyoutDisplayOptions.AsMultipleItems,
                    Items =
                    {
                                new ShellContent
                                {
                                    Icon = Icons.Dashboard,
                                    Title = "Admin Dashboard",
                                    ContentTemplate = new DataTemplate(typeof(DashBoardPage)),
                                },
                                new ShellContent
                                {
                                    Icon = Icons.AboutUs,
                                    Title = "Admin Profile",
                                    ContentTemplate = new DataTemplate(typeof(DashBoardPage)),
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
                            await Shell.Current.GoToAsync($"//{nameof(DashBoardPage)}");
                        });
                    }
                    else
                    {
                        await Shell.Current.GoToAsync($"//{nameof(DashBoardPage)}");
                    }
                }
            }
        }
    }
}
