using App.Helpers;
using App.Pages;
using App.Pages.Users;
using App.ViewModels;
using App.ViewModels.User;
using Microsoft.Extensions.Logging;

namespace App
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            string apiUrl = "https://mainapi.uttambsolutions.com";
            builder.Services.AddSingleton(new DevHttpConnectionHelper(apiUrl));

            // Register services
            builder.Services.AddSingleton<Services.ServiceProvider>();
            builder.Services.AddSingleton<AppShell>();

            // Register pages
            builder.Services.AddSingleton<LoginPage>();
            builder.Services.AddSingleton<DashBoardPage>();

            // Register view models
            builder.Services.AddSingleton<LoginPageViewModel>();
            builder.Services.AddSingleton<DashBoardViewModel>();

            return builder.Build();
        }
    }
}
