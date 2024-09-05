using Maqaoplus.ViewModels.PropertyHouse;

namespace Maqaoplus.Views.PropertyHouse
{
    public partial class PropertyHousesDetailPage : ContentPage
    {
        public PropertyHousesDetailPage(PropertyHouseViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}