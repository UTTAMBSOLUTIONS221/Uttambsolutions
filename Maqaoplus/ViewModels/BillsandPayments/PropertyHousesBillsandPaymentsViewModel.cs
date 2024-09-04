using DBL.Entities;
using DBL.Models;
using Maqaoplus.Views.BillsandPayments.Modals;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Maqaoplus.ViewModels.BillsandPayments
{
    public class PropertyHousesBillsandPaymentsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly Services.ServiceProvider _serviceProvider;
        public string CopyrightText => $"© 2020 - {DateTime.Now.Year}  UTTAMB SOLUTIONS LIMITED";
        public ObservableCollection<MonthlyRentInvoiceModel> Items { get; }
        public ObservableCollection<CustomerPaymentData> PaymentItems { get; }
        private MonthlyRentInvoiceModel _tenantInvoiceDetailData;
        private CustomerPaymentValidation _customerPaymentValidationData;

        public ICommand LoadOwnerBillItemsCommand { get; }
        public ICommand LoadAgentBillItemsCommand { get; }
        public ICommand LoadOwnerPaymentItemsCommand { get; }
        public ICommand LoadAgentPaymentItemsCommand { get; }
        public ICommand ViewDetailsCommand { get; }
        public ICommand OnCancelClickedCommand { get; }
        public ICommand OnOkClickedCommand { get; }
        public ICommand ViewPaymentDetailsCommand { get; }
        public ICommand ValidateThisPaymentCommand { get; }
        public ICommand UpdatePaymentValidationCommand { get; }

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
        public CustomerPaymentValidation CustomerPaymentValidationData
        {
            get => _customerPaymentValidationData;
            set
            {
                _customerPaymentValidationData = value;
                OnPropertyChanged();
            }
        }

        // Parameterless constructor for XAML support
        public PropertyHousesBillsandPaymentsViewModel(Services.ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            Items = new ObservableCollection<MonthlyRentInvoiceModel>();
            LoadOwnerBillItemsCommand = new Command(async () => await LoadOwnerBillItems());
            LoadAgentBillItemsCommand = new Command(async () => await LoadAgentBillItems());
            LoadOwnerPaymentItemsCommand = new Command(async () => await LoadOwnerPaymentItems());
            LoadAgentPaymentItemsCommand = new Command(async () => await LoadAgentPaymentItems());
            ViewDetailsCommand = new Command<MonthlyRentInvoiceModel>(async (propertyhouseinvoice) => await ViewDetails(propertyhouseinvoice.Invoiceid));
            OnCancelClickedCommand = new Command(OnCancelClicked);
            OnOkClickedCommand = new Command(async () => await SaveHouseInvoicePaymentDataAsync());
            PaymentItems = new ObservableCollection<CustomerPaymentData>();
            ValidateThisPaymentCommand = new Command<CustomerPaymentData>(async (payment) => await ValidateThisPaymentDetail(payment.CustomerPaymentId));
            UpdatePaymentValidationCommand = new Command(async () => await UpdatePaymentValidationData());
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

        private string _paymentActualAmountError;
        public string PaymentActualAmountError
        {
            get => _paymentActualAmountError;
            set
            {
                _paymentActualAmountError = value;
                OnPropertyChanged();
            }
        }
        private async Task LoadOwnerBillItems()
        {
            IsProcessing = true;
            IsDataLoaded = false;

            try
            {
                var response = await _serviceProvider.CallAuthWebApi<object>("/api/PropertyHouse/Gettenantmonthlyinvoicedatabyownerid/" + App.UserDetails.Usermodel.Userid, HttpMethod.Get, null);
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
        private async Task LoadAgentBillItems()
        {
            IsProcessing = true;
            IsDataLoaded = false;

            try
            {
                var response = await _serviceProvider.CallAuthWebApi<object>("/api/PropertyHouse/Gettenantmonthlyinvoicedatabyagentid/" + App.UserDetails.Usermodel.Userid, HttpMethod.Get, null);
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
                    var modalPage = new HousesRoomInvoiceDetailModalPage(this);
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
                CustomerPaymentValidation InvoicePaymentValidationData = new CustomerPaymentValidation();
                InvoicePaymentValidationData.CustomerPaymentId = TenantInvoiceDetailData.Propertyhouseroomtenantid;
                InvoicePaymentValidationData.Confirmedby = App.UserDetails.Usermodel.Userid;
                InvoicePaymentValidationData.Datemodified = DateTime.UtcNow;


                var response = await _serviceProvider.CallAuthWebApi<object>("/api/PropertyHouse/Validatepropertyhouseroomrentpaymentrequestdata", HttpMethod.Post, InvoicePaymentValidationData);
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
                InvoicePayemtModeError = "Pay mode is required.";
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

        private async Task LoadOwnerPaymentItems()
        {
            IsProcessing = true;
            IsDataLoaded = false;

            try
            {
                var response = await _serviceProvider.CallAuthWebApi<object>("/api/PropertyHouse/Gettenantmonthlyinvoicepaymentdatabyownerid/" + App.UserDetails.Usermodel.Userid, HttpMethod.Get, null);
                if (response != null && response.Data is List<dynamic> items)
                {
                    PaymentItems.Clear();
                    foreach (var item in items)
                    {
                        var payments = item.ToObject<CustomerPaymentData>();
                        PaymentItems.Add(payments);
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


        private async Task LoadAgentPaymentItems()
        {
            IsProcessing = true;
            IsDataLoaded = false;

            try
            {
                var response = await _serviceProvider.CallAuthWebApi<object>("/api/PropertyHouse/Gettenantmonthlyinvoicepaymentdatabyagentid/" + App.UserDetails.Usermodel.Userid, HttpMethod.Get, null);
                if (response != null && response.Data is List<dynamic> items)
                {
                    PaymentItems.Clear();
                    foreach (var item in items)
                    {
                        var payments = item.ToObject<CustomerPaymentData>();
                        PaymentItems.Add(payments);
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
        private async Task ValidateThisPaymentDetail(long CustomerPaymentId)
        {
            IsProcessing = true;
            var response = await _serviceProvider.CallAuthWebApi<object>("/api/PropertyHouse/Getsystempropertyroompaymentbypaymentid/" + CustomerPaymentId, HttpMethod.Get, null);
            if (response != null)
            {
                CustomerPaymentValidationData = JsonConvert.DeserializeObject<CustomerPaymentValidation>(response.Data.ToString());
            }
            var modalPage = new ValidateThisPaymentDetailModalPage(this);
            await Application.Current.MainPage.Navigation.PushModalAsync(modalPage);
            IsProcessing = false;
        }
        public async Task UpdatePaymentValidationData()
        {
            IsProcessing = true;
            if (!Validate())
            {
                IsProcessing = false;
                return;
            }

           
            if (CustomerPaymentValidationData == null)
            {
                IsProcessing = false;
                return;
            }
            CustomerPaymentValidationData.ValidatedBy = App.UserDetails.Usermodel.Userid;
            CustomerPaymentValidationData.Datecreated = DateTime.UtcNow;
            CustomerPaymentValidationData.Datemodified = DateTime.UtcNow;
            try
            {
                var response = await _serviceProvider.CallCustomUnAuthWebApi("/api/PropertyHouse/Validatepropertyhouseroomrentpaymentrequestdata", CustomerPaymentValidationData);
                if (response.RespStatus == 200 || response.RespStatus == 0)
                {
                    Application.Current.MainPage.Navigation.PopModalAsync();
                }
                else if (response.RespStatus == 1)
                {
                    await Shell.Current.DisplayAlert("Warning", response.RespMessage, "OK");
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

        private bool Validate()
        {
            bool isValid = true;
            if (CustomerPaymentValidationData.Actualamount == 0)
            {
                PaymentActualAmountError = "Required.";
                isValid = false;
            }
            else
            {
                PaymentActualAmountError = null;
            }
            IsProcessing = isValid;

            return isValid;
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
