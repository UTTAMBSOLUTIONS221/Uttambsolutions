using DBL.Models;
using Maqaoplus.Services;
namespace Maqaoplus
{
    public partial class App : Application
    {
        private readonly DeviceService _deviceService;
        private readonly Services.ServiceProvider _serviceProvider;
        public static UsermodelResponce UserDetails;
        public App(AppShell appShell, Services.ServiceProvider serviceProvider)
        {
            InitializeComponent();
            _deviceService = new DeviceService();
            _serviceProvider = serviceProvider;
            MainPage = appShell;
            CheckConnectivity();
        }
        private void CheckConnectivity()
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                // Handle no internet connection
                MainPage.DisplayAlert("No Internet", "You are not connected to the internet.", "OK");
            }
        }
        protected override async void OnStart()
        {
            base.OnStart();

            // Get device info and save it
            var deviceInfo = _deviceService.GetDeviceInfo();
            await _serviceProvider.CallCustomUnAuthWebApi("/api/PropertyHouse/Registersystempropertyhouseroomimagedata", deviceInfo);
        }
    }
}
