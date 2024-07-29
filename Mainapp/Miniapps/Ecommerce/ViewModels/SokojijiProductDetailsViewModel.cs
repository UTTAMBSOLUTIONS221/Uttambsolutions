using DBL.Models;
using Mainapp.ViewModels;

namespace Mainapp.Miniapps.Ecommerce.ViewModels
{
    public class SokojijiProductDetailsViewModel : BaseViewModel
    {
        private Organizationshopproductsdata _product;

        public Organizationshopproductsdata Product
        {
            get => _product;
            set => SetProperty(ref _product, value);
        }

        public SokojijiProductDetailsViewModel()
        {
        }

        public SokojijiProductDetailsViewModel(Organizationshopproductsdata product)
        {
            Product = product;
        }
    }
}
