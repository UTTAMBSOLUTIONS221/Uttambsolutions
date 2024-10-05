using Maqaoplus.ViewModels.PropertyHouse;

namespace Maqaoplus.Views.PropertyHouse;

public partial class PropertyHousesCaretakerPage : ContentPage
{
    private PropertyHouseViewModel _viewModel;

    public PropertyHousesCaretakerPage()
    {
        InitializeComponent();

        _viewModel = new PropertyHouseViewModel();

        BindingContext = _viewModel;
    }

    public PropertyHousesCaretakerPage(PropertyHouseViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
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

