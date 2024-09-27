using Maqaoplus.ViewModels.ServiceOffering;

namespace Maqaoplus.Views.ServiceOffering;

public partial class ServiceOfferingPage : ContentPage
{
    private ServiceOfferingViewModel _viewModel;

    public ServiceOfferingPage(Services.ServiceProvider serviceProvider)
    {
        InitializeComponent();
        _viewModel = new ServiceOfferingViewModel(serviceProvider);
        this.BindingContext = _viewModel;

    }
}