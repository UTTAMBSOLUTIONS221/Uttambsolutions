using Maqaoplus.ViewModels;
using Maqaoplus.Views;
using Maqaoplus.Views.PropertyHouse;
using Maqaoplus.Views.Startup;
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
            Routing.RegisterRoute(nameof(PropertyHousesDetailPage), typeof(PropertyHousesDetailPage));
            Routing.RegisterRoute(nameof(PropertyHousesRoomDetailPage), typeof(PropertyHousesRoomDetailPage));
        }
    }
}
