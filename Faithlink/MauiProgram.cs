using Faithlink.Services;
using Faithlink.ViewModels;
using Faithlink.ViewModels.Dashboard;
using Faithlink.ViewModels.Startup;
using Faithlink.Views.Dashboard;
using Faithlink.Views.OpenForums;
using Faithlink.Views.Startup;

namespace Faithlink
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

            // Views
            builder.Services.AddSingleton<LoginPage>();
            builder.Services.AddSingleton<DashboardPage>();
            builder.Services.AddSingleton<LoadingPage>();
            builder.Services.AddTransient<OpenForumsPage>();

            // View Models
            builder.Services.AddSingleton<LoginPageViewModel>();
            builder.Services.AddSingleton<DashboardPageViewModel>();
            builder.Services.AddSingleton<LoadingPageViewModel>();
            builder.Services.AddSingleton<OpenForumsViewModel>();

            // Services
            builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();
            builder.Services.AddSingleton<IOpenForumsApiService, OpenForumsApiService>();
            builder.Services.AddSingleton<IBibleApiService, BibleApiService>();


            return builder.Build();
        }
    }
}
