namespace Mainapp.Helpers
{
    public class DevHttpConnectionHelper
    {
        public DevHttpConnectionHelper(string apiUrl)
        {
            ApiUrl = apiUrl;
            LazyHttpClient = new Lazy<HttpClient>(() => new HttpClient(GetPlatformMessageHandler()));
        }

        public string ApiUrl { get; }

        private Lazy<HttpClient> LazyHttpClient;
        public HttpClient HttpClient => LazyHttpClient.Value;

        public HttpMessageHandler? GetPlatformMessageHandler()
        {
#if WINDOWS
            return null;
#elif ANDROID
            var handler = new CustomAndroidMessageHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true; // Ignore all SSL errors
            return handler;
#else
            throw new PlatformNotSupportedException("Only Windows and Android currently supported.");
#endif
        }

#if ANDROID
        internal sealed class CustomAndroidMessageHandler : Xamarin.Android.Net.AndroidMessageHandler
        {
            protected override Javax.Net.Ssl.IHostnameVerifier GetSSLHostnameVerifier(Javax.Net.Ssl.HttpsURLConnection connection)
                => new CustomHostnameVerifier();

            private sealed class CustomHostnameVerifier : Java.Lang.Object, Javax.Net.Ssl.IHostnameVerifier
            {
                public bool Verify(string? hostname, Javax.Net.Ssl.ISSLSession? session)
                {
                    return true; // Ignore hostname verification
                }
            }
        }
#endif
    }
}
