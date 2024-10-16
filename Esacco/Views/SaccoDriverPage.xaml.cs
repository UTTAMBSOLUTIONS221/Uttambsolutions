using Esacco.ViewModels;

namespace Esacco.Views;
public partial class SaccoDriverPage : ContentPage
{
    private EsaccoManagementViewModel _viewModel;
    public SaccoDriverPage(EsaccoManagementViewModel viewModel)
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