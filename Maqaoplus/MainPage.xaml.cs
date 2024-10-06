using Maqaoplus.ViewModels.PropertyHouse;

namespace Maqaoplus
{
    public partial class MainPage : ContentPage
    {
        private PropertyHouseViewModel _viewModel;
        public MainPage(PropertyHouseViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = viewModel;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (_viewModel.LoadVacantPropertyHousesCommand.CanExecute(null))
            {
                _viewModel.LoadVacantPropertyHousesCommand.Execute(null);
            }
        }
    }

}
