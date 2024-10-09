using CommunityToolkit.Maui;
using DBL;
using Maqaoplus.ViewModels;
using Maqaoplus.ViewModels.Agreements;
using Maqaoplus.ViewModels.Dashboards;
using Maqaoplus.ViewModels.HouseTenant;
using Maqaoplus.ViewModels.PropertyHouse;
using Maqaoplus.ViewModels.Startup;
using Maqaoplus.ViewModels.TenantBillsandPayments;
using Maqaoplus.Views;
using Maqaoplus.Views.Agreements;
using Maqaoplus.Views.Dashboards;
using Maqaoplus.Views.PropertyHouse;
using Maqaoplus.Views.PropertyHouseTenants;
using Maqaoplus.Views.Startup;
using Maqaoplus.Views.TenantBillsandPayments;
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
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddSingleton<LoadingPage>();
            builder.Services.AddSingleton<LoginPage>();
            builder.Services.AddTransient<RegisterPage>();
            builder.Services.AddTransient<ForgotPasswordPage>();
            builder.Services.AddTransient<ValidateStaffAccountPage>();
            builder.Services.AddSingleton<DashboardPage>();
            builder.Services.AddSingleton<AdminDashboardPage>();
            builder.Services.AddSingleton<UserDashboardPage>();
            builder.Services.AddSingleton<PropertyOwnerDashboardPage>();
            builder.Services.AddSingleton<PropertyCaretakerDashboardPage>();
            builder.Services.AddSingleton<AgentDashboardPage>();
            builder.Services.AddSingleton<UserProfilePage>();
            builder.Services.AddSingleton<UpdateUserProfilePage>();
            builder.Services.AddSingleton<PropertyHousesPage>();
            builder.Services.AddSingleton<PropertyHousesDetailPage>();
            builder.Services.AddSingleton<AgentPropertyHousesPage>();
            builder.Services.AddSingleton<PropertyHousesTenantDetailPage>();
            builder.Services.AddSingleton<PropertyHousesTenantBillsPage>();
            builder.Services.AddSingleton<PropertyHousesTenantPaymentsPage>();
            builder.Services.AddSingleton<PropertyTenantAgreementsPage>();
            builder.Services.AddSingleton<VacantPropertyHousesPage>();
            builder.Services.AddSingleton<PropertyHousesCaretakerPage>();
            builder.Services.AddSingleton<PropertyAgentAgreementsPage>();
            builder.Services.AddSingleton<PropertyOwnerAgreementsPage>();

            // View Models
            builder.Services.AddSingleton<MainPageViewModel>();
            builder.Services.AddSingleton<LoadingPageViewModel>();
            builder.Services.AddSingleton<LoginPageViewModel>();
            builder.Services.AddSingleton<RegisterPageViewModel>();
            builder.Services.AddSingleton<ForgotPasswordPageViewModel>();
            builder.Services.AddSingleton<ValidateStaffAccountPageViewModel>();
            builder.Services.AddSingleton<DashboardPageViewModel>();
            builder.Services.AddSingleton<UserProfilePageViewModel>();
            builder.Services.AddSingleton<SummaryDashBoardViewModel>();
            builder.Services.AddSingleton<PropertyHouseViewModel>();
            builder.Services.AddSingleton<Propertyhousetenantviewmodel>();
            builder.Services.AddSingleton<PropertyHousesTenantBillsViewModel>();
            builder.Services.AddSingleton<PropertyHousesTenantPaymentsViewModel>();
            builder.Services.AddSingleton<SystemAgreementViewModel>();

            return builder.Build();
        }
    }
}
