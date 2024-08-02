using Maqaoplus.ViewModels;
using Maqaoplus.Views.PropertyHouse;
namespace Maqaoplus
{
    public partial class AppShell : Shell
    {
        public AppShell(AppShellViewModel viewModel)
        {
            InitializeComponent();
            this.BindingContext = viewModel;
            Routing.RegisterRoute(nameof(PropertyHousesDetailPage), typeof(PropertyHousesDetailPage));
        }
    }
}
