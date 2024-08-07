using Maqaoplus.ViewModels.PropertyHouse;

namespace Maqaoplus.Views.PropertyHouse
{
    [QueryProperty(nameof(PropertyRoomId), "PropertyRoomId")]
    public partial class PropertyHousesRoomDetailPage : ContentPage
    {
        public long PropertyRoomId { get; set; }
        private PropertyHouseRoomDetailViewModel _viewModel;

        public PropertyHousesRoomDetailPage()
        {
            InitializeComponent();
            _viewModel = new PropertyHouseRoomDetailViewModel();
            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is PropertyHouseRoomDetailViewModel viewModel)
            {
                viewModel.SetPropertyRoomId(PropertyRoomId);
            }
        }
    }
}
