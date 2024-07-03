using DBL.Models;
using Faithlink.Models;
using Newtonsoft.Json;
using System.Text;

namespace Faithlink.Services
{
    public class AuthenticationService
    {
        private readonly string BaseUrl;

        public AuthenticationService()
        {
            BaseUrl = Constants.BaseUrl;
        }

        public async Task<UsermodelResponce> Validateuser(string username, string password)
        {
            Userloginmodel obj = new Userloginmodel
            {
                username = username,
                password = password
            };
            UsermodelResponce userModel = new UsermodelResponce();
            var resp = await POSTTOAPILOGIN("/api/Account/Authenticate", obj);
            if (resp.RespStatus == 200)
            {
                userModel = new UsermodelResponce
                {
                    Token = resp.Token,
                    Usermodel = resp.Usermodel,
                    RespStatus = resp.RespStatus,
                    RespMessage = resp.RespMessage,
                };
            }
            else
            {
                userModel.RespStatus = 401;
                userModel.RespMessage = "Incorrect Password!";
            }

            return userModel;
        }

        public async Task<UsermodelResponce> POSTTOAPILOGIN(string endpoint, dynamic obj)
        {
            UsermodelResponce resp = new UsermodelResponce();
            using (var httpClient = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync(BaseUrl + endpoint, content))
                {
                    string outPut = response.Content.ReadAsStringAsync().Result;
                    resp = JsonConvert.DeserializeObject<UsermodelResponce>(outPut);
                }
            }
            return resp;
        }
    }
}