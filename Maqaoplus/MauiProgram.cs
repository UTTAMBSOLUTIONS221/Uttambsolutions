using CommunityToolkit.Maui;
using Maqaoplus.Helpers;
using Maqaoplus.ViewModels;
using Maqaoplus.ViewModels.Dashboards;
using Maqaoplus.ViewModels.HouseTenant;
using Maqaoplus.ViewModels.PropertyHouse;
using Maqaoplus.ViewModels.PropertyHouseTenantAgreement;
using Maqaoplus.ViewModels.Reports;
using Maqaoplus.ViewModels.Startup;
using Maqaoplus.ViewModels.TenantBillsandPayments;
using Maqaoplus.Views;
using Maqaoplus.Views.Dashboards;
using Maqaoplus.Views.PropertyHouse;
using Maqaoplus.Views.PropertyHouseTenantAgreement;
using Maqaoplus.Views.PropertyHouseTenants;
using Maqaoplus.Views.Startup;
using Maqaoplus.Views.TenantBillsandPayments;

namespace Maqaoplus
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>().UseMauiCommunityToolkit()
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
            builder.Services.AddSingleton<PropertyHousesTenantAgreementsPage>();

            // View Models
            builder.Services.AddSingleton<AppShellViewModel>();
            builder.Services.AddSingleton<LoadingPageViewModel>();
            builder.Services.AddSingleton<LoginPageViewModel>();
            builder.Services.AddSingleton<RegisterPageViewModel>();
            builder.Services.AddSingleton<ForgotPasswordPageViewModel>();
            builder.Services.AddSingleton<ValidateStaffAccountPageViewModel>();
            builder.Services.AddSingleton<DashboardPageViewModel>();
            builder.Services.AddSingleton<UserProfilePageViewModel>();
            builder.Services.AddSingleton<SummaryDashBoardViewModel>();
            builder.Services.AddSingleton<PropertyHouseViewModel>();
            builder.Services.AddSingleton<PropertyHouseDetailViewModel>();
            builder.Services.AddSingleton<Propertyhousetenantviewmodel>();
            builder.Services.AddSingleton<SystemReportsViewModel>();
            builder.Services.AddSingleton<PropertyHousesTenantBillsViewModel>();
            builder.Services.AddSingleton<PropertyHousesTenantPaymentsViewModel>();
            builder.Services.AddSingleton<PropertyHouseTenantAgreementViewModel>();


            return builder.Build();
        }
    }
}
