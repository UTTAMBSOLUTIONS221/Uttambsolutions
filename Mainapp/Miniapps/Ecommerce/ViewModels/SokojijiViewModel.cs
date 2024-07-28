using Mainapp.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Mainapp.Miniapps.Ecommerce.ViewModels
{
    public class SokojijiViewModel : BaseViewModel
    {
        private readonly Services.ServiceProvider _serviceProvider;
        public ObservableCollection<dynamic> Items { get; }

        public ICommand LoadItemsCommand { get; }
        public ICommand ViewDetailsCommand { get; }
        public ICommand AddToCartCommand { get; }

        // Parameterless constructor for XAML support
        public SokojijiViewModel()
        {
            Items = new ObservableCollection<dynamic>();
            LoadItemsCommand = new Command(async () => await LoadItems());
            ViewDetailsCommand = new Command<dynamic>(async (item) => await ViewDetails(item));
            AddToCartCommand = new Command<dynamic>(async (item) => await AddToCart(item));
        }

        // Constructor with ServiceProvider parameter
        public SokojijiViewModel(Services.ServiceProvider serviceProvider) : this()
        {
            _serviceProvider = serviceProvider;
            LoadItemsCommand.Execute(null);
        }

        private async Task LoadItems()
        {
            IsBusy = true;

            try
            {
                var response = await _serviceProvider.CallUnAuthWebApi<object>("/api/Ecommerce/Getsystemorganizationshopproductsdata", HttpMethod.Get, null);
                if (response != null && response.Data is List<dynamic> items)
                {
                    Items.Clear();
                    foreach (var item in items)
                    {
                        Items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task ViewDetails(dynamic item)
        {
            // Navigate to a details page or show a modal with item details
            await Application.Current.MainPage.DisplayAlert("Product Details", item.Productdescription, "OK");
        }

        private async Task AddToCart(dynamic item)
        {
            // Logic to add the item to the cart
            await Application.Current.MainPage.DisplayAlert("Added to Cart", $"{item.Productname} has been added to your cart.", "OK");
        }
    }
}
