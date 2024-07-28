using Mainapp.Miniapps.Ecommerce.Views;
using Mainapp.ViewModels;
using Mainapp.Views;

namespace Mainapp
{
    public partial class AppShell : Shell
    {
        public AppShell(AppShellViewModel viewModel)
        {
            InitializeComponent();
            this.BindingContext = viewModel;

            Routing.RegisterRoute(nameof(CommonDashboardPage), typeof(CommonDashboardPage));
            Routing.RegisterRoute(nameof(SokojijiDashboardPage), typeof(SokojijiDashboardPage));
        }
    }
}

