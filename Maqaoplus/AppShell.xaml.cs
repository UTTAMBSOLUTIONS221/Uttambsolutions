using Maqaoplus.ViewModels;
using Maqaoplus.Views;
using Maqaoplus.Views.Agreements;
using Maqaoplus.Views.PropertyHouse;
using Maqaoplus.Views.PropertyHouseTenantAgreement;
using Maqaoplus.Views.PropertyHouseTenants;
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
            Routing.RegisterRoute(nameof(PropertyHousesPage), typeof(PropertyHousesPage));
            Routing.RegisterRoute(nameof(PropertyHousesDetailPage), typeof(PropertyHousesDetailPage));
            Routing.RegisterRoute(nameof(AgentPropertyHousesPage), typeof(AgentPropertyHousesPage));
            Routing.RegisterRoute(nameof(PropertyHousesTenantDetailPage), typeof(PropertyHousesTenantDetailPage));
            Routing.RegisterRoute(nameof(PropertyHousesTenantBillsPage), typeof(PropertyHousesTenantBillsPage));
            Routing.RegisterRoute(nameof(PropertyHousesTenantPaymentsPage), typeof(PropertyHousesTenantPaymentsPage));
            Routing.RegisterRoute(nameof(PropertyHousesTenantAgreementsPage), typeof(PropertyHousesTenantAgreementsPage));
            Routing.RegisterRoute(nameof(VacantPropertyHousesPage), typeof(VacantPropertyHousesPage));
            Routing.RegisterRoute(nameof(PropertyHousesCaretakerPage), typeof(PropertyHousesCaretakerPage));
            Routing.RegisterRoute(nameof(PropertyAgentAgreementsPage), typeof(PropertyAgentAgreementsPage));
        }
    }
}
