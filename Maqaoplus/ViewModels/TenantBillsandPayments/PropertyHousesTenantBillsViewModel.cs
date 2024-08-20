using DBL.Enum;
using DBL.Models;
using Maqaoplus.Views.TenantBillsandPayments.Modals;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Windows.Input;
namespace Maqaoplus.ViewModels.TenantBillsandPayments
{
    public class PropertyHousesTenantBillsViewModel : BaseViewModel
    {
        private readonly Services.ServiceProvider _serviceProvider;
        public ObservableCollection<MonthlyRentInvoiceModel> Items { get; }
        private MonthlyRentInvoiceModel _tenantInvoiceDetailData;
        public ICommand LoadItemsCommand { get; }
        public ICommand ViewDetailsCommand { get; }
        public ICommand OnCancelClickedCommand { get; }

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

        public MonthlyRentInvoiceModel TenantInvoiceDetailData
        {
            get => _tenantInvoiceDetailData;
            set
            {
                _tenantInvoiceDetailData = value;
                OnPropertyChanged();
            }
        }

        // Parameterless constructor for XAML support
        public PropertyHousesTenantBillsViewModel(Services.ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            Items = new ObservableCollection<MonthlyRentInvoiceModel>();
            LoadItemsCommand = new Command(async () => await LoadItems());
            ViewDetailsCommand = new Command<MonthlyRentInvoiceModel>(async (propertyhouseinvoice) => await ViewDetails(propertyhouseinvoice.Invoiceid));
            OnCancelClickedCommand = new Command(OnCancelClicked);
        }
        private ObservableCollection<ListModel> _systemPaymentModes;
        public ObservableCollection<ListModel> SystemPaymentModes
        {
            get => _systemPaymentModes;
            set
            {
                _systemPaymentModes = value;
                OnPropertyChanged();
            }
        }

        private ListModel _selectedPaymentModes;
        public ListModel SelectedPaymentModes
        {
            get => _selectedPaymentModes;
            set
            {
                _selectedPaymentModes = value;
                OnPropertyChanged();
            }
        }

        private string _InvoicePayemtCode;
        public string InvoicePayemtCode
        {
            get => _InvoicePayemtCode;
            set
            {
                _InvoicePayemtCode = value;
                OnPropertyChanged();
            }
        }

        private string _invoicePayemtModeError;
        public string InvoicePayemtModeError
        {
            get => _invoicePayemtModeError;
            set
            {
                _invoicePayemtModeError = value;
                OnPropertyChanged();
            }
        }

        private string _invoicePayemtCodeError;
        public string InvoicePayemtCodeError
        {
            get => _invoicePayemtCodeError;
            set
            {
                _invoicePayemtCodeError = value;
                OnPropertyChanged();
            }
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
                var response = await _serviceProvider.CallAuthWebApi<object>($"/api/PropertyHouse/Gettenantmonthlyinvoicedetaildatabyinvoiceid/" + Invoiceid, HttpMethod.Get, null);
                if (response != null && response.Data != null)
                {
                    TenantInvoiceDetailData = JsonConvert.DeserializeObject<MonthlyRentInvoiceModel>(response.Data.ToString());
                    var SystemPaymentModeResponse = await _serviceProvider.GetSystemDropDownData("/api/General?listType=" + ListModelType.Systemcashpaymentmodetype, HttpMethod.Get);
                    if (SystemPaymentModeResponse != null)
                    {
                        SystemPaymentModes = new ObservableCollection<ListModel>(SystemPaymentModeResponse);
                    }
                    var modalPage = new HousesRoomTenantInvoiceDetailModalPage(this);
                    await Application.Current.MainPage.Navigation.PushModalAsync(modalPage);
                    IsDataLoaded = true;
                }
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
        private void OnCancelClicked()
        {
            Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}
