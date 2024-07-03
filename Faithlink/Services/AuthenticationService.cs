using Faithlink.Entities;
using Faithlink.Models;
using Faithlink.Services;
using Newtonsoft.Json;
using System.Net;
using System.Text;

public class AuthenticationService : IAuthenticationService
{
    private readonly HttpClient _httpClient;

    public AuthenticationService()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("https://mainapi.uttambsolutions.com/");
        // Optionally configure timeout, headers, etc. for HttpClient
    }

    public async Task<UsermodelResponce> Validateuser(string email, string password)
    {
        try
        {
            var obj = new Userloginmodel
            {
                username = email,
                password = password
            };
            UsermodelResponce resp = new UsermodelResponce();
            using (var httpClient = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync("https://mainapi.uttambsolutions.com/api/Account/Authenticate", content))
                {
                    string outPut = response.Content.ReadAsStringAsync().Result;
                    resp = JsonConvert.DeserializeObject<UsermodelResponce>(outPut);
                }
            }
            return resp;
        }
        catch (HttpRequestException ex)
        {
            // Handle HTTP request errors
            Console.WriteLine($"HTTP request failed: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            // Handle other exceptions
            Console.WriteLine($"Error occurred: {ex.Message}");
            throw;
        }
    }
}
