using DBL.Entities;
using DBL.Enum;
using DBL.Models;
using Firebase.Storage;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Maqaoplus.Views.PropertyHouse.Modal;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Maqaoplus.ViewModels.PropertyHouse
{
    public class PropertyHouseViewModel : INotifyPropertyChanged
    {
        private readonly Services.ServiceProvider _serviceProvider;
        public event PropertyChangedEventHandler PropertyChanged;
        public string CopyrightText => $"© 2020 - {DateTime.Now.Year}  UTTAMB SOLUTIONS LIMITED";
        public string AgreementText { get; set; }
        public ObservableCollection<Systemproperty> Items { get; }
        private Systemproperty _systempropertyData;
        private OwnerTenantAgreementDetailData _ownerTenantAgreementDetailData;
        private SystemPropertyHouseImage _systemPropertyHouseImageData;

        private bool _isStep1Visible;
        private bool _isStep2Visible;
        private bool _isStep3Visible;
        private bool _isStep4Visible;
        private bool _isStep5Visible;

        public ICommand AddPropertyHouseCommand { get; }
        public ICommand EditPropertyHouseCommand { get; }
        public ICommand LoadItemsCommand { get; }
        public ICommand ViewDetailsCommand { get; }
        public ICommand ViewPropertyAgreementCommand { get; }
        public ICommand ViewPropertyHouseImageCommand { get; }
        public ICommand NextCommand { get; }
        public ICommand PreviousCommand { get; }
        public ICommand OnCancelClickedCommand { get; }
        public ICommand SavePropertyHouseCommand { get; }


        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }
        private bool _isValid;
        public bool IsValid
        {
            get => _isValid;
            private set
            {
                if (_isValid != value)
                {
                    _isValid = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isProcessing;
        public bool IsProcessing
        {
            get => _isProcessing;
            set
            {
                _isProcessing = value;
                OnPropertyChanged();
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



        private ObservableCollection<ListModel> _systemcounty;
        private ObservableCollection<ListModel> _systemsubcounty;
        private ObservableCollection<ListModel> _systemsubcountyward;
        private ObservableCollection<ListModel> _systemhouseentrystatus;
        private ObservableCollection<ListModel> _systemhousewatertype;
        private ObservableCollection<ListModel> _systemhouserentdueday;
        private ObservableCollection<ListModel> _systemhouserentdepositmonths;
        private ObservableCollection<ListModel> _systemhousevacantnoticeperiod;


        public Systemproperty SystempropertyData
        {
            get => _systempropertyData;
            set
            {
                _systempropertyData = value;
                OnPropertyChanged();
            }
        }
        public OwnerTenantAgreementDetailData OwnerTenantAgreementDetailData
        {
            get => _ownerTenantAgreementDetailData;
            set
            {
                _ownerTenantAgreementDetailData = value;
                OnPropertyChanged(nameof(OwnerTenantAgreementDetailData));
                OnPropertyChanged(nameof(IsSignatureDrawingVisible));
                OnPropertyChanged(nameof(IsSignatureImageVisible));
                OnPropertyChanged(nameof(IsSignatureAvailable));
            }
        }

        public bool IsSignatureDrawingVisible => string.IsNullOrEmpty(OwnerTenantAgreementDetailData?.OwnerSignatureimageurl);
        public bool IsSignatureImageVisible => !string.IsNullOrEmpty(OwnerTenantAgreementDetailData?.OwnerSignatureimageurl);
        public bool IsSignatureAvailable => !string.IsNullOrEmpty(OwnerTenantAgreementDetailData?.OwnerSignatureimageurl);


        public SystemPropertyHouseImage SystemPropertyHouseImageData
        {
            get => _systemPropertyHouseImageData;
            set
            {
                _systemPropertyHouseImageData = value;
                OnPropertyChanged();
            }
        }
        // Error properties
        private string _propertyHouseNameError;
        public string PropertyHouseNameError
        {
            get => _propertyHouseNameError;
            set
            {
                _propertyHouseNameError = value;
                OnPropertyChanged();
            }
        }

        private string _streetOrLandmarkError;
        public string StreetOrLandmarkError
        {
            get => _streetOrLandmarkError;
            set
            {
                _streetOrLandmarkError = value;
                OnPropertyChanged();
            }
        }

        private string _contactDetailsError;
        public string ContactDetailsError
        {
            get => _contactDetailsError;
            set
            {
                _contactDetailsError = value;
                OnPropertyChanged();
            }
        }

        private string _propertyHouseStatusError;
        public string PropertyHouseStatusError
        {
            get => _propertyHouseStatusError;
            set
            {
                _propertyHouseStatusError = value;
                OnPropertyChanged();
            }
        }
        private string _propertyHouseWaterTypeError;
        public string PropertyHouseWaterTypeError
        {
            get => _propertyHouseWaterTypeError;
            set
            {
                _propertyHouseWaterTypeError = value;
                OnPropertyChanged();
            }
        }


        private string _propertyHouseCountyError;
        public string PropertyHouseCountyError
        {
            get => _propertyHouseCountyError;
            set
            {
                _propertyHouseCountyError = value;
                OnPropertyChanged();
            }
        }

        private string _propertyHouseSubcountyError;
        public string PropertyHouseSubcountyError
        {
            get => _propertyHouseSubcountyError;
            set
            {
                _propertyHouseSubcountyError = value;
                OnPropertyChanged();
            }
        }
        private string _propertyHouseSubcountyWardError;
        public string PropertyHouseSubcountyWardError
        {
            get => _propertyHouseSubcountyWardError;
            set
            {
                _propertyHouseSubcountyWardError = value;
                OnPropertyChanged();
            }
        }
        private string _propertyHouseRentDueDayError;
        public string PropertyHouseRentDueDayError
        {
            get => _propertyHouseRentDueDayError;
            set
            {
                _propertyHouseRentDueDayError = value;
                OnPropertyChanged();
            }
        }
        private string _propertyHouseRentDepositMonthsError;
        public string PropertyHouseRentDepositMonthsError
        {
            get => _propertyHouseRentDepositMonthsError;
            set
            {
                _propertyHouseRentDepositMonthsError = value;
                OnPropertyChanged();
            }
        }

        private string _propertyHouseRentVacationPeriodMonthsError;
        public string PropertyHouseRentVacationPeriodMonthsError
        {
            get => _propertyHouseRentVacationPeriodMonthsError;
            set
            {
                _propertyHouseRentVacationPeriodMonthsError = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Systempropertyhousesize> PropertyHouseSizes { get; set; } = new ObservableCollection<Systempropertyhousesize>();
        public ObservableCollection<Systempropertyhousedepositfees> PropertyHouseDepositFees { get; set; } = new ObservableCollection<Systempropertyhousedepositfees>();
        public ObservableCollection<Systempropertyhousebenefits> PropertyHouseBenefits { get; set; } = new ObservableCollection<Systempropertyhousebenefits>();

        // Parameterless constructor for XAML support
        public PropertyHouseViewModel(Services.ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            Items = new ObservableCollection<Systemproperty>();
            AddPropertyHouseCommand = new Command(AddPropertyHouseAsync);
            EditPropertyHouseCommand = new Command<Systemproperty>(async (property) => await EditPropertyHouseAsync(property.Propertyhouseid));
            LoadItemsCommand = new Command(async () => await LoadItems());
            ViewDetailsCommand = new Command<Systemproperty>(async (property) => await ViewDetails(property.Propertyhouseid));
            ViewPropertyAgreementCommand = new Command<Systemproperty>(async (property) => await ViewPropertyAgreementDetails(property.Propertyhouseid, property.Propertyhouseowner));
            ViewPropertyHouseImageCommand = new Command<Systemproperty>(async (property) => await ViewPropertyHouseImagesDetails(property.Propertyhouseid));
            NextCommand = new Command(NextStep);
            PreviousCommand = new Command(PreviousStep);
            OnCancelClickedCommand = new Command(OnCancelClicked);
            SavePropertyHouseCommand = new Command(async () => await SavePropertyHouseAsync());


            // Initialize steps
            _isStep1Visible = true;
            _isStep2Visible = false;
            _isStep3Visible = false;
            _isStep4Visible = false;
            _isStep5Visible = false;
        }

        public ObservableCollection<ListModel> Systemcounty
        {
            get => _systemcounty;
            set
            {
                _systemcounty = value;
                OnPropertyChanged();
            }
        }
        private ListModel _selectedCounty;
        public ListModel SelectedCounty
        {
            get => _selectedCounty;
            set
            {
                _selectedCounty = value;

                // Ensure SystempropertyData is not null
                if (SystempropertyData != null)
                {
                    // Safely convert the selected value to long and assign it to Countyid
                    if (value != null && int.TryParse(value.Value?.ToString(), out int countyId))
                    {
                        SystempropertyData.Countyid = countyId;
                    }
                    else
                    {
                        SystempropertyData.Countyid = 0;
                    }

                    OnPropertyChanged(nameof(SelectedCounty));
                    OnPropertyChanged(nameof(SystempropertyData.Countyid));
                }
            }
        }


        public ObservableCollection<ListModel> Systemsubcounty
        {
            get => _systemsubcounty;
            set
            {
                _systemsubcounty = value;
                OnPropertyChanged();
            }
        }
        private ListModel _selectedSubcounty;
        public ListModel SelectedSubcounty
        {
            get => _selectedSubcounty;
            set
            {
                _selectedSubcounty = value;

                // Ensure SystempropertyData is not null
                if (SystempropertyData != null)
                {
                    // Safely convert the selected value to long and assign it to Countyid
                    if (value != null && int.TryParse(value.Value?.ToString(), out int subCountyId))
                    {
                        SystempropertyData.Subcountyid = subCountyId;
                    }
                    else
                    {
                        SystempropertyData.Countyid = 0;
                    }

                    OnPropertyChanged(nameof(SelectedSubcounty));
                    OnPropertyChanged(nameof(SystempropertyData.Subcountyid));
                }
            }
        }

        public ObservableCollection<ListModel> Systemsubcountyward
        {
            get => _systemsubcountyward;
            set
            {
                _systemsubcountyward = value;
                OnPropertyChanged();
            }
        }

        private ListModel _selectedSubcountyward;
        public ListModel SelectedSubcountyward
        {
            get => _selectedSubcountyward;
            set
            {
                _selectedSubcountyward = value;

                // Ensure SystempropertyData is not null
                if (SystempropertyData != null)
                {
                    // Safely convert the selected value to long and assign it to Countyid
                    if (value != null && int.TryParse(value.Value?.ToString(), out int subcountywardid))
                    {
                        SystempropertyData.Subcountywardid = subcountywardid;
                    }
                    else
                    {
                        SystempropertyData.Subcountywardid = 0;
                    }

                    OnPropertyChanged(nameof(SelectedSubcountyward));
                    OnPropertyChanged(nameof(SystempropertyData.Subcountywardid));
                }
            }
        }

        public ObservableCollection<ListModel> Systemhouseentrystatus
        {
            get => _systemhouseentrystatus;
            set
            {
                _systemhouseentrystatus = value;
                OnPropertyChanged();
            }
        }
        private ListModel _selectedHouseentrystatus;
        public ListModel SelectedHouseentrystatus
        {
            get => _selectedHouseentrystatus;
            set
            {
                _selectedHouseentrystatus = value;

                // Ensure SystempropertyData is not null
                if (SystempropertyData != null)
                {
                    // Safely convert the selected value to long and assign it to Countyid
                    if (value != null && int.TryParse(value.Value?.ToString(), out int propertyhousestatus))
                    {
                        SystempropertyData.Propertyhousestatus = propertyhousestatus;
                    }
                    else
                    {
                        SystempropertyData.Propertyhousestatus = 0;
                    }

                    OnPropertyChanged(nameof(SelectedHouseentrystatus));
                    OnPropertyChanged(nameof(SystempropertyData.Propertyhousestatus));
                }
            }
        }
        public ObservableCollection<ListModel> Systemhousewatertype
        {
            get => _systemhousewatertype;
            set
            {
                _systemhousewatertype = value;
                OnPropertyChanged();
            }
        }
        private ListModel _selectedHousewatertype;
        public ListModel SelectedHousewatertype
        {
            get => _selectedHousewatertype;
            set
            {
                _selectedHousewatertype = value;

                // Ensure SystempropertyData is not null
                if (SystempropertyData != null)
                {
                    // Safely convert the selected value to long and assign it to Countyid
                    if (value != null && int.TryParse(value.Value?.ToString(), out int watertypeid))
                    {
                        SystempropertyData.Watertypeid = watertypeid;
                    }
                    else
                    {
                        SystempropertyData.Watertypeid = 0;
                    }

                    OnPropertyChanged(nameof(SelectedHousewatertype));
                    OnPropertyChanged(nameof(SystempropertyData.Watertypeid));
                }
            }
        }
        public ObservableCollection<ListModel> Systemhouserentdueday
        {
            get => _systemhouserentdueday;
            set
            {
                _systemhouserentdueday = value;
                OnPropertyChanged();
            }
        }
        private ListModel _selectedHouserentdueday;
        public ListModel SelectedHouserentdueday
        {
            get => _selectedHouserentdueday;
            set
            {
                _selectedHouserentdueday = value;

                // Ensure SystempropertyData is not null
                if (SystempropertyData != null)
                {
                    // Safely convert the selected value to long and assign it to Countyid
                    if (value != null && int.TryParse(value.Value?.ToString(), out int rentdueday))
                    {
                        SystempropertyData.Rentdueday = rentdueday;
                    }
                    else
                    {
                        SystempropertyData.Rentdueday = 0;
                    }

                    OnPropertyChanged(nameof(SelectedHouserentdueday));
                    OnPropertyChanged(nameof(SystempropertyData.Rentdueday));
                }
            }
        }
        public ObservableCollection<ListModel> Systemhousedepostmonths
        {
            get => _systemhouserentdepositmonths;
            set
            {
                _systemhouserentdepositmonths = value;
                OnPropertyChanged();
            }
        }
        private ListModel _selectedHousedepostmonths;
        public ListModel SelectedHousedepostmonths
        {
            get => _selectedHousedepostmonths;
            set
            {
                _selectedHousedepostmonths = value;

                // Ensure SystempropertyData is not null
                if (SystempropertyData != null)
                {
                    // Safely convert the selected value to long and assign it to Countyid
                    if (value != null && int.TryParse(value.Value?.ToString(), out int rentdepositmonth))
                    {
                        SystempropertyData.Rentdepositmonth = rentdepositmonth;
                    }
                    else
                    {
                        SystempropertyData.Rentdepositmonth = 0;
                    }

                    OnPropertyChanged(nameof(SelectedHousedepostmonths));
                    OnPropertyChanged(nameof(SystempropertyData.Rentdepositmonth));
                }
            }
        }
        public ObservableCollection<ListModel> Systemhousevacantnoticeperiod
        {
            get => _systemhousevacantnoticeperiod;
            set
            {
                _systemhousevacantnoticeperiod = value;
                OnPropertyChanged();
            }
        }
        private ListModel _selectedHousevacantnoticeperiod;
        public ListModel SelectedHousevacantnoticeperiod
        {
            get => _selectedHousevacantnoticeperiod;
            set
            {
                _selectedHousevacantnoticeperiod = value;

                // Ensure SystempropertyData is not null
                if (SystempropertyData != null)
                {
                    // Safely convert the selected value to long and assign it to Countyid
                    if (value != null && int.TryParse(value.Value?.ToString(), out int vacantnoticeperiod))
                    {
                        SystempropertyData.Vacantnoticeperiod = vacantnoticeperiod;
                    }
                    else
                    {
                        SystempropertyData.Vacantnoticeperiod = 0;
                    }

                    OnPropertyChanged(nameof(SelectedHousevacantnoticeperiod));
                    OnPropertyChanged(nameof(SystempropertyData.Vacantnoticeperiod));
                }
            }
        }
        private async void AddPropertyHouseAsync()
        {
            IsProcessing = true;

            Systemhouseentrystatus = new ObservableCollection<ListModel>
            {
                new ListModel { Value = "0", Text = "First Tenants" },
                new ListModel { Value = "1", Text = "Second Tenants" },

            };
            Systemhouserentdueday = new ObservableCollection<ListModel>();
            for (int i = 1; i <= 28; i++)
            {
                string suffix = i switch
                {
                    1 or 21 => "st",
                    2 or 22 => "nd",
                    3 or 23 => "rd",
                    _ => "th"
                };
                Systemhouserentdueday.Add(new ListModel { Value = i.ToString(), Text = $"{i} {suffix} Day" });
            }
            Systemhousedepostmonths = new ObservableCollection<ListModel>();
            for (int i = 1; i <= 6; i++)
            {
                Systemhousedepostmonths.Add(new ListModel { Value = i.ToString(), Text = $"{i} Month{(i > 1 ? "s" : "")}" });
            }
            Systemhousevacantnoticeperiod = new ObservableCollection<ListModel>();
            for (int i = 1; i <= 12; i++)
            {
                Systemhousevacantnoticeperiod.Add(new ListModel { Value = i.ToString(), Text = $"{i} Month{(i > 1 ? "s" : "")}" });
            }
            LoadDropdownData();
            var response = await _serviceProvider.CallAuthWebApi<object>("/api/PropertyHouse/Getsystempropertyhousedetaildatabyid/0", HttpMethod.Get, null);
            if (response != null)
            {
                SystempropertyData = JsonConvert.DeserializeObject<Systemproperty>(response.Data.ToString());
            }
            var modalPage = new AddSystemPropertyHouseModalPage(this);
            await Application.Current.MainPage.Navigation.PushModalAsync(modalPage);
            IsProcessing = false;
        }


        private async Task EditPropertyHouseAsync(long propertyId)
        {
            IsProcessing = true;

            Systemhouseentrystatus = new ObservableCollection<ListModel>
            {
                new ListModel { Value = "0", Text = "First Tenants" },
                new ListModel { Value = "1", Text = "Second Tenants" },

            };
            Systemhouserentdueday = new ObservableCollection<ListModel>();
            for (int i = 1; i <= 28; i++)
            {
                string suffix = i switch
                {
                    1 or 21 => "st",
                    2 or 22 => "nd",
                    3 or 23 => "rd",
                    _ => "th"
                };
                Systemhouserentdueday.Add(new ListModel { Value = i.ToString(), Text = $"{i} {suffix} Day" });
            }
            Systemhousedepostmonths = new ObservableCollection<ListModel>();
            for (int i = 1; i <= 6; i++)
            {
                Systemhousedepostmonths.Add(new ListModel { Value = i.ToString(), Text = $"{i} Month{(i > 1 ? "s" : "")}" });
            }
            Systemhousevacantnoticeperiod = new ObservableCollection<ListModel>();
            for (int i = 1; i <= 12; i++)
            {
                Systemhousevacantnoticeperiod.Add(new ListModel { Value = i.ToString(), Text = $"{i} Month{(i > 1 ? "s" : "")}" });
            }
            LoadDropdownData();
            var response = await _serviceProvider.CallAuthWebApi<object>("/api/PropertyHouse/Getsystempropertyhousedetaildatabyid/" + propertyId, HttpMethod.Get, null);
            if (response != null)
            {
                SystempropertyData = JsonConvert.DeserializeObject<Systemproperty>(response.Data.ToString());
            }
            var modalPage = new AddSystemPropertyHouseModalPage(this);
            await Application.Current.MainPage.Navigation.PushModalAsync(modalPage);
            IsProcessing = false;
        }
        private async Task LoadDropdownData()
        {
            try
            {
                var SystemcountyResponse = await _serviceProvider.GetSystemDropDownData("/api/General?listType=" + ListModelType.SystemCounty, HttpMethod.Get);
                var SystemsubcountyResponse = await _serviceProvider.GetSystemDropDownData("/api/General?listType=" + ListModelType.SystemSubCounty, HttpMethod.Get);
                var SystemsubcountywardResponse = await _serviceProvider.GetSystemDropDownData("/api/General?listType=" + ListModelType.SystemSubCountyWard, HttpMethod.Get);
                var SystemhousewatertypeResponse = await _serviceProvider.GetSystemDropDownData("/api/General?listType=" + ListModelType.Systemhousewatertype, HttpMethod.Get);

                if (SystemcountyResponse != null)
                {
                    Systemcounty = new ObservableCollection<ListModel>(SystemcountyResponse);
                }
                if (SystemsubcountyResponse != null)
                {
                    Systemsubcounty = new ObservableCollection<ListModel>(SystemsubcountyResponse);
                }
                if (SystemsubcountywardResponse != null)
                {
                    Systemsubcountyward = new ObservableCollection<ListModel>(SystemsubcountywardResponse);
                }
                if (SystemhousewatertypeResponse != null)
                {
                    Systemhousewatertype = new ObservableCollection<ListModel>(SystemhousewatertypeResponse);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task LoadItems()
        {
            IsProcessing = true;
            IsDataLoaded = false;

            try
            {
                var response = await _serviceProvider.CallAuthWebApi<object>("/api/PropertyHouse/Getsystempropertyhousedatabyowner/" + App.UserDetails.Usermodel.Userid, HttpMethod.Get, null);
                if (response != null && response.Data is List<dynamic> items)
                {
                    Items.Clear();
                    foreach (var item in items)
                    {
                        var product = item.ToObject<Systemproperty>();
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
        private async Task ViewDetails(long propertyId)
        {
            IsProcessing = true;
            try
            {
                var encodedPropertyId = Uri.EscapeDataString(propertyId.ToString());
                await Shell.Current.GoToAsync($"PropertyHousesDetailPage?PropertyId={encodedPropertyId}");
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
        private async Task ViewPropertyAgreementDetails(long propertyId, long Ownerid)
        {
            IsProcessing = true;
            var response = await _serviceProvider.CallAuthWebApi<object>("/api/PropertyHouse/Getsystempropertyhouseagreementdetaildatabypropertyidandownerid/" + propertyId + "/" + Ownerid, HttpMethod.Get, null);
            if (response != null)
            {
                OwnerTenantAgreementDetailData = JsonConvert.DeserializeObject<OwnerTenantAgreementDetailData>(response.Data.ToString());
            }
            var modalPage = new SystemPropertyHouseAgreementModalPage(this);
            await Application.Current.MainPage.Navigation.PushModalAsync(modalPage);
            IsProcessing = false;
        }

        public async Task AgreeToPropertyHouseAgreementasync(string imageUrl)
        {
            IsProcessing = true;

            await Task.Delay(500);
            if (OwnerTenantAgreementDetailData == null)
            {
                IsProcessing = false;
                return;
            }
            OwnerTenantAgreementDetailData.Propertyhouseowner = App.UserDetails.Usermodel.Userid;
            OwnerTenantAgreementDetailData.Signatureimageurl = imageUrl;
            OwnerTenantAgreementDetailData.Ownerortenant = "Owner";
            OwnerTenantAgreementDetailData.Agreementname = OwnerTenantAgreementDetailData.Fullname + " Property " + OwnerTenantAgreementDetailData.Propertyhousename + " Owner Agreement";
            OwnerTenantAgreementDetailData.Datecreated = DateTime.UtcNow;
            try
            {
                var response = await _serviceProvider.CallCustomUnAuthWebApi("/api/PropertyHouse/Registersystempropertyhouseagreementdata", OwnerTenantAgreementDetailData);
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


        private async Task ViewPropertyHouseImagesDetails(long propertyHouseId)
        {
            IsProcessing = true;
            var response = await _serviceProvider.CallAuthWebApi<object>("/api/PropertyHouse/Getsystempropertyhouseroomimagebyhouseid/" + propertyHouseId, HttpMethod.Get, null);
            if (response != null)
            {
                SystemPropertyHouseImageData = JsonConvert.DeserializeObject<SystemPropertyHouseImage>(response.Data.ToString());
            }
            var modalPage = new SystemPropertyHouseImagesModalPage(this);
            await Application.Current.MainPage.Navigation.PushModalAsync(modalPage);
            IsProcessing = false;
        }
        public async Task SavePropertyHouseImageasync(string imageUrl)
        {
            IsProcessing = true;

            await Task.Delay(500);
            if (SystemPropertyHouseImageData == null)
            {
                IsProcessing = false;
                return;
            }
            SystemPropertyHouseImageData.Createdby = App.UserDetails.Usermodel.Userid;
            SystemPropertyHouseImageData.Houseorroomimageurl = imageUrl;
            SystemPropertyHouseImageData.Houseorroom = "PropertyHouse";
            SystemPropertyHouseImageData.Datecreated = DateTime.UtcNow;
            try
            {
                var response = await _serviceProvider.CallCustomUnAuthWebApi("/api/PropertyHouse/Registersystempropertyhouseroomimagedata", SystemPropertyHouseImageData);
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
        public bool IsStep1Visible
        {
            get => _isStep1Visible;
            set
            {
                _isStep1Visible = value;
                OnPropertyChanged();
            }
        }

        public bool IsStep2Visible
        {
            get => _isStep2Visible;
            set
            {
                _isStep2Visible = value;
                OnPropertyChanged();
            }
        }

        public bool IsStep3Visible
        {
            get => _isStep3Visible;
            set
            {
                _isStep3Visible = value;
                OnPropertyChanged();
            }
        }

        public bool IsStep4Visible
        {
            get => _isStep4Visible;
            set
            {
                _isStep4Visible = value;
                OnPropertyChanged();
            }
        }
        public bool IsStep5Visible
        {
            get => _isStep5Visible;
            set
            {
                _isStep5Visible = value;
                OnPropertyChanged();
            }
        }
        private async void NextStep()
        {
            IsLoading = true;

            await Task.Delay(500);
            // Move to the next step
            if (_isStep1Visible)
            {
                if (!ValidateStep1())
                {
                    IsLoading = false;
                    return;
                }
                _isStep1Visible = false;
                _isStep2Visible = true;
            }
            else if (_isStep2Visible)
            {

                _isStep2Visible = false;
                _isStep3Visible = true;
            }
            else if (_isStep3Visible)
            {
                _isStep3Visible = false;
                _isStep4Visible = true;
            }
            else if (_isStep4Visible)
            {
                _isStep4Visible = false;
                _isStep5Visible = true;

            }
            IsLoading = false;
            OnPropertyChanged(nameof(IsStep1Visible));
            OnPropertyChanged(nameof(IsStep2Visible));
            OnPropertyChanged(nameof(IsStep3Visible));
            OnPropertyChanged(nameof(IsStep4Visible));
            OnPropertyChanged(nameof(IsStep5Visible));
        }

        private async void PreviousStep()
        {
            IsLoading = true;

            await Task.Delay(500);
            // Move to the previous step
            if (_isStep5Visible)
            {
                _isStep5Visible = false;
                _isStep4Visible = true;
            }
            else if (_isStep4Visible)
            {
                _isStep4Visible = false;
                _isStep3Visible = true;
            }
            else if (_isStep3Visible)
            {
                _isStep3Visible = false;
                _isStep2Visible = true;
            }
            else if (_isStep2Visible)
            {
                _isStep2Visible = false;
                _isStep1Visible = true;
            }
            IsLoading = false;

            OnPropertyChanged(nameof(IsStep1Visible));
            OnPropertyChanged(nameof(IsStep2Visible));
            OnPropertyChanged(nameof(IsStep3Visible));
            OnPropertyChanged(nameof(IsStep4Visible));
            OnPropertyChanged(nameof(IsStep5Visible));
        }
        private void OnCancelClicked()
        {
            Application.Current.MainPage.Navigation.PopModalAsync();
        }
        public async Task SavePropertyHouseAsync()
        {
            IsProcessing = true;

            await Task.Delay(500);
            if (SystempropertyData == null)
            {
                IsProcessing = false;
                return;
            }
            SystempropertyData.Propertyhouseowner = App.UserDetails.Usermodel.Userid;
            SystempropertyData.Createdby = App.UserDetails.Usermodel.Userid;
            SystempropertyData.Modifiedby = App.UserDetails.Usermodel.Userid;
            SystempropertyData.Propertyhouseposter = App.UserDetails.Usermodel.Userid;
            SystempropertyData.Datecreated = DateTime.Now;
            SystempropertyData.Datemodified = DateTime.Now;
            try
            {
                var response = await _serviceProvider.CallCustomUnAuthWebApi("/api/PropertyHouse/Registersystempropertyhousedata", SystempropertyData);
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

        private bool ValidateStep1()
        {
            bool isValid = true;

            // Validate Property Name
            if (string.IsNullOrWhiteSpace(SystempropertyData.Propertyhousename))
            {
                PropertyHouseNameError = "Property Name is required.";
                isValid = false;
            }
            else
            {
                PropertyHouseNameError = null;
            }

            // Validate Street or Landmark
            if (string.IsNullOrWhiteSpace(SystempropertyData?.Streetorlandmark))
            {
                StreetOrLandmarkError = "Street or Landmark is required.";
                isValid = false;
            }
            else
            {
                StreetOrLandmarkError = null;
            }

            // Validate Contact Details
            if (string.IsNullOrWhiteSpace(SystempropertyData?.Contactdetails))
            {
                ContactDetailsError = "Contact Details are required.";
                isValid = false;
            }
            else
            {
                ContactDetailsError = null;
            }

            // Validate Property House Status
            if (SystempropertyData?.Propertyhousestatus < 0)
            {
                PropertyHouseStatusError = "Property House Status is required.";
                isValid = false;
            }
            else
            {
                PropertyHouseStatusError = null;
            }
            // Validate Property House Water Type
            if (SystempropertyData?.Watertypeid == 0)
            {
                PropertyHouseWaterTypeError = "Property House Water Type is required.";
                isValid = false;
            }
            else
            {
                PropertyHouseWaterTypeError = null;
            }
            // Validate Property House County
            if (SystempropertyData?.Countyid == 0)
            {
                PropertyHouseCountyError = "Property House County is required.";
                isValid = false;
            }
            else
            {
                PropertyHouseCountyError = null;
            }
            // Validate Property House Sub County
            if (SystempropertyData?.Subcountyid == 0)
            {
                PropertyHouseSubcountyError = "Property House Subcounty is required.";
                isValid = false;
            }
            else
            {
                PropertyHouseSubcountyError = null;
            }
            // Validate Property House sub County Ward
            if (SystempropertyData?.Subcountywardid == 0)
            {
                PropertyHouseSubcountyWardError = "Property House Subcounty Ward is required.";
                isValid = false;
            }
            else
            {
                PropertyHouseSubcountyWardError = null;
            }
            // Validate Property House Rent Deposit
            if (SystempropertyData?.Rentdueday == 0)
            {
                PropertyHouseRentDueDayError = "Property House Rent Due Day is required.";
                isValid = false;
            }
            else
            {
                PropertyHouseRentDueDayError = null;
            }
            // Validate Property House Rent Deposit Months
            if (SystempropertyData?.Rentdepositmonth == 0)
            {
                PropertyHouseRentDepositMonthsError = "Property House Rent Deposit Months is required.";
                isValid = false;
            }
            else
            {
                PropertyHouseRentDepositMonthsError = null;
            }
            // Validate Property House Vacation Period
            if (SystempropertyData?.Vacantnoticeperiod == 0)
            {
                PropertyHouseRentVacationPeriodMonthsError = "Property House Vacation Period Months is required.";
                isValid = false;
            }
            else
            {
                PropertyHouseRentVacationPeriodMonthsError = null;
            }
            // Update overall IsValid property
            IsValid = isValid;

            return isValid;
        }

        public async Task<string> GenerateAndUploadAgreementPdfAsync()
        {
            if (TenantAgreementDetailData == null)
                return null;

            // Initialize a memory stream to hold the PDF data
            using (var memoryStream = new MemoryStream())
            {
                try
                {
                    // Generate PDF
                    using (var writer = new PdfWriter(memoryStream))
                    {
                        using (var pdf = new PdfDocument(writer))
                        {
                            var document = new Document(pdf);

                            // Title
                            document.Add(new Paragraph("RENTAL AGREEMENT")
                                .SetFontSize(18)
                                .SetBold()
                                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));

                            // Agreement Details
                            document.Add(new Paragraph($"This Rental Agreement (Agreement) is made and entered into on this {TenantAgreementDetailData.TenantDatecreated:dd} day of {TenantAgreementDetailData.TenantDatecreated:MMM}, 20{TenantAgreementDetailData.TenantDatecreated:yy}, by and between:"));

                            // Landlord Details
                            document.Add(new Paragraph($"Landlord: {TenantAgreementDetailData.Ownerfullname}"));
                            document.Add(new Paragraph($"Property Name: {TenantAgreementDetailData.Propertyhousename}"));
                            document.Add(new Paragraph($"Address: {TenantAgreementDetailData.Countyname}-{TenantAgreementDetailData.Subcountyname}-{TenantAgreementDetailData.Subcountywardname}"));
                            document.Add(new Paragraph($"Phone Number: {TenantAgreementDetailData.Ownerphonenumber}"));
                            document.Add(new Paragraph($"Email Address: {TenantAgreementDetailData.Owneremailaddress}"));

                            // Tenant Details
                            document.Add(new Paragraph("AND"));
                            document.Add(new Paragraph($"Tenant: {TenantAgreementDetailData.Tenantfullname}"));
                            document.Add(new Paragraph($"ID/Passport Number: {TenantAgreementDetailData.Tenantidnumber}"));
                            document.Add(new Paragraph($"Phone Number: {TenantAgreementDetailData.Tenantphonenumber}"));
                            document.Add(new Paragraph($"Email Address: {TenantAgreementDetailData.Tenantemailaddress}"));

                            // Agreement Sections
                            document.Add(new Paragraph($"1. PREMISES: The Landlord hereby agrees to rent to the Tenant, and the Tenant hereby agrees to rent from the Landlord {TenantAgreementDetailData.Propertyhousename}, the residential premises located at: {TenantAgreementDetailData.Countyname}-{TenantAgreementDetailData.Subcountyname}-{TenantAgreementDetailData.Subcountywardname}"));
                            document.Add(new Paragraph($"Land Mark: {TenantAgreementDetailData.Streetorlandmark}"));

                            // Term
                            var startDate = TenantAgreementDetailData.TenantDatecreated.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                            var endDate = TenantAgreementDetailData.TenantDatecreated.AddMonths(12).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture); // Example of fixed-term lease
                            document.Add(new Paragraph($"2. TERM: The term of this rental agreement shall commence on {startDate} (Start Date) and shall continue as follows:"));
                            document.Add(new Paragraph(TenantAgreementDetailData.Monthlyrentterms
                                ? $"Month-to-month tenancy beginning on {startDate}."
                                : $"Fixed-term lease ending on {endDate}."));

                            // Rent
                            document.Add(new Paragraph($"3. RENT: The Tenant agrees to pay the Landlord a monthly rent of Ksh. {TenantAgreementDetailData.Systempropertyhousesizerent:#,##0.00}, payable in advance on or before the {TenantAgreementDetailData.Rentdueday} day of each month. The first rent payment is due on {TenantAgreementDetailData.Nextrentduedate:yyyy-MM-dd}."));

                            // Security Deposit
                            document.Add(new Paragraph($"4. SECURITY DEPOSIT: The Tenant agrees to pay a security deposit of Ksh. {TenantAgreementDetailData.Systempropertyhousesizerentdeposit:#,##0.00}, equivalent to {TenantAgreementDetailData.Rentdepositmonth} month's rent, to be held by the Landlord as security for the performance of the Tenant's obligations under this Agreement. The security deposit will be refunded to the Tenant within {TenantAgreementDetailData.Rentdepositrefunddays} days after vacating the premises, less any deductions for damages beyond normal wear and tear."));

                            // Utilities
                            document.Add(new Paragraph("5. UTILITIES:"));
                            document.Add(new Paragraph(TenantAgreementDetailData.Rentutilityinclusive
                                ? "All utilities (e.g., electricity, water) are included in the rent."
                                : $"The Tenant shall be responsible for the payment of the following utilities: {TenantAgreementDetailData.Propertyhouseutility}"));

                            // Additional Sections
                            document.Add(new Paragraph("6. MAINTENANCE AND REPAIRS: The Tenant agrees to keep the premises in a clean and habitable condition and to promptly notify the Landlord of any necessary repairs. The Tenant shall not make any alterations to the premises without the prior written consent of the Landlord."));
                            document.Add(new Paragraph($"7. OCCUPANTS: The premises shall be occupied by the Tenant and the following individuals:\n{TenantAgreementDetailData.Tenantsintheroom}"));
                            document.Add(new Paragraph("8. PETS:"));
                            document.Add(new Paragraph(TenantAgreementDetailData.Allowpets
                                ? "Pets are allowed, subject to the following conditions: [Specify pet conditions here]"
                                : "No pets are allowed on the premises"));
                            document.Add(new Paragraph("9. PAYMENT DETAILS: The Tenant shall make all payments to the following bank account details:"));
                            document.Add(new Paragraph($"Banking Details: {TenantAgreementDetailData.Systempropertybankname}"));
                            document.Add(new Paragraph("10. INSURANCE: The Tenant is encouraged to obtain renter's insurance to cover personal property against loss or damage. The Landlord shall not be responsible for any loss or damage to the Tenant's personal property."));
                            document.Add(new Paragraph("11. DISPUTE RESOLUTION: Any disputes arising out of or relating to this Agreement shall be resolved through mediation or arbitration, as per the laws of Kenya. The parties agree to seek resolution through these methods before pursuing any legal action."));
                            document.Add(new Paragraph("12. GOVERNING LAW: This Agreement shall be governed by and construed in accordance with the laws of Kenya."));
                            document.Add(new Paragraph("13. ENTIRE AGREEMENT: This Agreement constitutes the entire agreement between the parties and supersedes all prior agreements or understandings, whether written or oral, relating to the subject matter hereof."));

                            // Additional Clauses
                            document.Add(new Paragraph("14. WEAR AND TEAR: Normal wear and tear refers to the deterioration of the premises and its fixtures caused by ordinary use over time, without negligence or abuse. Examples include minor scuffs on walls, worn carpet, or faded paint. Damage beyond normal wear and tear includes, but is not limited to, large holes in walls, broken windows, and significant damage to appliances or fixtures. The Tenant is responsible for repairs or costs associated with damage beyond normal wear and tear. The Landlord shall provide a written estimate for such repairs, and the Tenant shall have the right to contest the charges if they disagree."));

                            document.Add(new Paragraph("15. DATA PROTECTION: The Landlord and Tenant agree to comply with all applicable data protection laws and regulations. The Tenant's personal data collected under this Agreement will be used solely for the purpose of managing the rental relationship and will not be shared with third parties without the Tenant's consent, except as required by law. The Tenant has the right to access, correct, or delete their personal data upon request."));

                            // Signatures Section
                            document.Add(new Paragraph("SIGNATURES")
                                .SetFontSize(16)
                                .SetBold()
                                .SetMarginBottom(20));

                            // Landlord Signature
                            document.Add(new Paragraph("Landlord:")
                                .SetFontSize(16)
                                .SetBold()
                                .SetMarginBottom(5));
                            document.Add(new Paragraph($"Signature: ____________________________"));
                            document.Add(new Paragraph($"Name: {TenantAgreementDetailData.Ownerfullname}"));
                            document.Add(new Paragraph($"Property Owner: {TenantAgreementDetailData.Propertyhousename}"));
                            document.Add(new Paragraph($"Date: {TenantAgreementDetailData.TenantDatecreated:yyyy-MM-dd}"));

                            // Tenant Signature
                            document.Add(new Paragraph("Tenant:")
                                .SetFontSize(16)
                                .SetBold()
                                .SetMarginBottom(5));
                            document.Add(new Paragraph($"Signature: ____________________________"));
                            document.Add(new Paragraph($"Name: {TenantAgreementDetailData.Tenantfullname}"));
                            document.Add(new Paragraph($"Date: {TenantAgreementDetailData.TenantDatecreated:yyyy-MM-dd}"));

                            document.Close();
                        }
                    }
                    memoryStream.Position = 0;
                    var firebaseStorage = new FirebaseStorage("uttambsolutions-4ec2a.appspot.com");
                    var storageReference = firebaseStorage.Child("maqaoplus").Child("agreements").Child("RentalAgreement.pdf");
                    var uploadTask = storageReference.PutAsync(memoryStream);
                    var downloadUrl = await uploadTask;
                    return downloadUrl.ToString();
                }
                catch (Exception ex)
                {
                    // Handle exceptions
                    Debug.WriteLine($"An error occurred while generating or uploading the PDF: {ex.Message}");
                    throw;
                }
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
