using Mainapp.ViewModels;
namespace Mainapp.Pages;

public partial class DashBoardPage : ContentPage
{
    public DashBoardPage(DashBoardViewModel viewModel)
    {
        InitializeComponent();

        this.BindingContext = viewModel;
    }
}
