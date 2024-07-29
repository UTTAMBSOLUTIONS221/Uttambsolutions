using DBL.Models;
using Mainapp.Miniapps.Ecommerce.Views;
using Mainapp.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Mainapp.Miniapps.Ecommerce.ViewModels
{
    public class SokojijiViewModel : BaseViewModel
    {
        private readonly Services.ServiceProvider _serviceProvider;
        public ObservableCollection<Organizationshopproductsdata> Items { get; }

        public ICommand LoadItemsCommand { get; }
        public ICommand ViewDetailsCommand { get; }
        public ICommand AddToCartCommand { get; }

        // Parameterless constructor for XAML support
        public SokojijiViewModel()
        {
            Items = new ObservableCollection<Organizationshopproductsdata>();
            LoadItemsCommand = new Command(async () => await LoadItems());
            ViewDetailsCommand = new Command<Organizationshopproductsdata>(async (item) => await ViewDetails(item));
            AddToCartCommand = new Command<Organizationshopproductsdata>(async (item) => await AddToCart(item));
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
                        var product = item.ToObject<Organizationshopproductsdata>(); // Assuming item is of type JObject
                        Items.Add(product);
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
        private async Task ViewDetails(Organizationshopproductsdata item)
        {
            var navigationParameter = new Dictionary<string, object>
            {
                { "Product", item }
            };

            await Shell.Current.GoToAsync(nameof(SokojijiProductDetailsPage), navigationParameter);
        }

        private async Task AddToCart(Organizationshopproductsdata item)
        {
            //_cartService.AddToCart(item);
            await Application.Current.MainPage.DisplayAlert("Added to Cart", $"{item.Productname} has been added to your cart.", "OK");
        }

        private async Task NavigateToCart()
        {
            // Navigate to the Cart page
            await Shell.Current.GoToAsync("CartPage");
        }
    }
}
