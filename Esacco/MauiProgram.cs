using CommunityToolkit.Maui;
using DBL;
using Esacco.ViewModels;
using Microsoft.Extensions.Logging;

namespace Esacco
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


            builder.Services.AddSingleton<LoginPage>();


            builder.Services.AddSingleton<UserManagementViewModel>();

            return builder.Build();
        }
    }
}
