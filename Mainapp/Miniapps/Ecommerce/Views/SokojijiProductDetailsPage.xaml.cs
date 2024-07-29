using Mainapp.Miniapps.Ecommerce.ViewModels;

namespace Mainapp.Miniapps.Ecommerce.Views
{
    public partial class SokojijiProductDetailsPage : ContentPage
    {
        public SokojijiProductDetailsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is SokojijiProductDetailsViewModel viewModel)
            {
                if (Shell.Current.CurrentPage?.BindingContext is SokojijiProductDetailsViewModel currentPageViewModel)
                {
                    var productParameter = currentPageViewModel.Product;
                    if (productParameter != null)
                    {
                        viewModel.Product = productParameter;
                    }
                }
            }
        }
    }
}
