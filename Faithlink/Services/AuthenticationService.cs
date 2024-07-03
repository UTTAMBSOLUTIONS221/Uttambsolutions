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

            // Create HttpClient with custom HttpClientHandler
            using (var handler = new HttpClientHandler())
            {
                // Ignore SSL certificate errors (use with caution)
                handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;

                using (var httpClient = new HttpClient(handler))
                {
                    var json = JsonConvert.SerializeObject(obj);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync("https://mainapi.uttambsolutions.com/api/Account/Authenticate", content))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string outPut = await response.Content.ReadAsStringAsync();
                            resp = JsonConvert.DeserializeObject<UsermodelResponce>(outPut);
                        }
                        else
                        {
                            // Handle HTTP error response
                            Console.WriteLine($"HTTP error: {response.StatusCode}");
                            // Optionally handle error response body
                            // string errorResponse = await response.Content.ReadAsStringAsync();
                        }
                    }
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
