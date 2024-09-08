using DBL.Entities;
using DBL.Enum;
using DBL.Models;
using Maqaoplus.Constants;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Maqaoplus.ViewModels
{
    public class UserProfilePageViewModel : INotifyPropertyChanged
    {
        private readonly Services.ServiceProvider _serviceProvider;
        public string CopyrightText => $"© 2020 - {DateTime.Now.Year}  UTTAMB SOLUTIONS LIMITED";

        private SystemStaff _staffData;
        private CancellationTokenSource _cancellationTokenSource;

        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<ListModel> _systemgender;
        private ObservableCollection<ListModel> _systemmaritalstatus;
        private ObservableCollection<ListModel> _systemkinrelationship;

        public ICommand LoadCurrentUserCommand { get; }
        public ICommand UpdateCurrentUserDetailsCommand { get; }
        public ICommand SubmitCurrentUserDetailsCommand { get; }
        private bool _isProcessing;
        private bool _isSubmitProcessing;

        public SystemStaff StaffData
        {
            get => _staffData;
            set
            {
                _staffData = value;
                OnPropertyChanged();
            }
        }

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
        public bool IsProcessing
        {
            get => _isProcessing;
            set
            {
                _isProcessing = value;
                OnPropertyChanged();
                ((Command)UpdateCurrentUserDetailsCommand).ChangeCanExecute();
            }
        }
        public bool IsSubmitProcessing
        {
            get => _isSubmitProcessing;
            set
            {
                _isSubmitProcessing = value;
                OnPropertyChanged();
                ((Command)SubmitCurrentUserDetailsCommand).ChangeCanExecute();
            }
        }

        public UserProfilePageViewModel(Services.ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            LoadDropdownData();
            StaffData = new SystemStaff();
            LoadCurrentUserCommand = new Command(async () => await LoadCurrentUserDataAsync());
            UpdateCurrentUserDetailsCommand = new Command(async () => await Updateuserdetailsasync());
            SubmitCurrentUserDetailsCommand = new Command(async () => await Submituserdetailsasync());
        }


        public ObservableCollection<ListModel> Systemgender
        {
            get => _systemgender;
            set
            {
                _systemgender = value;
                OnPropertyChanged();
            }
        }
        private ListModel _selectedstaffgender;
        public ListModel Selectedstaffgender
        {
            get => _selectedstaffgender;
            set
            {
                _selectedstaffgender = value;
                // Ensure SystempropertyData is not null
                if (Systemgender != null)
                {
                    // Safely convert the selected value to long and assign it to Countyid
                    if (value != null && int.TryParse(value.Value?.ToString(), out int genderid))
                    {
                        StaffData.Genderid = genderid;
                    }
                    else
                    {
                        StaffData.Genderid = 0;
                    }

                    OnPropertyChanged(nameof(Selectedstaffgender));
                    OnPropertyChanged(nameof(StaffData.Genderid));
                }
            }
        }

        public ObservableCollection<ListModel> Systemmaritalstatus
        {
            get => _systemmaritalstatus;
            set
            {
                _systemmaritalstatus = value;
                OnPropertyChanged();
            }
        }
        private ListModel _selectedstaffmaritalstatus;
        public ListModel Selectedstaffmaritalstatus
        {
            get => _selectedstaffmaritalstatus;
            set
            {
                _selectedstaffmaritalstatus = value;
                // Ensure SystempropertyData is not null
                if (Systemmaritalstatus != null)
                {
                    // Safely convert the selected value to long and assign it to Countyid
                    if (value != null && int.TryParse(value.Value?.ToString(), out int maritalstatusid))
                    {
                        StaffData.Maritalstatusid = maritalstatusid;
                    }
                    else
                    {
                        StaffData.Maritalstatusid = 0;
                    }

                    OnPropertyChanged(nameof(Selectedstaffmaritalstatus));
                    OnPropertyChanged(nameof(StaffData.Maritalstatusid));
                }
            }
        }

        public ObservableCollection<ListModel> Systemkinrelationship
        {
            get => _systemkinrelationship;
            set
            {
                _systemkinrelationship = value;
                OnPropertyChanged();
            }
        }
        private ListModel _selectedstaffkinrelationship;
        public ListModel Selectedstaffkinrelationship
        {
            get => _selectedstaffkinrelationship;
            set
            {
                _selectedstaffkinrelationship = value;
                // Ensure SystempropertyData is not null
                if (Systemkinrelationship != null)
                {
                    // Safely convert the selected value to long and assign it to Countyid
                    if (value != null && int.TryParse(value.Value?.ToString(), out int kinrelationshipid))
                    {
                        StaffData.Kinrelationshipid = kinrelationshipid;
                    }
                    else
                    {
                        StaffData.Kinrelationshipid = 0;
                    }

                    OnPropertyChanged(nameof(Selectedstaffkinrelationship));
                    OnPropertyChanged(nameof(StaffData.Kinrelationshipid));
                }
            }
        }

        private async Task LoadCurrentUserDataAsync()
        {
            IsProcessing = true;
            IsDataLoaded = false;
            try
            {
                var response = await _serviceProvider.CallAuthWebApi<object>("/api/Account/Getsystemstaffprofiledatabyid/" + App.UserDetails.Usermodel.Userid, HttpMethod.Get, null);
                if (response != null)
                {
                    StaffData = JsonConvert.DeserializeObject<SystemStaff>(response.Data.ToString());
                    if (StaffData.Genderid > 0)
                    {
                        Selectedstaffgender = Systemgender.FirstOrDefault(x => x.Value == _staffData.Genderid.ToString());
                    }
                    if (StaffData.Maritalstatusid > 0)
                    {
                        Selectedstaffmaritalstatus = Systemmaritalstatus.FirstOrDefault(x => x.Value == _staffData.Maritalstatusid.ToString());
                    }
                    if (StaffData.Kinrelationshipid > 0)
                    {
                        Selectedstaffkinrelationship = Systemkinrelationship.FirstOrDefault(x => x.Value == _staffData.Kinrelationshipid.ToString());
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

        private async Task LoadDropdownData()
        {
            try
            {
                var SystemgenderResponse = await _serviceProvider.GetSystemDropDownData("/api/General?listType=" + ListModelType.Systemgender, HttpMethod.Get);
                var SystemmaritalstatusResponse = await _serviceProvider.GetSystemDropDownData("/api/General?listType=" + ListModelType.Systemmaritalstatus, HttpMethod.Get);
                var SystemkinrelationshipResponse = await _serviceProvider.GetSystemDropDownData("/api/General?listType=" + ListModelType.Systemkinrelationship, HttpMethod.Get);
                if (SystemgenderResponse != null)
                {
                    Systemgender = new ObservableCollection<ListModel>(SystemgenderResponse);
                }

                if (SystemmaritalstatusResponse != null)
                {
                    Systemmaritalstatus = new ObservableCollection<ListModel>(SystemmaritalstatusResponse);
                }
                if (SystemkinrelationshipResponse != null)
                {
                    Systemkinrelationship = new ObservableCollection<ListModel>(SystemkinrelationshipResponse);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task Updateuserdetailsasync()
        {
            IsProcessing = true;

            if (!IsValidInput())
            {
                IsProcessing = false;
                return;
            }
            if (StaffData == null)
            {
                IsProcessing = false;
                return;
            }

            try
            {
                IsProcessing = true;
                StaffData.Updateprofile = false;
                StaffData.Modifiedby = App.UserDetails.Usermodel.Userid;
                StaffData.Datemodified = DateTime.Now;
                // Call your registration service here
                var response = await _serviceProvider.CallUnAuthWebApi("/api/Account/Registerstaff", HttpMethod.Post, StaffData);
                if (response.StatusCode == 200)
                {
                    string userDetailStr = JsonConvert.SerializeObject(App.UserDetails);
                    Preferences.Set(nameof(App.UserDetails), userDetailStr);
                    await AppConstant.AddFlyoutMenusDetails();
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
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsProcessing = false;
            }
        }


        private async Task Submituserdetailsasync()
        {
            IsProcessing = true;

            if (!IsValidInput())
            {
                IsProcessing = false;
                return;
            }
            if (StaffData == null)
            {
                IsProcessing = false;
                return;
            }

            try
            {
                IsProcessing = true;
                StaffData.Updateprofile = false;
                StaffData.Modifiedby = App.UserDetails.Usermodel.Userid;
                StaffData.Datemodified = DateTime.Now;
                // Call your registration service here
                var response = await _serviceProvider.CallCustomUnAuthWebApi("/api/Account/Registerstaff", StaffData);
                if (response.RespStatus == 200 || response.RespStatus == 0)
                {
                    await Shell.Current.DisplayAlert("Success", response.RespMessage, "OK");
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
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsProcessing = false;
            }
        }
        private string _systemStaffFirstNameError;
        public string SystemStaffFirstNameError
        {
            get => _systemStaffFirstNameError;
            set
            {
                _systemStaffFirstNameError = value;
                OnPropertyChanged();
            }
        }
        private string _systemStaffLastNameError;
        public string SystemStaffLastNameError
        {
            get => _systemStaffLastNameError;
            set
            {
                _systemStaffLastNameError = value;
                OnPropertyChanged();
            }
        }

        private string _systemStaffEmailAddressError;
        public string SystemStaffEmailAddressError
        {
            get => _systemStaffEmailAddressError;
            set
            {
                _systemStaffEmailAddressError = value;
                OnPropertyChanged();
            }
        }
        private string _systemStaffPhonenumberError;
        public string SystemStaffPhonenumberError
        {
            get => _systemStaffPhonenumberError;
            set
            {
                _systemStaffPhonenumberError = value;
                OnPropertyChanged();
            }
        }
        private string _systemStaffIdnumberError;
        public string SystemStaffIdnumberError
        {
            get => _systemStaffIdnumberError;
            set
            {
                _systemStaffIdnumberError = value;
                OnPropertyChanged();
            }
        }
        private string _systemStaffGenderError;
        public string SystemStaffGenderError
        {
            get => _systemStaffGenderError;
            set
            {
                _systemStaffGenderError = value;
                OnPropertyChanged();
            }
        }
        private string _systemStaffMaritalstatusError;
        public string SystemStaffMaritalstatusError
        {
            get => _systemStaffMaritalstatusError;
            set
            {
                _systemStaffMaritalstatusError = value;
                OnPropertyChanged();
            }
        }

        private string _systemStaffKinnameError;
        public string SystemStaffKinnameError
        {
            get => _systemStaffKinnameError;
            set
            {
                _systemStaffKinnameError = value;
                OnPropertyChanged();
            }
        }

        private string _systemStaffKinrelationshipError;
        public string SystemStaffKinrelationshipError
        {
            get => _systemStaffKinrelationshipError;
            set
            {
                _systemStaffKinrelationshipError = value;
                OnPropertyChanged();
            }
        }

        private string _systemStaffKinphonenumberError;
        public string SystemStaffKinphonenumberError
        {
            get => _systemStaffKinphonenumberError;
            set
            {
                _systemStaffKinphonenumberError = value;
                OnPropertyChanged();
            }
        }
        private bool IsValidInput()
        {
            bool isValid = true;
            if (string.IsNullOrWhiteSpace(StaffData.Firstname))
            {
                SystemStaffFirstNameError = "Required.";
                isValid = false;
            }
            else
            {
                SystemStaffFirstNameError = null;
            }
            if (string.IsNullOrWhiteSpace(StaffData.Lastname))
            {
                SystemStaffLastNameError = "Required.";
                isValid = false;
            }
            else
            {
                SystemStaffLastNameError = null;
            }

            if (string.IsNullOrWhiteSpace(StaffData.Emailaddress))
            {
                SystemStaffEmailAddressError = "Required.";
                isValid = false;
            }
            else
            {
                SystemStaffEmailAddressError = null;
            }
            if (string.IsNullOrWhiteSpace(StaffData.Phonenumber))
            {
                SystemStaffPhonenumberError = "Required.";
                isValid = false;
            }
            else
            {
                SystemStaffPhonenumberError = null;
            }
            if (StaffData.Idnumber == 0)
            {
                SystemStaffIdnumberError = "Required.";
                isValid = false;
            }
            else if (StaffData.Idnumber.ToString().Length < 8)
            {
                SystemStaffIdnumberError = "Id number must be from 8 characters.";
                isValid = false;
            }
            else
            {
                SystemStaffIdnumberError = null;
            }

            if (Selectedstaffgender == null)
            {
                SystemStaffGenderError = "Required.";
                isValid = false;
            }
            else
            {
                SystemStaffGenderError = null;
            }
            if (Selectedstaffmaritalstatus == null)
            {
                SystemStaffMaritalstatusError = "Required.";
                isValid = false;
            }
            else
            {
                SystemStaffMaritalstatusError = null;
            }
            if (string.IsNullOrWhiteSpace(StaffData.Kinname))
            {
                SystemStaffKinnameError = "Required.";
                isValid = false;
            }
            else
            {
                SystemStaffKinnameError = null;
            }
            if (string.IsNullOrWhiteSpace(StaffData.Kinphonenumber))
            {
                SystemStaffKinphonenumberError = "Required.";
                isValid = false;
            }
            else
            {
                SystemStaffKinphonenumberError = null;
            }
            if (Selectedstaffkinrelationship == null)
            {
                SystemStaffKinrelationshipError = "Required.";
                isValid = false;
            }
            else
            {
                SystemStaffKinrelationshipError = null;
            }

            return isValid;
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Add this method to cancel any ongoing operations
        public void CancelOperations()
        {
            _cancellationTokenSource?.Cancel();
        }
    }
}
