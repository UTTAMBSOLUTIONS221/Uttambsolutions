using Maqaoplus.ViewModels.PropertyHouse;

namespace Maqaoplus.Views.PropertyHouse.Modal;

public partial class SystemPropertyHouseAgreementSignatureModalPage : ContentPage
{
    public SystemPropertyHouseAgreementSignatureModalPage(PropertyHouseViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}