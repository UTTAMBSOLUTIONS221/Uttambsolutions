using Mainapp.Helpers;
using Mainapp.Miniapps.Ecommerce.ViewModels;
using Mainapp.Miniapps.Ecommerce.Views;
using Mainapp.ViewModels;
using Mainapp.ViewModels.Startup;
using Mainapp.Views;
using Mainapp.Views.Startup;

namespace Mainapp
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

            string apiUrl = "https://mainapi.uttambsolutions.com";
            builder.Services.AddSingleton(new DevHttpConnectionHelper(apiUrl));
            // Services
            builder.Services.AddSingleton<Services.ServiceProvider>();

            // Register services
            builder.Services.AddSingleton<AppShell>();
            // Views
            builder.Services.AddSingleton<LoadingPage>();
            builder.Services.AddSingleton<LoginPage>();
            builder.Services.AddSingleton<CommonDashboardPage>();
            builder.Services.AddSingleton<SokojijiDashboardPage>();
            builder.Services.AddSingleton<SokojijiProductDetailsPage>();

            // View Models
            builder.Services.AddSingleton<AppShellViewModel>();
            builder.Services.AddSingleton<LoadingPageViewModel>();
            builder.Services.AddSingleton<LoginPageViewModel>();
            builder.Services.AddSingleton<DashboardPageViewModel>();
            builder.Services.AddSingleton<SokojijiViewModel>();
            builder.Services.AddSingleton<SokojijiProductDetailsViewModel>();




            return builder.Build();
        }
    }
}

