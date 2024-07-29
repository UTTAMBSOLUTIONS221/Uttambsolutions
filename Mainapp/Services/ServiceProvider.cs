using DBL.Models;
using Mainapp.Entities.Startup;
using Mainapp.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

                var result = JsonConvert.DeserializeObject<BaseResponse>(responseContent);
                result.StatusCode = (int)response.StatusCode;

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



        #region Cart Management

        private const string CartKey = "CartItems";

        // Add item to the cart
        public async Task AddToCartAsync(Organizationshopproductsdata item)
        {
            // Retrieve cart items from preferences
            var cartItems = GetCartItems().ToList();
            if (!cartItems.Any(x => x.Shopproductid == item.Shopproductid)) // Prevent duplicates
            {
                cartItems.Add(item);
                // Save updated cart items to preferences
                SaveCartItems(cartItems);
                await Task.CompletedTask; // Simulate async operation
            }
        }
        // Remove item from the cart
        public void RemoveFromCart(Organizationshopproductsdata item)
        {
            var cartItems = GetCartItems().ToList();
            cartItems.RemoveAll(x => x.Shopproductid == item.Shopproductid);
            SaveCartItems(cartItems);
        }

        // Get all items from the cart
        public IEnumerable<Organizationshopproductsdata> GetCartItems()
        {
            var cartJson = Preferences.Get(CartKey, "[]"); // Default to empty array if not found
            return JsonConvert.DeserializeObject<List<Organizationshopproductsdata>>(cartJson) ?? new List<Organizationshopproductsdata>();
        }

        // Clear the cart
        public void ClearCart()
        {
            Preferences.Set(CartKey, "[]"); // Set to empty array
        }

        // Save cart items to preferences
        private void SaveCartItems(List<Organizationshopproductsdata> cartItems)
        {
            var cartJson = JsonConvert.SerializeObject(cartItems);
            Preferences.Set(CartKey, cartJson);
        }

        // Checkout the cart items
        public async Task<BaseResponse> CheckoutAsync()
        {
            var cartItems = GetCartItems();
            if (!cartItems.Any())
            {
                return new BaseResponse
                {
                    StatusCode = 400,
                    StatusMessage = "Cart is empty."
                };
            }

            var apiUrl = "/api/Cart/Checkout"; // Replace with your actual API endpoint
            var response = await CallAuthWebApi("/api/Cart/Checkout", HttpMethod.Post, new { Items = cartItems });
            ClearCart(); // Clear cart after successful checkout
            return response;
        }

        #endregion
    }
}
