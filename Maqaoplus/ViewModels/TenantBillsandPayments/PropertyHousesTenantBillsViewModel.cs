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
                    //var kitchentypeResponse = await _serviceProvider.GetSystemDropDownData("/api/General?listType=" + ListModelType.Systemkitchentype, HttpMethod.Get);

                    //if (kitchentypeResponse != null)
                    //{
                    //    Systemkitchentype = new ObservableCollection<ListModel>(kitchentypeResponse);
                    //    SelectedKitchentype = Systemkitchentype.FirstOrDefault(x => x.Value == _houseroomData.Kitchentypeid.ToString());
                    //}
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
        private void OnOkButtonClicked()
        {
            Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}
