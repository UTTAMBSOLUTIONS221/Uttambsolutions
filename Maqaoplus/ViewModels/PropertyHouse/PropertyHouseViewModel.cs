﻿using DBL.Models;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Maqaoplus.ViewModels.PropertyHouse
{
    public class PropertyHouseViewModel : BaseViewModel
    {
        private readonly Services.ServiceProvider _serviceProvider;
        public ObservableCollection<Organizationshopproductsdata> Items { get; }

        public ICommand LoadItemsCommand { get; }
        public ICommand ViewDetailsCommand { get; }

        // Parameterless constructor for XAML support
        public PropertyHouseViewModel()
        {
            Items = new ObservableCollection<Organizationshopproductsdata>();
            LoadItemsCommand = new Command(async () => await LoadItems());
            ViewDetailsCommand = new Command<Organizationshopproductsdata>(async (item) => await ViewDetails(item));
        }

        // Constructor with ServiceProvider parameter
        public PropertyHouseViewModel(Services.ServiceProvider serviceProvider) : this()
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
            try
            {
                var jsonProduct = JsonConvert.SerializeObject(item);
                var encodedProduct = Uri.EscapeDataString(jsonProduct);
                await Shell.Current.GoToAsync($"SokojijiProductDetailsPage?Product={encodedProduct}");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Navigation Error", ex.Message, "OK");
            }
        }
    }
}
