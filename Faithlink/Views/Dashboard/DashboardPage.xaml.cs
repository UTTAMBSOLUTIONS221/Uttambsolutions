using Faithlink.ViewModels.Dashboard;

namespace Faithlink.Views.Dashboard;

public partial class DashboardPage : ContentPage
{
    public DashboardPage(DashboardPageViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;
    }
}