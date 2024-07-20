using App.ViewModels;

namespace App.Pages;

public partial class DashBoardPage : ContentPage
{
    public DashBoardPage(DashBoardViewModel viewModel)
    {
        InitializeComponent();

        this.BindingContext = viewModel;
    }
}