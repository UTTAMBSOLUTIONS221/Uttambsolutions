namespace Maqaoplus.Views.PropertyHouseAgent
{

    [QueryProperty(nameof(PropertyId), "PropertyId")]

    public partial class PropertyAgentHousesDetailPage : ContentPage
    {
        public long PropertyId { get; set; }
        private PropertyHouseAgentDetailViewModel _viewModel;
        public PropertyAgentHousesDetailPage(Services.ServiceProvider serviceProvider)
        {
            InitializeComponent();
            _viewModel = new PropertyHouseAgentDetailViewModel(serviceProvider);
            this.BindingContext = _viewModel;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is PropertyHouseAgentDetailViewModel viewModel)
            {
                viewModel.SetPropertyId(PropertyId);
            }
        }
    }
}
