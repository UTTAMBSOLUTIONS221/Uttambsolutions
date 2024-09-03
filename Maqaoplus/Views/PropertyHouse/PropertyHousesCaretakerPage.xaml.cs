using Maqaoplus.ViewModels.PropertyHouse;

namespace Maqaoplus.Views.PropertyHouse;

public partial class PropertyHousesCaretakerPage : ContentPage
{
    private PropertyHouseViewModel _viewModel;

    public PropertyHousesCaretakerPage(Services.ServiceProvider serviceProvider)
    {
        InitializeComponent();
        _viewModel = new PropertyHouseViewModel(serviceProvider);
        this.BindingContext = _viewModel;

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel.LoadPropertyHouseCaretakerItemsCommand.CanExecute(null))
        {
            _viewModel.LoadPropertyHouseCaretakerItemsCommand.Execute(null);
        }
    }
}
