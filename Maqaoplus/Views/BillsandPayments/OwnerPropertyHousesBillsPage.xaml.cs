using Maqaoplus.ViewModels.BillsandPayments;

namespace Maqaoplus.Views.BillsandPayments;

public partial class OwnerPropertyHousesBillsPage : ContentPage
{
    private PropertyHousesBillsandPaymentsViewModel _viewModel;

    public OwnerPropertyHousesBillsPage(Services.ServiceProvider serviceProvider)
    {
        InitializeComponent();
        _viewModel = new PropertyHousesBillsandPaymentsViewModel(serviceProvider);
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