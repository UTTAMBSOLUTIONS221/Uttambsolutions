using Mainapp.ViewModels;
namespace Mainapp.Views;

public partial class CommonDashboardPage : ContentPage
{
    public CommonDashboardPage(DashboardPageViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;
    }
}