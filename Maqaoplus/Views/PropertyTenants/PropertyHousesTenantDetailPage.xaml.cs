using Maqaoplus.ViewModels.PropertyHouse;
namespace Maqaoplus.Views.PropertyHouse;

[QueryProperty(nameof(Propertyhousetenantidnumber), "Propertyhousetenantidnumber")]
public partial class PropertyHousesTenantDetailPage : ContentPage
{
    public long Propertyhousetenantidnumber { get; set; }
    private PropertyHousesTenantDetailViewModel _viewModel;

    public PropertyHousesTenantDetailPage(PropertyHousesTenantDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is PropertyHousesTenantDetailViewModel viewModel)
        {
            viewModel.SetPropertyHousesTenantIdNumber(Propertyhousetenantidnumber);
        }
    }
}