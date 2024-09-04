using DBL.Entities;
using DBL.Models;
using Maqaoplus.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace Maqaoplus.Services
{
    public class ServiceProvider
    {
        public string _accessToken = "";
        private DevHttpConnectionHelper _devHttpHelper;

        public ServiceProvider(DevHttpConnectionHelper devHttpHelper)
        {
            _devHttpHelper = devHttpHelper;
        }

        public async Task<UsermodelResponce> Authenticate(Userloginmodel request)
        {
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(_devHttpHelper.ApiUrl + "/api/Account/Authenticate")
            };

            if (request != null)
            {
                string jsonContent = JsonConvert.SerializeObject(request);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                httpRequestMessage.Content = httpContent;
            }

            try
            {
                var response = await _devHttpHelper.HttpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<UsermodelResponce>(responseContent);
                result.RespStatus = (int)response.StatusCode;

                if (result.RespStatus == 200)
                {
                    _accessToken = result.Token;
                }
                return result;
            }
            catch (Exception ex)
            {
                return new UsermodelResponce
                {
                    RespStatus = 500,
                    RespMessage = ex.Message
                };
            }
        }
        public async Task<Genericmodel> CallCustomUnAuthWebApi(string endpoint, dynamic obj)
        {
            Genericmodel resp = new Genericmodel();
            using (var httpClient = new HttpClient())
            {
                string json = JsonConvert.SerializeObject(obj);
                var content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync(_devHttpHelper.ApiUrl + endpoint, content))
                {
                    string outPut = response.Content.ReadAsStringAsync().Result;
                    resp = JsonConvert.DeserializeObject<Genericmodel>(outPut);
                }
            }
            return resp;
        }
        public async Task<BaseResponse> CallUnAuthWebApi<TRequest>(string apiUrl, HttpMethod httpMethod, TRequest request)
        {
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = httpMethod,
                RequestUri = new Uri(_devHttpHelper.ApiUrl + apiUrl)
            };

            if (request != null)
            {
                string jsonContent = JsonConvert.SerializeObject(request);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                httpRequestMessage.Content = httpContent;
            }

            try
            {
                var response = await _devHttpHelper.HttpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = new BaseResponse
                {
                    StatusCode = (int)response.StatusCode,
                    StatusMessage = "OK"
                };
                //var result = JsonConvert.DeserializeObject<BaseResponse>(responseContent);
                var json = JObject.Parse(responseContent);
                if (json["data"] is JArray dataArray)
                {
                    result.Data = dataArray.ToObject<List<dynamic>>();
                }
                else
                {
                    result.Data = json["data"];
                }

                return result;
            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    StatusCode = 500,
                    StatusMessage = ex.Message
                };
            }
        }

        public async Task<BaseResponse> CallAuthWebApi<TRequest>(string apiUrl, HttpMethod httpMethod, TRequest request)
        {
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = httpMethod,
                RequestUri = new Uri(_devHttpHelper.ApiUrl + apiUrl),
                Headers = { Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", App.UserDetails.Token) }
            };

            if (request != null)
            {
                string jsonContent = JsonConvert.SerializeObject(request);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                httpRequestMessage.Content = httpContent;
            }

            try
            {
                var response = await _devHttpHelper.HttpClient.SendAsync(httpRequestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = new BaseResponse
                {
                    StatusCode = (int)response.StatusCode,
                    StatusMessage = "OK"
                };
                //var result = JsonConvert.DeserializeObject<BaseResponse>(responseContent);
                var json = JObject.Parse(responseContent);
                if (json["data"] is JArray dataArray)
                {
                    result.Data = dataArray.ToObject<List<dynamic>>();
                }
                else
                {
                    result.Data = json["data"];
                }

                return result;
            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    StatusCode = 500,
                    StatusMessage = ex.Message
                };
            }
        }

        public async Task<List<ListModel>> GetSystemDropDownData(string apiUrl, HttpMethod httpMethod)
        {
            List<ListModel> list = new List<ListModel>();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.UserDetails.Token);
                using (var response = await httpClient.GetAsync(_devHttpHelper.ApiUrl + apiUrl))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    list = JsonConvert.DeserializeObject<List<ListModel>>(apiResponse);
                }
            }
            return list;
        }
    }
}
