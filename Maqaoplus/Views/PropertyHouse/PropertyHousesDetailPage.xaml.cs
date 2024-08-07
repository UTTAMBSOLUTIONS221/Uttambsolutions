using Maqaoplus.ViewModels.PropertyHouse;
namespace Maqaoplus.Views.PropertyHouse
{
    [QueryProperty(nameof(PropertyId), "PropertyId")]

    public partial class PropertyHousesDetailPage : ContentPage
    {
        public long PropertyId { get; set; }
        private PropertyHouseDetailViewModel _viewModel;
        public PropertyHousesDetailPage(PropertyHouseDetailViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is PropertyHouseDetailViewModel viewModel)
            {
                viewModel.SetPropertyId(PropertyId);
            }
            // Use PropertyId to load details or set up the page
            System.Diagnostics.Debug.WriteLine($"Property ID received: {PropertyId}");
        }
    }
}
