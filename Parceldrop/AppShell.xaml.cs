using Parceldrop.Views;
using Parceldrop.Views.Startup;

namespace Parceldrop
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
            Routing.RegisterRoute(nameof(ValidateStaffAccountPage), typeof(ValidateStaffAccountPage));
            Routing.RegisterRoute(nameof(UserProfilePage), typeof(UserProfilePage));
            Routing.RegisterRoute(nameof(UpdateUserProfilePage), typeof(UpdateUserProfilePage));
        }
    }
}
