using Maqaoplus.ViewModels.HouseTenant;

namespace Maqaoplus.Views.Dashboards;
public partial class UserDashboardPage : ContentPage
{
    public UserDashboardPage(Propertyhousetenantviewmodel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is Propertyhousetenantviewmodel viewModel && viewModel.LoadItemsCommand.CanExecute(null))
        {
            viewModel.LoadItemsCommand.Execute(null);
        }
    }
}