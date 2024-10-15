using CommunityToolkit.Maui;
using DBL;
using Microsoft.Extensions.Logging;
using Parceldrop.ViewModels;
using Parceldrop.ViewModels.Parceldrop;
using Parceldrop.ViewModels.Startup;
using Parceldrop.Views;
using Parceldrop.Views.Startup;

namespace Parceldrop
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            // Determine environment based on conditional compilation
            string environment = "Production";
#if DEBUG
            environment = "Development";
#endif

            // Use the environment to set up logging or other services
            if (environment == "Development")
            {
                builder.Logging.AddDebug();
            }

            // Set up connection string or other services based on environment
            string connectionString = environment == "Development" ? "Data Source=sql6032.site4now.net;Initial Catalog=db_aaa347_uttambsolutionsdev;user id=db_aaa347_uttambsolutionsdev_admin;password=Password123!;" : "Data Source=SQL6030.site4now.net;Initial Catalog=db_aaa347_uttambsolutions;user id=db_aaa347_uttambsolutions_admin;password=Password123!;";
            builder.Services.AddSingleton<BL>(sp => new BL(connectionString));
            builder
              .UseMauiApp<App>().UseMauiCommunityToolkit()
              .ConfigureFonts(fonts =>
              {
                  fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                  fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
              });

            builder.Services.AddTransient<MainPage>();
            builder.Services.AddSingleton<LoginPage>();
            builder.Services.AddSingleton<RegisterPage>();
            builder.Services.AddTransient<ValidateStaffAccountPage>();
            builder.Services.AddSingleton<UserProfilePage>();
            builder.Services.AddSingleton<UpdateUserProfilePage>();


            // View Models
            builder.Services.AddSingleton<MainPageViewModel>();
            builder.Services.AddSingleton<LoginPageViewModel>();
            builder.Services.AddSingleton<ParcelDropViewModel>();

            return builder.Build();
        }
    }
}
