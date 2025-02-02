﻿//using DBL;
//using DBL.Helpers;
//using Quartz;

//namespace Jobs.Schedulers
//{
//    public class PublishJobandBlogtoLinkedinJob : IJob
//    {
//        private readonly IServiceProvider _provider;
//        private readonly BL bl;
//        //LinkedinHelpers linkedinHelper = new LinkedinHelpers();
//        public PublishJobandBlogtoLinkedinJob(IServiceProvider provider, IConfiguration config)
//        {
//            _provider = provider;
//            bl = new BL(Util.ShareConnectionString(config));
//        }

//        public async Task Execute(IJobExecutionContext context)
//        {
//            Logs($"{DateTime.Now} [Reminders Service called]" + Environment.NewLine);

//            await InitiateLinkedInAuthorizationAsync();
//            await Task.CompletedTask;
//        }

//        private async Task InitiateLinkedInAuthorizationAsync()
//        {
//            string redirectUri = "https://academicresearchwriters.uttambsolutions.com/linkedin/redirect";

//            try
//            {
//                // Construct the URL for LinkedIn authorization redirect
//                string authorizationUrl = $"{redirectUri}";

//                // Bypass SSL validation (for development purposes only)
//                HttpClientHandler handler = new HttpClientHandler
//                {
//                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
//                };

//                using (var client = new HttpClient(handler))
//                {
//                    var response = await client.GetAsync(authorizationUrl);

//                    if (!response.IsSuccessStatusCode)
//                    {
//                        // Handle unsuccessful request
//                        var errorContent = await response.Content.ReadAsStringAsync();
//                        throw new Exception($"Failed to initiate LinkedIn authorization: {errorContent}");
//                    }
//                }
//            }
//            catch (HttpRequestException ex) when (ex.InnerException != null)
//            {
//                // Handle any exceptions and log inner exception details
//                Console.WriteLine($"Error initiating LinkedIn authorization: {ex.Message}");
//                Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
//                throw;
//            }
//            catch (Exception ex)
//            {
//                // Handle other exceptions
//                Console.WriteLine($"Error initiating LinkedIn authorization: {ex.Message}");
//                throw;
//            }
//        }


//        public string GetAuthorizationUrl(string clientId, string redirectUri, string state)
//        {
//            return $"https://www.linkedin.com/oauth/v2/authorization?response_type=code&client_id={clientId}&redirect_uri={Uri.EscapeDataString(redirectUri)}&state={state}&scope=w_member_social";
//        }
//        public void Logs(string message)
//        {
//            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Quartz");
//            if (!Directory.Exists(path))
//            {
//                Directory.CreateDirectory(path);
//            }
//            path = Path.Combine(path, "Logs.txt");
//            using FileStream fstream = new FileStream(path, FileMode.Append);
//            using TextWriter writer = new StreamWriter(fstream);
//            writer.WriteLine(message);
//        }
//    }
//}