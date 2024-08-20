using DBL.Entities;
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
        public ICommand OnOkClickedCommand { get; }

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
            OnOkClickedCommand = new Command(async () => await SaveHouseInvoicePaymentDataAsync());
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

        private string _houseRoomIdError;
        public string HouseRoomIdError
        {
            get => _houseRoomIdError;
            set
            {
                _houseRoomIdError = value;
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
        private async Task SaveHouseInvoicePaymentDataAsync()
        {
            IsProcessing = true;
            if (TenantInvoiceDetailData.Propertyhouseroomid == 0)
            {
                HouseRoomIdError = "Room is required.";
                return;
            }

            if (!ValidatePaymentData())
            {
                IsProcessing = false;
                return;
            }
            try
            {
                CustomerRentInvoicePayment InvoicePaymentData = new CustomerRentInvoicePayment();
                InvoicePaymentData.Tenantid = TenantInvoiceDetailData.Propertyhouseroomtenantid;
                InvoicePaymentData.Houseromid = TenantInvoiceDetailData.Propertyhouseroomid;
                InvoicePaymentData.Paymentmodeid = Convert.ToInt64(SelectedPaymentModes.Value);
                InvoicePaymentData.Amount = 0;
                InvoicePaymentData.Transactionreference = InvoicePayemtCode;
                InvoicePaymentData.Transactiondate = DateTime.UtcNow;
                InvoicePaymentData.Ispaymentvalidated = false;
                InvoicePaymentData.Chequeno = "";
                InvoicePaymentData.Chequedate = DateTime.UtcNow;
                InvoicePaymentData.Memo = "Tenant Validating His Renatal Payments";
                InvoicePaymentData.Drawerbank = "";
                InvoicePaymentData.Depositbank = "";
                InvoicePaymentData.Paidby = App.UserDetails.Usermodel.Userid;
                InvoicePaymentData.Slipreference = InvoicePayemtCode;
                InvoicePaymentData.Datecreated = DateTime.UtcNow;


                var response = await _serviceProvider.CallAuthWebApi<object>("/api/PropertyHouse/Registerpropertyhouseroomdata", HttpMethod.Post, InvoicePaymentData);
                if (response.StatusCode == 200)
                {
                    Application.Current.MainPage.Navigation.PopModalAsync();
                }
                else if (response.StatusCode == 1)
                {
                    await Shell.Current.DisplayAlert("Warning", "Something went wrong. Contact Admin!", "OK");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Sever error occured. Kindly Contact Admin!", "OK");
                }
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

        private bool ValidatePaymentData()
        {
            bool isValid = true;

            // Validate Property Name
            if (string.IsNullOrWhiteSpace(InvoicePayemtCode))
            {
                InvoicePayemtCodeError = "Payment code is required.";
                isValid = false;
            }
            else
            {
                InvoicePayemtCodeError = null;
            }
            if (SelectedPaymentModes == null)
            {
                InvoicePayemtModeError = "Payment mode is required.";
                isValid = false;
            }
            else
            {
                InvoicePayemtModeError = null;
            }
            // Update overall IsValid property
            IsProcessing = isValid;

            return isValid;
        }
    }
}
