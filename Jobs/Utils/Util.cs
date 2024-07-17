namespace Jobs
{
    public class Util
    {
        public static string ShareConnectionString(IConfiguration config)
        {
            return config["ConnectionStrings:DatabaseConnection"];
        }
        public static void LogError(string userName, Exception ex, bool isError = true)
        {
            try
            {
                string logDir = Path.Combine(Directory.GetCurrentDirectory(), "logs");

                //---- Create Directory if it does not exist              
                if (!Directory.Exists(logDir))
                {
                    Directory.CreateDirectory(logDir);
                }
                string logFile = Path.Combine(logDir, "ErrorLog.log");
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
            catch (Exception e)
            {
                throw e;
            }
        }

        public static string GetLogFile()
        {
            try
            {
                string logDir = Path.Combine(Directory.GetCurrentDirectory(), "logs");

                //---- Create Directory if it does not exist              
                if (!Directory.Exists(logDir))
                {
                    Directory.CreateDirectory(logDir);
                }
                return Path.Combine(logDir, "ErrorLog.log");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static UsermodelResponce GetCurrentUserData(IEnumerable<ClaimsIdentity> claims)
        {
            // Find the claim that contains the user data
            var claim = claims.FirstOrDefault(u => u.IsAuthenticated && u.HasClaim(c => c.Type == "userData"))?.FindFirst("userData");

            // If the claim is not found or the value is null/empty, return null
            if (claim == null || string.IsNullOrEmpty(claim.Value))
                return null;

            // Deserialize the user data JSON to the UsermodelResponce object
            var userData = JsonConvert.DeserializeObject<UsermodelResponce>(claim.Value);

            // Perform any additional processing if needed
            return userData;
        }

    }

    public class Alert
    {
        public const string TempDataKey = "TempDataAlerts";
        public string? AlertStyle { get; set; }
        public string? Message { get; set; }
        public bool Dismissable { get; set; }
        public string? IconClass { get; set; }
    }

    public static class AlertStyles
    {
        public const string Success = "success";
        public const string Information = "info";
        public const string Warning = "warning";
        public const string Danger = "danger";
    }
}
