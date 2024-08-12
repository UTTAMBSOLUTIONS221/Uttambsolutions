﻿using DBL.Entities;
using DBL.Enum;
using DBL.Models;
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
        private SystemStaff _staffData;
        private CancellationTokenSource _cancellationTokenSource;

        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<ListModel> _systemgender;
        private ObservableCollection<ListModel> _systemmaritalstatus;
        private ObservableCollection<ListModel> _systemkinrelationship;

        public ICommand LoadCurrentUserCommand { get; }
        public ICommand UpdateCurrentUserDetailsCommand { get; }
        private bool _isProcessing;

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

        public UserProfilePageViewModel(Services.ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            LoadCurrentUserCommand = new Command(async () => await LoadCurrentUserDataAsync());
            LoadDropdownData();
            UpdateCurrentUserDetailsCommand = new Command(async () => await Updateuserdetailsasync());
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
                if (StaffData != null)
                {
                    // Safely convert the selected value to long and assign it to Countyid
                    if (value != null && int.TryParse(value.Value?.ToString(), out int genderid))
                    {
                        StaffData.Genderid = genderid;
                    }
                    else
                    {
                        StaffData.Genderid = StaffData.Genderid;
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
                if (StaffData != null)
                {
                    // Safely convert the selected value to long and assign it to Countyid
                    if (value != null && int.TryParse(value.Value?.ToString(), out int maritalstatusid))
                    {
                        StaffData.Maritalstatusid = maritalstatusid;
                    }
                    else
                    {
                        StaffData.Maritalstatusid = StaffData.Maritalstatusid;
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
                if (StaffData != null)
                {
                    // Safely convert the selected value to long and assign it to Countyid
                    if (value != null && int.TryParse(value.Value?.ToString(), out int kinrelationshipid))
                    {
                        StaffData.Kinrelationshipid = kinrelationshipid;
                    }
                    else
                    {
                        StaffData.Kinrelationshipid = StaffData.Kinrelationshipid;
                    }

                    OnPropertyChanged(nameof(Selectedstaffkinrelationship));
                    OnPropertyChanged(nameof(StaffData.Kinrelationshipid));
                }
            }
        }

        private async Task LoadCurrentUserDataAsync()
        {
            IsLoading = true;
            IsDataLoaded = false;
            _cancellationTokenSource = new CancellationTokenSource();

            try
            {
                var response = await _serviceProvider.CallAuthWebApi<object>("/api/Account/Getsystemstaffdetaildatabyid/" + App.UserDetails.Usermodel.Userid, HttpMethod.Get, null);

                if (response != null)
                {
                    StaffData = JsonConvert.DeserializeObject<SystemStaff>(response.Data.ToString());
                }
                IsDataLoaded = true;
            }
            catch (OperationCanceledException)
            {
                // Handle cancellation
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsLoading = false;
                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = null;
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
            IsLoading = true;

            await Task.Delay(500);
            if (StaffData == null)
            {
                IsLoading = false;
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
                    await Shell.Current.GoToAsync("//LoginPage");
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
