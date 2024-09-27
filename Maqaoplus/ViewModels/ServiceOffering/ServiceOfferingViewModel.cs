using DBL.Entities;
using DBL.Enum;
using DBL.Models;
using Maqaoplus.Views.ServiceOffering.Modals;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Maqaoplus.ViewModels.ServiceOffering
{
    public class ServiceOfferingViewModel
    {
        private readonly Services.ServiceProvider _serviceProvider;
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand AddServiceOfferingCommand { get; }
        public ICommand OnCancelClickedCommand { get; }
        public string CopyrightText => $"© 2020 - {DateTime.Now.Year}  UTTAMB SOLUTIONS LIMITED";


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

        private ServiceOfferings _serviceofferingsData;
        public ServiceOfferings ServiceofferingsData
        {
            get => _serviceofferingsData;
            set
            {
                if (_serviceofferingsData != value)
                {
                    _serviceofferingsData = value;
                    OnPropertyChanged(nameof(ServiceofferingsData));
                }
            }
        }

        private ObservableCollection<ListModel> _servicetype;
        public ObservableCollection<ListModel> Servicetype
        {
            get => _servicetype;
            set
            {
                _servicetype = value;
                OnPropertyChanged();
            }
        }
        private ListModel _selectedServicetype;
        public ListModel SelectedServicetype
        {
            get => _selectedServicetype;
            set
            {
                _selectedServicetype = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<ListModel> _systemcounty;
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
                if (ServiceofferingsData != null)
                {
                    // Safely convert the selected value to long and assign it to Countyid
                    if (value != null && int.TryParse(value.Value?.ToString(), out int countyId))
                    {
                        ServiceofferingsData.Countyid = countyId;
                    }
                    else
                    {
                        ServiceofferingsData.Countyid = ServiceofferingsData.Countyid;
                    }

                    OnPropertyChanged(nameof(SelectedCounty));
                    OnPropertyChanged(nameof(ServiceofferingsData.Countyid));
                    LoadSubcountyDataCountyCode();
                }
            }
        }
        private ObservableCollection<ListModel> _systemsubcounty;
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
                if (ServiceofferingsData != null)
                {
                    // Safely convert the selected value to long and assign it to Countyid
                    if (value != null && int.TryParse(value.Value?.ToString(), out int Subcountyid))
                    {
                        ServiceofferingsData.Subcountyid = Subcountyid;
                    }
                    else
                    {
                        ServiceofferingsData.Subcountyid = ServiceofferingsData.Subcountyid; ;
                    }

                    OnPropertyChanged(nameof(SelectedSubcounty));
                    OnPropertyChanged(nameof(ServiceofferingsData.Subcountyid));
                    if (!string.IsNullOrEmpty(value?.Value))
                    {
                        LoadSubcountyWardDataCountyCode();
                    }
                }
            }
        }

        private ObservableCollection<ListModel> _systemsubcountyward;
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
                if (ServiceofferingsData != null)
                {
                    // Safely convert the selected value to long and assign it to Countyid
                    if (value != null && int.TryParse(value.Value?.ToString(), out int subcountywardid))
                    {
                        ServiceofferingsData.Subcountywardid = subcountywardid;
                    }
                    else
                    {
                        ServiceofferingsData.Subcountywardid = ServiceofferingsData.Subcountywardid;
                    }

                    OnPropertyChanged(nameof(SelectedSubcountyward));
                    OnPropertyChanged(nameof(ServiceofferingsData.Subcountywardid));
                }
            }
        }




        private string _servicetypeError;
        public string ServicetypeError
        {
            get => _servicetypeError;
            set
            {
                _servicetypeError = value;
                OnPropertyChanged();
            }
        }

        public ServiceOfferingViewModel(Services.ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            LoadDropdownData();
            AddServiceOfferingCommand = new Command<ServiceOfferings>(async (service) => { var staffserviceId = service?.Staffserviceid ?? 0; await AddServiceOfferingAsync(staffserviceId); });
            OnCancelClickedCommand = new Command(OnCancelClicked);
        }

        private async Task AddServiceOfferingAsync(long Staffserviceid)
        {
            IsProcessing = true;
            if (Staffserviceid > 0)
            {
                var response = await _serviceProvider.CallAuthWebApi<object>("/api/Services/Getsystemserviceofferingdatabyid/" + Staffserviceid, HttpMethod.Get, null);
                if (response != null)
                {
                    ServiceofferingsData = JsonConvert.DeserializeObject<ServiceOfferings>(response.Data.ToString());
                    if (Staffserviceid > 0)
                    {
                        if (ServiceofferingsData != null)
                        {
                            if (ServiceofferingsData.Servicetypeid > 0)
                            {
                                SelectedServicetype = Servicetype.FirstOrDefault(x => x.Value == _serviceofferingsData.Servicetypeid.ToString());
                            }
                        }
                    }
                }
            }
            var modalPage = new AddServiceOfferingModalPage(this);
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
                var SystemservicesResponse = await _serviceProvider.GetSystemDropDownData("/api/General?listType=" + ListModelType.Systemservices, HttpMethod.Get);
                if (SystemservicesResponse != null)
                {
                    Servicetype = new ObservableCollection<ListModel>(SystemservicesResponse);
                }
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
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }
        private async Task LoadSubcountyDataCountyCode()
        {
            try
            {
                if (SelectedCounty != null && !string.IsNullOrEmpty(SelectedCounty.Value))
                {
                    var SystemsubcountyResponse = await _serviceProvider.GetSystemDropDownData("/api/General/Getdropdownitembycode?listType=" + ListModelType.SystemSubCounty + "&code=" + Convert.ToInt64(SelectedCounty.Value), HttpMethod.Get);
                    if (SystemsubcountyResponse != null)
                    {
                        Systemsubcounty = new ObservableCollection<ListModel>(SystemsubcountyResponse);
                    }
                }
                else
                {
                    var SystemsubcountyResponse = await _serviceProvider.GetSystemDropDownData("/api/General?listType=" + ListModelType.SystemSubCounty, HttpMethod.Get);
                    if (SystemsubcountyResponse != null)
                    {
                        Systemsubcounty = new ObservableCollection<ListModel>(SystemsubcountyResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }
        private async Task LoadSubcountyWardDataCountyCode()
        {
            try
            {
                // Ensure SelectedSubcounty and SelectedSubcounty.Value are not null
                if (SelectedSubcounty != null && !string.IsNullOrEmpty(SelectedSubcounty.Value))
                {
                    var url = "/api/General/Getdropdownitembycode?listType=" + ListModelType.SystemSubCountyWard + "&code=" + Convert.ToInt64(SelectedSubcounty.Value);
                    var SystemsubcountywardResponse = await _serviceProvider.GetSystemDropDownData(url, HttpMethod.Get);

                    if (SystemsubcountywardResponse != null)
                    {
                        // Assign the result to Systemsubcountyward
                        Systemsubcountyward = new ObservableCollection<ListModel>(SystemsubcountywardResponse);
                    }
                    else
                    {
                        Systemsubcountyward?.Clear(); // Clear the collection if no response data is found
                    }
                }
                else
                {
                    var SystemsubcountywardResponse = await _serviceProvider.GetSystemDropDownData("/api/General?listType=" + ListModelType.SystemSubCountyWard, HttpMethod.Get);
                    if (SystemsubcountywardResponse != null)
                    {
                        // Assign the result to Systemsubcountyward
                        Systemsubcountyward = new ObservableCollection<ListModel>(SystemsubcountywardResponse);
                    }
                    else
                    {
                        Systemsubcountyward?.Clear(); // Clear the collection if no response data is found
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the API call
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }
        private void OnCancelClicked()
        {
            Application.Current.MainPage.Navigation.PopModalAsync();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
