using Esacco.ViewModels;

namespace Esacco.Views;

public partial class SaccoAdministratorPage : ContentPage
{
    private EsaccoManagementViewModel _viewModel;
    public SaccoAdministratorPage(EsaccoManagementViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel.LoadSaccoSummaryDataCommand.CanExecute(null))
        {
            _viewModel.LoadSaccoSummaryDataCommand.Execute(null);
        }
    }
}