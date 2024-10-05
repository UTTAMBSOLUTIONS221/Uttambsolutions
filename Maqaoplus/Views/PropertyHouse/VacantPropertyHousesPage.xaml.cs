using Maqaoplus.ViewModels.PropertyHouse;

namespace Maqaoplus.Views.PropertyHouse;

public partial class VacantPropertyHousesPage : ContentPage
{
    private PropertyHouseViewModel _viewModel;
    public VacantPropertyHousesPage(PropertyHouseViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel.LoadVacantPropertyHousesCommand.CanExecute(null))
        {
            _viewModel.LoadVacantPropertyHousesCommand.Execute(null);
        }
    }
}
