using Mainapp.Miniapps.Apps.Church.ViewModels;

namespace Mainapp.Miniapps.Apps.Church;

public partial class ChurchDashBoardPage : ContentPage
{
    public ChurchDashBoardPage(ChurchDashBoardViewModel viewModel)
    {
        InitializeComponent();

        this.BindingContext = viewModel;
    }

    private void OnMenuHeaderTapped(object sender, EventArgs e)
    {
        // Toggle the visibility of the menu
        MenuStackLayout.IsVisible = !MenuStackLayout.IsVisible;
    }
}