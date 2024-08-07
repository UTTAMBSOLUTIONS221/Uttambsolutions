using Maqaoplus.Helpers;
using Maqaoplus.ViewModels;
using Maqaoplus.ViewModels.Dashboards;
using Maqaoplus.ViewModels.HouseTenant;
using Maqaoplus.ViewModels.PropertyHouse;
using Maqaoplus.ViewModels.Startup;
using Maqaoplus.Views.Dashboards;
using Maqaoplus.Views.PropertyHouse;
using Maqaoplus.Views.Startup;

namespace Maqaoplus
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
            builder.Services.AddTransient<RegisterPage>();
            builder.Services.AddSingleton<DashboardPage>();
            builder.Services.AddSingleton<PropertyHousesDetailPage>();
            builder.Services.AddSingleton<PropertyHousesRoomDetailPage>();

            // View Models
            builder.Services.AddSingleton<AppShellViewModel>();
            builder.Services.AddSingleton<LoadingPageViewModel>();
            builder.Services.AddSingleton<LoginPageViewModel>();
            builder.Services.AddSingleton<RegisterPageViewModel>();
            builder.Services.AddSingleton<DashboardPageViewModel>();
            builder.Services.AddSingleton<PropertyHouseViewModel>();
            builder.Services.AddSingleton<PropertyHouseDetailViewModel>();
            builder.Services.AddSingleton<Propertyhousetenantviewmodel>();
            builder.Services.AddSingleton<PropertyHouseRoomDetailViewModel>();

            return builder.Build();
        }
    }
}
