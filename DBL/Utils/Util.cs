using PhoneNumbers;
using System.Globalization;

namespace DBL.Utils
{
    public class Util
    {
        public static string GetHexId()
        {
            SlimHexIdGenerator gen = new SlimHexIdGenerator();
            return gen.NewId();
        }

        public class SlimHexIdGenerator
        {
            private readonly DateTime _baseDate = new DateTime(2016, 1, 1);
            private readonly IDictionary<long, IList<long>> _cache = new Dictionary<long, IList<long>>();

            public string NewId()
            {
                var now = DateTime.Now.ToString("HHmmssfff");
                var daysDiff = (DateTime.Today - _baseDate).Days;
                var current = long.Parse(string.Format("{0}{1}", daysDiff, now));
                return IdGeneratorHelper.NewId(_cache, current);
            }

            static class IdGeneratorHelper
            {
                public static string NewId(IDictionary<long, IList<long>> cache, long current)
                {
                    if (cache.Any() && cache.Keys.Max() < current)
                    {
                        cache.Clear();
                    }

                    if (!cache.Any())
                    {
                        cache.Add(current, new List<long>());
                    }

                    string secondPart;
                    if (cache[current].Any())
                    {
                        var maxValue = cache[current].Max();
                        cache[current].Add(maxValue + 1);
                        secondPart = maxValue.ToString(CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        cache[current].Add(0);
                        secondPart = string.Empty;
                    }

                    var nextValueFormatted = string.Format("{0}{1}", current, secondPart);
                    return UInt64.Parse(nextValueFormatted).ToString("X");
                }
            }
        }

        public static string FormatPhoneNo(string phoneNo, string zipCode)
        {
            PhoneNumber phoneNumber = null;
            PhoneNumberUtil phoneNumberUtil = PhoneNumberUtil.GetInstance();
            string finalNumber = "";
            zipCode = zipCode.Replace("+", "");
            string isoCode = phoneNumberUtil.GetRegionCodeForCountryCode(Convert.ToInt32(zipCode));
            bool isValid = false;
            PhoneNumberType isMobile = PhoneNumberType.MOBILE;
            try
            {
                phoneNumber = phoneNumberUtil.Parse(phoneNo, isoCode);
                isValid = phoneNumberUtil.IsValidNumber(phoneNumber);
                isMobile = phoneNumberUtil.GetNumberType(phoneNumber);

            }
            catch (NumberParseException e)
            {

            }
            catch (Exception e)
            {
                //e.printStackTrace();
            }

            if (isValid && (PhoneNumberType.MOBILE == isMobile ||
                    PhoneNumberType.FIXED_LINE_OR_MOBILE == isMobile))
            {
                finalNumber = phoneNumberUtil.Format(phoneNumber, PhoneNumberFormat.E164);//.substring(1);
            }

            return finalNumber;

        }

        public static void LogError(string logFile, string userName, Exception ex, bool isError = true)
        {
            try
            {
                if (string.IsNullOrEmpty(logFile))
                    return;

                //--- Delete log if it more than 500Kb
                if (File.Exists(logFile))
                {
                    FileInfo fi = new FileInfo(logFile);
                    if ((fi.Length / 1000) > 500)
                        fi.Delete();
                }
                //--- Create stream writter
                StreamWriter stream = new StreamWriter(logFile, true);
                stream.WriteLine(string.Format("{0}|{1:dd-MMM-yyyy HH:mm:ss}|{2}|{3}",
                    isError ? "ERROR" : "INFOR",
                    DateTime.Now,
                    userName,
                    isError ? ex.ToString() : ex.Message));
                stream.Close();
            }
            catch (Exception e) { }
        }
    }
}
