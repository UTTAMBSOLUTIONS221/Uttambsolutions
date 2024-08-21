using Maqaoplus.ViewModels.OwnerBillsandPayments;
namespace Maqaoplus.Views.OwnerBillsandPayments;
public partial class PropertyHousesOwnerBillsPage : ContentPage
{
    private PropertyHousesOwnerBillsViewModel _viewModel;

    public PropertyHousesOwnerBillsPage(Services.ServiceProvider serviceProvider)
    {
        InitializeComponent();
        _viewModel = new PropertyHousesOwnerBillsViewModel(serviceProvider);
        this.BindingContext = _viewModel;

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel.LoadItemsCommand.CanExecute(null))
        {
            _viewModel.LoadItemsCommand.Execute(null);
        }
    }
}