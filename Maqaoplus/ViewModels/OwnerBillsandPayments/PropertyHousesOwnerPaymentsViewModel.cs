﻿using DBL.Models;
using Maqaoplus.Views.OwnerBillsandPayments.Modals;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Maqaoplus.ViewModels.OwnerBillsandPayments
{
    public class PropertyHousesOwnerPaymentsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly Services.ServiceProvider _serviceProvider;
        public string CopyrightText => $"© 2020 - {DateTime.Now.Year}  UTTAMB SOLUTIONS LIMITED";
        public ObservableCollection<CustomerPaymentData> PaymentItems { get; }
        private CustomerPaymentData _customerPaymentData;
        private MonthlyRentInvoiceModel _tenantInvoiceDetailData;
        public ICommand LoadPaymentItemsCommand { get; }
        public ICommand ViewDetailsCommand { get; }
        public ICommand ValidateThisPaymentCommand { get; }

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


        public CustomerPaymentData CustomerPaymentData
        {
            get => _customerPaymentData;
            set
            {
                _customerPaymentData = value;
                OnPropertyChanged();
            }
        }

        // Parameterless constructor for XAML support
        public PropertyHousesOwnerPaymentsViewModel(Services.ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            PaymentItems = new ObservableCollection<CustomerPaymentData>();
            LoadPaymentItemsCommand = new Command(async () => await LoadPaymentItems());
            ValidateThisPaymentCommand = new Command<CustomerPaymentData>(async (payment) => await ValidateThisPaymentDetail(payment.CustomerPaymentId));
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

        private async Task ValidateThisPaymentDetail(long CustomerPaymentId)
        {
            IsProcessing = true;
            var response = await _serviceProvider.CallAuthWebApi<object>("/api/PropertyHouse/Getsystempropertyhouseroomimagebyhouseid/" + CustomerPaymentId, HttpMethod.Get, null);
            if (response != null)
            {
                CustomerPaymentData = JsonConvert.DeserializeObject<CustomerPaymentData>(response.Data.ToString());
            }
            var modalPage = new ValidateThisPaymentDetailModalPage(this);
            await Application.Current.MainPage.Navigation.PushModalAsync(modalPage);
            IsProcessing = false;
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
