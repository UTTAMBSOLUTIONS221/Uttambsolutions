using Maqaoplus.ViewModels.Dashboards;

namespace Maqaoplus.Views.Dashboards;

public partial class CaretakerDashboardPage : ContentPage
{
    public CaretakerDashboardPage(SummaryDashBoardViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is SummaryDashBoardViewModel viewModel && viewModel.LoadOwnerSummaryCommand.CanExecute(null))
        {
            viewModel.LoadOwnerSummaryCommand.Execute(null);
        }
    }
}