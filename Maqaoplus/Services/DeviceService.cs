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
                Userid = 0,
                Androidid = androidId,
                Manufacturer = DeviceInfo.Manufacturer,
                Model = DeviceInfo.Model,
                Osversion = DeviceInfo.VersionString,
                Platforms = DeviceInfo.Platform.ToString(),
                Devicename = DeviceInfo.Name,
                Datecreated = DateTime.UtcNow,
                Datemodified = DateTime.UtcNow
            };

            return deviceInfo;
#else
                    // Handle non-Android case or return default values
                    return new DeviceInfoModel();
#endif
        }
    }
}
