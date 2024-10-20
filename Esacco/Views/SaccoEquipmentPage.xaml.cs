using Esacco.ViewModels;

namespace Esacco.Views;
public partial class SaccoEquipmentPage : ContentPage
{
    private EsaccoManagementViewModel _viewModel;
    public SaccoEquipmentPage(EsaccoManagementViewModel viewModel)
    {
        InitializeComponent();
        Shell.SetBackButtonBehavior(this, new BackButtonBehavior
        {
            IsEnabled = false,
            IsVisible = false
        });
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