using Maqaoplus.ViewModels.Dashboards;

namespace Maqaoplus.Views.Dashboards;

public partial class DashboardPage : ContentPage
{
    public DashboardPage(DashboardPageViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;
    }
}