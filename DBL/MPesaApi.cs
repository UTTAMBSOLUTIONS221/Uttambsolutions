using DBL.Entities.Mpesa;
using DBL.Enum;
using DBL.Models.Mpesa;
using DBL.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace DBL
{
    public class MPesaApi
    {
        public string LogFile { get; set; }

        public string GetMPesaAuthToken(string url, string consumerKey, string consumerSecret)
        {
            string authToken = "";

            //---- Create headers
            Dictionary<string, string> headers = new Dictionary<string, string>();
            Encoding iso = Encoding.GetEncoding("ISO-8859-1");
            Encoding utf8 = Encoding.UTF8;
            byte[] utfBytes = utf8.GetBytes(consumerKey + ":" + consumerSecret);
            byte[] isoBytes = Encoding.Convert(Encoding.UTF8, iso, utfBytes);
            string tkn = Convert.ToBase64String(isoBytes);
            headers.Add("Authorization", "Basic " + tkn);

            HttpClient1 httpClient = new HttpClient1(url, HttpClient1.RequestType.Get, headers);

            Exception error;
            var results = httpClient.SendRequest("", out error);
            if (!string.IsNullOrEmpty(results))
            {
                if (results != "Error!")
                {
                    dynamic data = JObject.Parse(results);
                    authToken = data.access_token;
                }
            }

            return authToken;
        }

        public RequestResponse RegisterC2BUrl(string url, RegisterC2BUrlRequestData data, string token)
        {
            //---- Serialize data
            string jsonData = JsonConvert.SerializeObject(data);

            ///--- Call POST
            var results = DoPost(jsonData, token, url);

            if (results.StartsWith("Error!"))
            {
                return new RequestResponse
                {
                    Status = ResponseStatus.Error,
                    Message = ""
                };
            }
            else
            {
                var respData = JsonConvert.DeserializeObject<RegisterC2BUrlResponseData>(results);
                if (respData == null)
                {
                    return new RequestResponse
                    {
                        Status = ResponseStatus.Error,
                        Message = "Unknown results!"
                    };
                }

                if (respData.ResponseDescription == "success")
                {
                    return new RequestResponse
                    {
                        Status = ResponseStatus.Success,
                        Message = ""
                    };
                }
                else
                {
                    return new RequestResponse
                    {
                        Status = ResponseStatus.Error,
                        Message = respData.ResponseDescription
                    };
                }
            }
        }

        public RequestResponse MakeB2CPayment(string url, B2CPaymentData data, string token)
        {
            //---- Serialize data
            string jsonData = JsonConvert.SerializeObject(data);

            //--- Call POST
            var results = DoPost(jsonData, token, url);

            if (results.StartsWith("Error!"))
            {
                return new RequestResponse
                {
                    Status = ResponseStatus.Error,
                    Message = results
                };
            }
            else
            {
                var respData = JsonConvert.DeserializeObject<B2CPaymentRespone>(results);
                if (respData == null)
                {
                    return new RequestResponse
                    {
                        Status = ResponseStatus.Error,
                        Message = "Unknown results!"
                    };
                }

                if (Convert.ToInt32(respData.ResponseCode) == 0)
                {
                    return new RequestResponse
                    {
                        Status = ResponseStatus.Success,
                        Message = "",
                        Data = respData.ConversationID
                    };
                }
                else
                {
                    return new RequestResponse
                    {
                        Status = ResponseStatus.Error,
                        Message = respData.ResponseDescription
                    };
                }
            }
        }

        public RequestResponse MakeExprPayment(string url, ExprPaymentData data, string token)
        {
            try
            {
                //---- Serialize data
                string jsonData = JsonConvert.SerializeObject(data);

                //--- Call POST
                var results = DoPost(jsonData, token, url);

                if (results.StartsWith("Error!"))
                {
                    return new RequestResponse
                    {
                        Status = ResponseStatus.Error,
                        Message = results
                    };
                }
                else
                {
                    var respData = JsonConvert.DeserializeObject<ExprPaymentResponse>(results);
                    if (respData == null)
                    {
                        return new RequestResponse
                        {
                            Status = ResponseStatus.Error,
                            Message = "Unknown results!"
                        };
                    }

                    if (respData.ResponseCode == 0)
                    {
                        return new RequestResponse
                        {
                            Status = ResponseStatus.Success,
                            Message = "",
                            Data = respData.CheckoutRequestID
                        };
                    }
                    else
                    {
                        return new RequestResponse
                        {
                            Status = ResponseStatus.Error,
                            Message = respData.ResponseDescription
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new RequestResponse
                {
                    Status = ResponseStatus.Error,
                    Message = ex.Message
                };
            }
        }

        public string CreateSecurityCreds(string initPassword, string certificateFile)
        {
            byte[] dataToEncrypt = Encoding.UTF8.GetBytes(initPassword);
            byte[] encryptedData = null;

            X509Certificate2 x509 = new X509Certificate2(System.Security.Cryptography.X509Certificates.X509Certificate.CreateFromCertFile(certificateFile));

            using (var cert = (RSACryptoServiceProvider)x509.PublicKey.Key)
            {

                encryptedData = cert.Encrypt(dataToEncrypt, false);
            }
            return Convert.ToBase64String(encryptedData);
        }

        private string DoPost(string jsonData, string authHeader, string url)
        {
            Util.LogRequest(LogFile, "MPesaApi.DoPost", jsonData);

            //---- Create headers
            Dictionary<string, string> headers = new Dictionary<string, string>();

            //---- Create token
            headers.Add("Authorization", "Bearer " + authHeader);

            HttpClient1 httpClient = new HttpClient1(url, HttpClient1.RequestType.Post, headers);

            Exception ex;
            var results = httpClient.SendRequest(jsonData, out ex);

            if (string.IsNullOrEmpty(results))
                throw ex;

            return results;
        }
    }
}
