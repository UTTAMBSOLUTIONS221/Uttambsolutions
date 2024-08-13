using Maqaoplus.ViewModels.PropertyHouse;
namespace Maqaoplus.Views.PropertyHouse;

[QueryProperty(nameof(PropertyhousetenantId), "PropertyhousetenantId")]
public partial class PropertyHousesTenantDetailPage : ContentPage
{
    public long PropertyhousetenantId { get; set; }
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
            viewModel.SetPropertyHousesTenantId(PropertyhousetenantId);
        }
    }
}