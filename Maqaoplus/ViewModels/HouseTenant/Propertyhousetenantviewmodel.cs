using DBL;
using DBL.Entities;
using DBL.Models;
using Maqaoplus.Views.PropertyHouseTenants.Modal;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Maqaoplus.ViewModels.HouseTenant
{
    public class Propertyhousetenantviewmodel : INotifyPropertyChanged
    {
        private readonly BL _bl;
        private PropertyHouseRoomTenantData _tenantData;
        public string CopyrightText => $"© 2020 - {DateTime.Now.Year}  UTTAMB SOLUTIONS LIMITED";

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand LoadItemsCommand { get; }
        public ICommand NeedtoVacateCommand { get; }
        public ICommand OnCancelClickedCommand { get; }
        public ICommand OnOkClickedCommand { get; }

        public PropertyHouseRoomTenantData TenantData
        {
            get => _tenantData;
            set
            {
                _tenantData = value;
                OnPropertyChanged();
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
        private bool _isVacatingProcessing;
        public bool IsVacatingProcessing
        {
            get => _isVacatingProcessing;
            set
            {
                _isVacatingProcessing = value;
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
        private bool _isvisible;

        public bool Isvisible
        {
            get => _isvisible;
            set
            {
                _isvisible = value;
                OnPropertyChanged(nameof(Isvisible));
            }
        }

        public Propertyhousetenantviewmodel(BL bl)
        {
            _bl = bl;
            TenantData = new PropertyHouseRoomTenantData();
            LoadItemsCommand = new Command(async () => await LoadItems());
            NeedtoVacateCommand = new Command(async () => await NeedtoVacatethisHouseAsync());
            OnCancelClickedCommand = new Command(OnCancelClicked);
            OnOkClickedCommand = new Command(async () => await SubmitVacatingRequestAsync());
        }
        private async Task LoadItems()
        {
            IsProcessing = true;
            IsDataLoaded = false;
            try
            {
                var response = await _bl.Getsystempropertyhousetenantdatabytenantid(App.UserDetails.Usermodel.Userid);
                if (response != null)
                {
                    TenantData = response.Data;
                    if (TenantData.Tenantroomdata != null)
                    {
                        TenantData.Tenantroomdata.Expectedvacatingdate = DateTime.Now.AddMonths(TenantData.Tenantroomdata.Vacatingperioddays);
                        Isvisible = TenantData.Tenantroomdata.Occupationalstatus == "Occupant";
                    }
                    else
                    {
                        Isvisible = false;
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

        private async Task NeedtoVacatethisHouseAsync()
        {
            IsVacatingProcessing = true;
            var modalPage = new TenantVacationNoticeModalPage(this);
            await Application.Current.MainPage.Navigation.PushModalAsync(modalPage);
            IsVacatingProcessing = false;
        }
        private void OnCancelClicked()
        {
            Application.Current.MainPage.Navigation.PopModalAsync();
        }
        private string _propertyHousePlannedVacatingDateError;
        public string PropertyHousePlannedVacatingDateError
        {
            get => _propertyHousePlannedVacatingDateError;
            set
            {
                _propertyHousePlannedVacatingDateError = value;
                OnPropertyChanged();
            }
        }
        private async Task SubmitVacatingRequestAsync()
        {
            IsVacatingProcessing = true;
            if (TenantData.Tenantroomdata.Plannedvacatingdate == null || TenantData.Tenantroomdata.Plannedvacatingdate == DateTime.MinValue)
            {
                await Shell.Current.DisplayAlert("Warning", "Planned Vacating Date is required.", "OK");
                IsVacatingProcessing = false;
                return;
            }
            if (TenantData == null)
            {
                IsVacatingProcessing = false;
                return;
            }
            var tenantVacatingRequest = new SystemPropertyHouseVacatingRequest
            {
                Systempropertyhousetenantid = App.UserDetails.Usermodel.Userid,
                Systempropertyhouseroomid = TenantData.Tenantroomdata.Systempropertyhouseroomid,
                Plannedvacatingdate = TenantData.Tenantroomdata.Plannedvacatingdate,
                Expectedvacatingdate = TenantData.Tenantroomdata.Expectedvacatingdate,
                Vacatingreason = TenantData.Tenantroomdata.Systempropertyhousevacatingreason,
                Vacatingstatus = 2,
                Approvedby = 0,
                Datecreated = DateTime.UtcNow,
                Dateapproved = DateTime.UtcNow
            };
            try
            {
                var response = await _bl.Registerpropertyhousevacaterequestdata(JsonConvert.SerializeObject(tenantVacatingRequest));
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
                IsVacatingProcessing = false;
            }
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
