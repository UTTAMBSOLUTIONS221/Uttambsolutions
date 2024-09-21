using DBL.Entities.PaymentEntities;
using Newtonsoft.Json;

namespace API.Paymentservices
{
    public class EquityJengaApiService
    {
        private readonly HttpClient _httpClient;

        public EquityJengaApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<JengaPaymentVerificationResponse> VerifyPaymentAsync(string Tokenbearer, PaymentValidationRequest obj)
        {
            JengaPaymentVerificationResponse resp = new JengaPaymentVerificationResponse();
            string BaseUrl = "https://uat.finserve.africa/v3-apis/transaction-api/v3.0/bills/validation";
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Tokenbearer);
                var content = new StringContent(JsonConvert.SerializeObject(obj), null, "application/json");
                using (var response = await httpClient.PostAsync(BaseUrl, content))
                {
                    string outPut = response.Content.ReadAsStringAsync().Result;
                    resp = JsonConvert.DeserializeObject<JengaPaymentVerificationResponse>(outPut);
                }
            }
            return resp;
        }
    }

    // Model classes for JSON responses
    public class JengaTokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
    }

    public class JengaPaymentVerificationResponse
    {
        // Define properties based on Jenga's API response structure
        public string Status { get; set; }
        public string Message { get; set; }
    }

}
