using DBL;
using DBL.Entities;
using DBL.Models.Dashboards;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Maqaoplus.ViewModels.Dashboards
{
    public class SummaryDashBoardViewModel : INotifyPropertyChanged
    {
        private readonly BL _bl;
        private PropertyHouseSummary _dashBoardSummaryData;

        public string CopyrightText => $"© 2020 - {DateTime.Now.Year}  UTTAMB SOLUTIONS LIMITED";
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand LoadOwnerSummaryCommand { get; }
        public ICommand LoadAgentSummaryCommand { get; }

        public PropertyHouseSummary DashBoardSummaryData
        {
            get => _dashBoardSummaryData ?? new PropertyHouseSummary
            {
                Propertyhouseunits = 0,
                Systempropertyoccupiedroom = 0,
                Systempropertyvacantroom = 0,
                Rentarrears = 0,
                Uncollectedpayments = 0,
                Consumedmeters = 0,
                Propertybysummary = new List<PropertySummary>()
            };
            set
            {
                _dashBoardSummaryData = value;
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

        public SummaryDashBoardViewModel(BL bl)
        {
            _bl = bl;
            DashBoardSummaryData = new PropertyHouseSummary();
            LoadOwnerSummaryCommand = new Command(async () => await LoadOwnerSummary());
            LoadAgentSummaryCommand = new Command(async () => await LoadAgentSummary());
        }

        private async Task LoadOwnerSummary()
        {
            IsProcessing = true;
            IsDataLoaded = false;

            try
            {
                var response = await _bl.Getsystempropertyhousedashboardsummarydatabyowner(App.UserDetails.Usermodel.Userid);
                if (response != null)
                {
                    DashBoardSummaryData = JsonConvert.DeserializeObject<PropertyHouseSummary>(response.Data.ToString());
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

        private async Task LoadAgentSummary()
        {
            IsProcessing = true;
            IsDataLoaded = false;

            try
            {
                var response = await _bl.Getsystempropertyhousedashboardsummarydatabyagent(App.UserDetails.Usermodel.Userid);
                if (response != null)
                {
                    DashBoardSummaryData = JsonConvert.DeserializeObject<PropertyHouseSummary>(response.Data.ToString());
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




        public async Task Savesystemuserdevicedata(string Androidid)
        {
            var deviceInfo = new DeviceInfoModel
            {
                Userid = App.UserDetails.Usermodel.Userid,
                Androidid = Androidid,
                Manufacturer = DeviceInfo.Manufacturer,
                Model = DeviceInfo.Model,
                Osversion = DeviceInfo.VersionString,
                Platforms = DeviceInfo.Platform.ToString(),
                Devicename = DeviceInfo.Name,
                Datecreated = DateTime.UtcNow,
                Datemodified = DateTime.UtcNow
            };
            var response = await _bl.Registersystemuserdevicedata(JsonConvert.SerializeObject(deviceInfo));
        }



        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
