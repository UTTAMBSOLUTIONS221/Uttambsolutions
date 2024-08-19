using DBL.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Maqaoplus.ViewModels.TenantBillsandPayments
{
    public class PropertyHousesTenantBillsViewModel : BaseViewModel
    {
        private readonly Services.ServiceProvider _serviceProvider;
        public ObservableCollection<MonthlyRentInvoiceModel> Items { get; }
        public ICommand LoadItemsCommand { get; }
        public ICommand ViewDetailsCommand { get; }

        private bool _isProcessing;
        public bool IsProcessing
        {
            get => _isProcessing;
            set
            {
                _isProcessing = value;
                OnPropertyChanged(nameof(IsProcessing));
            }
        }

        private bool _isDataLoaded;
        public bool IsDataLoaded
        {
            get => _isDataLoaded;
            set
            {
                _isDataLoaded = value;
                OnPropertyChanged();
            }
        }

        // Parameterless constructor for XAML support
        public PropertyHousesTenantBillsViewModel()
        {
            Items = new ObservableCollection<MonthlyRentInvoiceModel>();
            LoadItemsCommand = new Command(async () => await LoadItems());
            ViewDetailsCommand = new Command<MonthlyRentInvoiceModel>(async (propertyhouseinvoice) => await ViewDetails(propertyhouseinvoice.Invoiceid));
        }

        // Constructor with ServiceProvider parameter
        public PropertyHousesTenantBillsViewModel(Services.ServiceProvider serviceProvider) : this()
        {
            _serviceProvider = serviceProvider;
        }
        private async Task LoadItems()
        {
            IsProcessing = true;
            IsDataLoaded = false;

            try
            {
                var response = await _serviceProvider.CallAuthWebApi<object>("/api/PropertyHouse/Gettenantmonthlyinvoicedatabytenantid/" + App.UserDetails.Usermodel.Userid, HttpMethod.Get, null);
                if (response != null && response.Data is List<dynamic> items)
                {
                    Items.Clear();
                    foreach (var item in items)
                    {
                        var product = item.ToObject<MonthlyRentInvoiceModel>();
                        Items.Add(product);
                    }
                }
                IsDataLoaded = true;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsProcessing = false;
            }
        }
        private async Task ViewDetails(long Invoiceid)
        {
            IsProcessing = true;
            IsDataLoaded = false;
            try
            {
                var encodedPropertyInvoiceid = Uri.EscapeDataString(Invoiceid.ToString());
                await Shell.Current.GoToAsync($"PropertyHousesTenantDetailPage?Propertyhousetenantidnumber={encodedPropertyInvoiceid}");
                IsDataLoaded = true;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Navigation Error", ex.Message, "OK");
            }
            finally
            {
                IsProcessing = false;
            }
        }
    }
}
