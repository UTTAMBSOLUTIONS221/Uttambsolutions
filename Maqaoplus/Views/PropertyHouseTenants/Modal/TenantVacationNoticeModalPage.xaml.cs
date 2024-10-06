using Maqaoplus.ViewModels.HouseTenant;

namespace Maqaoplus.Views.PropertyHouseTenants.Modal;


public partial class TenantVacationNoticeModalPage : ContentPage
{
    public TenantVacationNoticeModalPage(Propertyhousetenantviewmodel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}