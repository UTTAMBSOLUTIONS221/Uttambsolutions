using DBL.Models;
using Mainapp.Entities.Startup;
using Mainapp.Helpers;
using Newtonsoft.Json;
using System.Text;

namespace Mainapp.Services
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

        public async Task<BaseResponse<TResponse>> CallUnAuthWebApi<TRequest, TResponse>(
            string apiUrl, HttpMethod httpMethod, TRequest request)
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

                var result = JsonConvert.DeserializeObject<BaseResponse<TResponse>>(responseContent);
                result.StatusCode = (int)response.StatusCode;

                return result;
            }
            catch (Exception ex)
            {
                return new BaseResponse<TResponse>
                {
                    StatusCode = 500,
                    StatusMessage = ex.Message
                };
            }
        }

        public async Task<BaseResponse<TResponse>> CallAuthWebApi<TRequest, TResponse>(
            string apiUrl, HttpMethod httpMethod, TRequest request)
        {
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = httpMethod,
                RequestUri = new Uri(_devHttpHelper.ApiUrl + apiUrl),
                Headers = { Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _accessToken) }
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

                var result = JsonConvert.DeserializeObject<BaseResponse<TResponse>>(responseContent);
                result.StatusCode = (int)response.StatusCode;

                return result;
            }
            catch (Exception ex)
            {
                return new BaseResponse<TResponse>
                {
                    StatusCode = 500,
                    StatusMessage = ex.Message
                };
            }
        }
    }
}
