using DBL.Entities;
#if ANDROID
using Android.Provider;
using Android.Content;
#endif
namespace Maqaoplus.Services
{
    public class DeviceService
    {
        public DeviceInfoModel GetDeviceInfo()
        {
#if ANDROID
            // Fetch Android ID
            string androidId = Settings.Secure.GetString(Android.App.Application.Context.ContentResolver, Settings.Secure.AndroidId);

            var deviceInfo = new DeviceInfoModel
            {
                AndroidId = androidId,
                Manufacturer = DeviceInfo.Manufacturer,
                Model = DeviceInfo.Model,
                OSVersion = DeviceInfo.VersionString,
                Platform = DeviceInfo.Platform.ToString(),
                DeviceName = DeviceInfo.Name
            };

            return deviceInfo;
#else
            // Handle non-Android case or return default values
            return new DeviceInfoModel();
#endif
        }
    }
}
