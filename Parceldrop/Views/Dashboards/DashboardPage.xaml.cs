using Parceldrop.ViewModels.Dashboards;

namespace Parceldrop.Views.Dashboards;

public partial class DashboardPage : ContentPage
{
    public DashboardPage(DashboardPageViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;
    }
}