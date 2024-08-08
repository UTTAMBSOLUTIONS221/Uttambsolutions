using DBL.Entities.Mpesa;
using Newtonsoft.Json;
using System.Text;

namespace WEB.Services
{
    public class Mpesaservices
    {
        string BaseUrl = "";
        public Mpesaservices()
        {
        }
        public async Task<Object> Verifympesatransaction(TransactionStatusQueryModel obj)
        {
            var resp = await UNAUTHPOSTTOAPI("https://sandbox.safaricom.co.ke/mpesa/transactionstatus/v1/query", obj);
            return resp;
        }
        public async Task<Object> UNAUTHPOSTTOAPI(string endpoint, dynamic obj)
        {
            Object resp = new Object();
            using (var httpClient = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync(endpoint, content))
                {
                    string outPut = response.Content.ReadAsStringAsync().Result;
                    resp = JsonConvert.DeserializeObject<Object>(outPut);
                }
            }
            return resp;
        }
        public async Task<Object> POSTTOAPI(string Tokenbearer, string endpoint, dynamic obj)
        {
            Object resp = new Object();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Tokenbearer);
                var content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync(endpoint, content))
                {
                    string outPut = response.Content.ReadAsStringAsync().Result;
                    resp = JsonConvert.DeserializeObject<Object>(outPut);
                }
            }
            return resp;
        }
    }
}
