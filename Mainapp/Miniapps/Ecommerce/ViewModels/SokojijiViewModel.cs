using DBL.Models;
using Mainapp.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Mainapp.Miniapps.Ecommerce.ViewModels
{
    public class SokojijiViewModel : BaseViewModel
    {
        private readonly Services.ServiceProvider _serviceProvider;
        public ObservableCollection<Systemorganizationshopproducts> Items { get; }

        public ICommand LoadItemsCommand { get; }

        public SokojijiViewModel(Services.ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            Items = new ObservableCollection<Systemorganizationshopproducts>();
            LoadItemsCommand = new Command(async () => await LoadItems());
            LoadItemsCommand.Execute(null);
        }

        private async Task LoadItems()
        {
            IsBusy = true;

            try
            {
                var response = await _serviceProvider.CallUnAuthWebApi<object, BaseResponse<Systemorganizationshopproducts>>("/api/Ecommerce/Getsystemorganizationshopproductsdata", HttpMethod.Get, null);
                //if (response != null && response.Data != null)
                //{
                //    foreach (var item in response.Data.Data)
                //    {
                //        Items.Add(item);
                //    }
                //}
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
    }
}
