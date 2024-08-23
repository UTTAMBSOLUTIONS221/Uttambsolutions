using Maqaoplus.ViewModels.PropertyHouse;

namespace Maqaoplus.Views.PropertyHouse
{
    [QueryProperty(nameof(PropertyId), "PropertyId")]

    public partial class PropertyHousesDetailPage : ContentPage
    {
        public long PropertyId { get; set; }
        private PropertyHouseDetailViewModel _viewModel;
        public PropertyHousesDetailPage(Services.ServiceProvider serviceProvider)
        {
            InitializeComponent();
            _viewModel = new PropertyHouseDetailViewModel(serviceProvider);
            this.BindingContext = _viewModel;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is PropertyHouseDetailViewModel viewModel)
            {
                viewModel.SetPropertyId(PropertyId);
            }
        }
    }
}