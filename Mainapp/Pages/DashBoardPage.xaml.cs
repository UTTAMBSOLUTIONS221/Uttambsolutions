using Mainapp.ViewModels;
namespace Mainapp.Pages;

public partial class DashBoardPage : ContentPage
{
    public DashBoardPage()
    {
        InitializeComponent();

        // Pass INavigation to the ViewModel
        BindingContext = new DashBoardViewModel(Navigation);
    }
}
