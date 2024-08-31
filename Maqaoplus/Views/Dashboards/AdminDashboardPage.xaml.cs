using Maqaoplus.ViewModels.Dashboards;

namespace Maqaoplus.Views.Dashboards;

public partial class AdminDashboardPage : ContentPage
{
    public AdminDashboardPage(SummaryDashBoardViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is SummaryDashBoardViewModel viewModel && viewModel.LoadItemsCommand.CanExecute(null))
        {
            viewModel.LoadItemsCommand.Execute(null);
        }
    }
}