using DBL.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Maqaoplus.ViewModels.TenantBillsandPayments
{
    public class PropertyHousesTenantPaymentsViewModel : BaseViewModel
    {
        private readonly Services.ServiceProvider _serviceProvider;
        public ObservableCollection<CustomerPaymentData> PaymentItems { get; }
        private MonthlyRentInvoiceModel _tenantInvoiceDetailData;
        public string CopyrightText => $"© 2020 - {DateTime.Now.Year}  UTTAMB SOLUTIONS LIMITED";
        public ICommand LoadPaymentItemsCommand { get; }
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
        public PropertyHousesTenantPaymentsViewModel(Services.ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            PaymentItems = new ObservableCollection<CustomerPaymentData>();
            LoadPaymentItemsCommand = new Command(async () => await LoadPaymentItems());
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
        private async Task LoadPaymentItems()
        {
            IsProcessing = true;
            IsDataLoaded = false;

            try
            {
                var response = await _serviceProvider.CallAuthWebApi<object>("/api/PropertyHouse/Gettenantmonthlyinvoicepaymentdatabytenantid/" + App.UserDetails.Usermodel.Userid, HttpMethod.Get, null);
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
    }
}
