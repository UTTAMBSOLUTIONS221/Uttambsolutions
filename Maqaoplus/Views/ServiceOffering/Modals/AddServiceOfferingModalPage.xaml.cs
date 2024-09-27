using Maqaoplus.ViewModels.ServiceOffering;

namespace Maqaoplus.Views.ServiceOffering.Modals;

public partial class AddServiceOfferingModalPage : ContentPage
{
    public AddServiceOfferingModalPage(ServiceOfferingViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}