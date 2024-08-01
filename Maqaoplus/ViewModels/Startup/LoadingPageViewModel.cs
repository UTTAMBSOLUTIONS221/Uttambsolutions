﻿using DBL.Models;
using Maqaoplus.Views.Startup;
using Newtonsoft.Json;
namespace Maqaoplus.ViewModels.Startup
{
    public class LoadingPageViewModel
    {
        public LoadingPageViewModel()
        {
            CheckUserLoginDetails();
        }
        private async void CheckUserLoginDetails()
        {
            string userDetailsStr = Preferences.Get(nameof(App.UserDetails), "");

            if (string.IsNullOrWhiteSpace(userDetailsStr))
            {
                if (DeviceInfo.Platform == DevicePlatform.WinUI)
                {
                    AppShell.Current.Dispatcher.Dispatch(async () =>
                    {
                        await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                    });
                }
                else
                {
                    await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                }
                // navigate to Login Page
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UsermodelResponce>(userDetailsStr);
                App.UserDetails = userInfo.Usermodel;
                await AppConstant.AddFlyoutMenusDetails(nameof(CommonDashboardPage));

                if (DeviceInfo.Platform == DevicePlatform.WinUI)
                {
                    AppShell.Current.Dispatcher.Dispatch(async () =>
                    {
                        await Shell.Current.GoToAsync($"//{nameof(CommonDashboardPage)}");
                    });
                }
                else
                {
                    await Shell.Current.GoToAsync($"//{nameof(CommonDashboardPage)}");
                }
            }
        }
    }
}
