using Maqaoplus.ViewModels.PropertyHouse;

namespace Maqaoplus.Views.PropertyHouse;

public partial class VacantPropertyHousesPage : ContentPage
{
    private PropertyHouseViewModel _viewModel;

    public VacantPropertyHousesPage(Services.ServiceProvider serviceProvider)
    {
        InitializeComponent();
        _viewModel = new PropertyHouseViewModel(serviceProvider);
        this.BindingContext = _viewModel;

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
