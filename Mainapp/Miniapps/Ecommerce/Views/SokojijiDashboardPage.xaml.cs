using Mainapp.Miniapps.Ecommerce.ViewModels;
namespace Mainapp.Miniapps.Ecommerce.Views;

public partial class SokojijiDashboardPage : ContentPage
{
    public SokojijiDashboardPage(SokojijiViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;
    }
}