using Faithlink.Entities;
using Faithlink.Models;
using Newtonsoft.Json;
using System.Text;

namespace Faithlink.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _httpClient;

        public AuthenticationService()
        {
            _httpClient = new HttpClient();
            // Configure HttpClient, base address, headers, etc.
        }

        public async Task<UsermodelResponce> Validateuser(string email, string password)
        {
            Userloginmodel obj = new Userloginmodel
            {
                username = email,
                password = password
            };
            UsermodelResponce resp = new UsermodelResponce();
            using (var httpClient = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync("http://mainapi.uttambsolutions.com/api/Account/Authenticate", content))
                {
                    string outPut = response.Content.ReadAsStringAsync().Result;
                    resp = JsonConvert.DeserializeObject<UsermodelResponce>(outPut);
                }
            }
            return resp;
        }
    }
}
