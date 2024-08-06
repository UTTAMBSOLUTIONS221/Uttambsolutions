using Maqaoplus.ViewModels.HouseTenant;

namespace Maqaoplus.Views.Dashboards;
public partial class UserDashboardPage : ContentPage
{
    private Propertyhousetenantviewmodel _viewModel;
    public UserDashboardPage()
    {
        InitializeComponent();
        _viewModel = BindingContext as Propertyhousetenantviewmodel;
        _viewModel.LoadItemsCommand.Execute(null);
    }
}