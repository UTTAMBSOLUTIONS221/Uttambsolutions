using Mainapp.Helpers;
using Mainapp.Services.Authenticate;
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

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest request)
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

                var result = JsonConvert.DeserializeObject<AuthenticateResponse>(responseContent);
                result.StatusCode = (int)response.StatusCode;

                if (result.StatusCode == 200)
                {
                    _accessToken = result.Token;
                }
                return result;
            }
            catch (Exception ex)
            {
                return new AuthenticateResponse
                {
                    StatusCode = 500,
                    StatusMessage = ex.Message
                };
            }
        }

        public async Task<TResponse> CallWebApi<TRequest, TResponse>(
            string apiUrl, HttpMethod httpMethod, TRequest request) where TResponse : BaseResponse
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

                var result = JsonConvert.DeserializeObject<TResponse>(responseContent);
                result.StatusCode = (int)response.StatusCode;

                return result;
            }
            catch (Exception ex)
            {
                var result = Activator.CreateInstance<TResponse>();
                result.StatusCode = 500;
                result.StatusMessage = ex.Message;
                return result;
            }
        }
    }
}
