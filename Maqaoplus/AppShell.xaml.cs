using Maqaoplus.ViewModels;
using Maqaoplus.Views;
using Maqaoplus.Views.PropertyHouse;
using Maqaoplus.Views.PropertyHouseTenants;
using Maqaoplus.Views.Reports;
using Maqaoplus.Views.Startup;
using Maqaoplus.Views.TenantBillsandPayments;
namespace Maqaoplus
{
    public partial class AppShell : Shell
    {
        public AppShell(AppShellViewModel viewModel)
        {
            InitializeComponent();
            this.BindingContext = viewModel;
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
            Routing.RegisterRoute(nameof(ForgotPasswordPage), typeof(ForgotPasswordPage));
            Routing.RegisterRoute(nameof(ValidateStaffAccountPage), typeof(ValidateStaffAccountPage));
            Routing.RegisterRoute(nameof(UserProfilePage), typeof(UserProfilePage));
            Routing.RegisterRoute(nameof(UpdateUserProfilePage), typeof(UpdateUserProfilePage));
            Routing.RegisterRoute(nameof(AddPropertyHousePage), typeof(AddPropertyHousePage));
            Routing.RegisterRoute(nameof(PropertyHousesDetailPage), typeof(PropertyHousesDetailPage));
            Routing.RegisterRoute(nameof(PropertyHousesPage), typeof(PropertyHousesPage));
            Routing.RegisterRoute(nameof(PropertyHousesTenantDetailPage), typeof(PropertyHousesTenantDetailPage));
            Routing.RegisterRoute(nameof(SystemPropertyOwnerReportsPage), typeof(SystemPropertyOwnerReportsPage));
            Routing.RegisterRoute(nameof(PropertyHousesTenantBillsPage), typeof(PropertyHousesTenantBillsPage));
            Routing.RegisterRoute(nameof(PropertyHousesTenantPaymentsPage), typeof(PropertyHousesTenantPaymentsPage));
        }
    }
}
