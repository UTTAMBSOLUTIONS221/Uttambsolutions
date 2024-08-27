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
                    OwnerTenantAgreementDetailData.OwnerSignatureimageurl = response.Data2;
                    OwnerTenantAgreementDetailData.Agreementdetailpdfurl = await GenerateAndUploadAgreementPdfAsync();
                    OwnerTenantAgreementDetailData.Agreementid = Convert.ToInt64(response.Data1);
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
            if (OwnerTenantAgreementDetailData == null)
                return null;

            using (var memoryStream = new MemoryStream())
            {
                // Initialize PDF writer and document
                using (var document = new Document(PageSize.A4))
                {
                    PdfWriter.GetInstance(document, memoryStream);
                    document.Open();

                    // Define fonts
                    var boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                    var regularFont = FontFactory.GetFont(FontFactory.HELVETICA, 14);

                    // Title
                    var titleParagraph = new Paragraph("RENTAL MANAGEMENT SYSTEM AGREEMENT", boldFont);
                    titleParagraph.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                    document.Add(titleParagraph);
                    document.Add(new Paragraph(" ")); // Add spacing

                    // Date
                    document.Add(new Paragraph($"Date: {OwnerTenantAgreementDetailData.OwnerDatecreated:yyyy-MM-dd}", regularFont));
                    document.Add(new Paragraph(" ")); // Add spacing

                    // Property Owner Details
                    document.Add(new Paragraph($"Property: {OwnerTenantAgreementDetailData.Propertyhousename}", boldFont));
                    document.Add(new Paragraph($"Name: {OwnerTenantAgreementDetailData.Fullname}", regularFont));
                    document.Add(new Paragraph($"Address: {OwnerTenantAgreementDetailData.Countyname}-{OwnerTenantAgreementDetailData.Subcountyname}-{OwnerTenantAgreementDetailData.Subcountywardname}", regularFont));
                    document.Add(new Paragraph($"Phone: {OwnerTenantAgreementDetailData.Phonenumber}", regularFont));
                    document.Add(new Paragraph($"Email: {OwnerTenantAgreementDetailData.Emailaddress}", regularFont));
                    document.Add(new Paragraph(" ")); // Add spacing

                    // Rental Management System Provider
                    document.Add(new Paragraph("Rental Management System Provider:", boldFont));
                    document.Add(new Paragraph("Name: UTTAMB SOLUTIONS LIMITED", regularFont));
                    document.Add(new Paragraph("Address: Nairobi Kenya", regularFont));
                    document.Add(new Paragraph("Phone: 0717850720", regularFont));
                    document.Add(new Paragraph("Email: support@utambsolutions.com", regularFont));
                    document.Add(new Paragraph(" ")); // Add spacing

                    // Agreement Sections
                    document.Add(new Paragraph("1. PURPOSE OF THE AGREEMENT", boldFont));
                    document.Add(new Paragraph($"The purpose of this Agreement is to outline the terms and conditions under which UTTAMB SOLUTIONS LIMITED (hereinafter referred to as the Management System Provider) will provide rental management services to {OwnerTenantAgreementDetailData.Fullname} (hereinafter referred to as the Property Owner) for the property located at {OwnerTenantAgreementDetailData.Countyname}-{OwnerTenantAgreementDetailData.Subcountyname}-{OwnerTenantAgreementDetailData.Subcountywardname} (hereinafter referred to as the Property).", regularFont));
                    document.Add(new Paragraph(" ")); // Add spacing

                    document.Add(new Paragraph("2. SERVICES PROVIDED", boldFont));
                    document.Add(new Paragraph("- Advertising and Marketing: Listing the Property on various platforms to attract potential tenants.", regularFont));
                    document.Add(new Paragraph("- Tenant Screening: Conducting background checks and verifying tenant credentials.", regularFont));
                    document.Add(new Paragraph("- Rent Collection: Facilitating the collection of rent payments from tenants.", regularFont));
                    document.Add(new Paragraph("- Property Maintenance: Coordinating with contractors for repairs and regular maintenance of the Property.", regularFont));
                    document.Add(new Paragraph("- Reporting: Providing regular reports on the status of the Property, rent collection, and any issues that arise.", regularFont));
                    document.Add(new Paragraph(" ")); // Add spacing

                    document.Add(new Paragraph("3. FEES AND PAYMENTS", boldFont));
                    document.Add(new Paragraph("- Service Fee: The Property Owner agrees to pay the Management System Provider a service fee of 1% of the monthly rent collected.", regularFont));
                    document.Add(new Paragraph("- Subscription Payment: The Property Owner agrees to pay a subscription fee for the services rendered by the Management System Provider. The subscription fee shall be paid monthly to the following bank account:", regularFont));
                    document.Add(new Paragraph("  Bank Name: FAMILY BANK", boldFont));
                    document.Add(new Paragraph("  Pay Bill: 222111", boldFont));
                    document.Add(new Paragraph("  Account Number: 2340982", boldFont));
                    document.Add(new Paragraph("- Payment Terms: The subscription fee is due on the 10th day of each month.", regularFont));
                    document.Add(new Paragraph("- Additional Costs: Any costs related to property maintenance, legal fees, or other services not covered under this Agreement will be billed separately with the Property Owner's prior approval.", regularFont));
                    document.Add(new Paragraph(" ")); // Add spacing

                    document.Add(new Paragraph("4. PROPERTY OWNER RESPONSIBILITIES", boldFont));
                    document.Add(new Paragraph("- Property Upkeep: The Property Owner agrees to maintain the Property in a condition suitable for rental.", regularFont));
                    document.Add(new Paragraph("- Insurance: The Property Owner is responsible for obtaining and maintaining appropriate insurance coverage for the Property.", regularFont));
                    document.Add(new Paragraph("- Legal Compliance: The Property Owner agrees to comply with all local, county, and national laws relating to the rental and maintenance of the Property.", regularFont));
                    document.Add(new Paragraph(" ")); // Add spacing

                    document.Add(new Paragraph("5. DATA PROTECTION AND PRIVACY", boldFont));
                    document.Add(new Paragraph("- Compliance with Data Protection Act, 2019: The Management System Provider shall ensure that all personal data collected, processed, and stored as part of the rental management services is handled in accordance with the Data Protection Act, 2019 of Kenya.", regularFont));
                    document.Add(new Paragraph("- Data Security: Both parties agree to implement appropriate technical and organizational measures to protect personal data against unauthorized or unlawful processing, accidental loss, destruction, or damage.", regularFont));
                    document.Add(new Paragraph(" ")); // Add spacing

                    document.Add(new Paragraph("6. TERM AND TERMINATION", boldFont));
                    document.Add(new Paragraph($"- Term: This Agreement will begin on {OwnerTenantAgreementDetailData.OwnerDatecreated:yyyy-MM-dd} and will continue until terminated by either party.", regularFont));
                    document.Add(new Paragraph("- Termination: Either party may terminate this Agreement with 14 days' written notice. Upon termination, the Property Owner is responsible for any outstanding fees and obligations under this Agreement.", regularFont));
                    document.Add(new Paragraph(" ")); // Add spacing

                    document.Add(new Paragraph("7. INDEMNIFICATION", boldFont));
                    document.Add(new Paragraph("The Property Owner agrees to indemnify and hold harmless the Management System Provider from any claims, liabilities, or damages arising out of the management of the Property, except in cases of gross negligence or willful misconduct by the Management System Provider.", regularFont));
                    document.Add(new Paragraph(" ")); // Add spacing

                    document.Add(new Paragraph("8. GOVERNING LAW", boldFont));
                    document.Add(new Paragraph("This Agreement shall be governed by and construed in accordance with the laws of Kenya.", regularFont));
                    document.Add(new Paragraph(" ")); // Add spacing

                    document.Add(new Paragraph("9. ENTIRE AGREEMENT", boldFont));
                    document.Add(new Paragraph("This Agreement constitutes the entire agreement between the parties with respect to its subject matter and supersedes all prior agreements and understandings, whether written or oral.", regularFont));
                    document.Add(new Paragraph(" ")); // Add spacing

                    // Signatures
                    document.Add(new Paragraph("AGREED AND ACCEPTED", boldFont));
                    document.Add(new Paragraph(" ")); // Add spacing

                    // Add the signature lines and labels
                    // Assuming the existing code setup
                    var formattedDate = OwnerTenantAgreementDetailData.OwnerDatecreated.ToString("yyyy-MM-dd");

                    // Signatures
                    var signatureOwnerImage = iTextSharp.text.Image.GetInstance(OwnerTenantAgreementDetailData.OwnerSignatureimageurl);
                    signatureOwnerImage.ScaleToFit(200, 50);
                    document.Add(signatureOwnerImage);
                    document.Add(new Paragraph(" "));
                    document.Add(new Paragraph("Property Owner", regularFont));
                    document.Add(new Paragraph(" "));
                    document.Add(new Paragraph($"Date: {OwnerTenantAgreementDetailData.OwnerDatecreated:yyyy-MM-dd}", regularFont));
                    document.Add(new Paragraph(" "));



                    // Add the signature image
                    //var imgPath = "resources/images/mysignature.png";
                    //var signatureImage = iTextSharp.text.Image.GetInstance(imgPath);
                    //signatureImage.ScaleToFit(200, 50);
                    //document.Add(signatureImage);
                    document.Add(new Paragraph(" "));
                    document.Add(new Paragraph("Management System Provider", regularFont));
                    document.Add(new Paragraph(" "));
                    document.Add(new Paragraph($"Date: {OwnerTenantAgreementDetailData.OwnerDatecreated:yyyy-MM-dd}", regularFont));
                    document.Add(new Paragraph(" "));


                    // Close the document
                    document.Close();
                }

                // Convert memory stream to byte array
                var pdfBytes = memoryStream.ToArray();

                // Upload to Firebase Storage
                var storage = new FirebaseStorage("uttambsolutions-4ec2a.appspot.com");
                var stream = new MemoryStream(pdfBytes);

                // Sanitize the file name to avoid issues with special characters
                string sanitizedFullName = OwnerTenantAgreementDetailData.Fullname.Replace(" ", "_").Replace("/", "_").Replace("\\", "_");
                string sanitizedPropertyName = OwnerTenantAgreementDetailData.Propertyhousename.Replace(" ", "_").Replace("/", "_").Replace("\\", "_");
                var fileName = $"{sanitizedFullName}_{sanitizedPropertyName}_Owner_Agreement.pdf";
                var uploadTask = storage.Child("maqaoplus").Child("agreements").Child(fileName).PutAsync(stream);
                var downloadUrl = await uploadTask;
                return downloadUrl;
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
