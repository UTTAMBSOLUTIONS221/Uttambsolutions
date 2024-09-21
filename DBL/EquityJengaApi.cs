using DBL.Entities.PaymentEntities;
using Newtonsoft.Json.Linq;

namespace DBL
{
    public class EquityJengaApi
    {
        public async Task<string> GetMPesaAuthTokenAsync(MerchantAuthenticationRequest requestData)
        {
            string authToken = "";
            var client = new System.Net.Http.HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://uat.finserve.africa/authentication/api/v3/authenticate/merchant");
            request.Headers.Add("Api-Key", "PEJp9UN+wtfg2WU09VbqCvL22QmU3d5DWdTcS6kR700dSTIfhrNgkxES+nWrFHXHTZSjU1rLsL6Tv8A6u/BIQw==");
            var content = new StringContent($@"{{""merchantCode"": ""{requestData.MerchantCode}"",""consumerSecret"": ""{requestData.ConsumerSecret}""}}", null, "application/json");
            request.Content = content;
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(jsonResponse))
            {
                if (jsonResponse != "Error!")
                {
                    dynamic data = JObject.Parse(jsonResponse);
                    authToken = data.accessToken;
                }
            }

            return authToken;
        }
    }
}
