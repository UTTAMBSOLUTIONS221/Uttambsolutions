﻿using DBL;
using Microsoft.Extensions.Logging;

namespace Maqaoplus
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
            string connectionString = environment == "Development" ? "Data Source=SQL6032.site4now.net;Initial Catalog=db_aaa347_jcmchurchdev;user id=db_aaa347_jcmchurchdev_admin;password=Password123!;" : "Data Source=SQL6032.site4now.net;Initial Catalog=db_aaa347_jcmchurch;user id=db_aaa347_jcmchurch_admin;password=Password123!;";
            builder.Services.AddSingleton<BL>(sp => new BL(connectionString));

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddTransient<MainPage>();



            return builder.Build();
        }
    }
}
