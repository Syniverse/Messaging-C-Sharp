using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ScgApi
{
    public class SessionOptions
    {
        private String baseUrl = "https://api.syniverse.com";
        private String basePath = "scg-external-api/api/v1";
        private String authFilePath = "auth.json";
        private AuthInfo auth;

        public String BaseUrl { get { return baseUrl; } set { baseUrl = value; } }
        public String BasePath { get { return basePath; } set { basePath = value; } }

        public String AuthFilePath { get { return authFilePath; } set { authFilePath = value; } }

        public AuthInfo Auth
        {
            get
            {
                if (auth == null)
                {
                    auth = AuthInfo.LoadFromFile(AuthFilePath);
                }
                return auth;
            }
            set
            {
                auth = value;
            }
        }
    }

    public class Session : IDisposable
    {
        private SessionOptions options;
        private HttpClient client;
        private AuthInfo Auth { get { return options.Auth; } }

        public String BaseUrl { get { return options.BaseUrl; } }
        public String BasePath { get { return options.BasePath; } }

        public Session(AuthInfo auth, String baseUrl = null, String basePath = null)
        {
            var opts = new SessionOptions()
            {
                Auth = auth
            };

            if (baseUrl != null)
                opts.BaseUrl = baseUrl;

            if (basePath != null)
                opts.BasePath = basePath;

            Init(opts);
        }

        public Session(SessionOptions opts)
        {
            Init(opts);
        }

        private void Init(SessionOptions opts)
        {
            options = opts;
            client = new HttpClient();
            string uri = BaseUrl + "/" + BasePath;

            // HttpClient base path MUST end with slash.
            if (!uri.EndsWith("/"))
            {
                uri += "/";
            }
            client.BaseAddress = new Uri(uri);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            if (!String.IsNullOrEmpty(Auth.Token))
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Auth.Token);

            if (!String.IsNullOrEmpty(Auth.CompanyId))
                client.DefaultRequestHeaders.Add("int-companyId", Auth.CompanyId);

            if (!String.IsNullOrEmpty(Auth.AppId))
                client.DefaultRequestHeaders.Add("int-appId", Auth.AppId);

            if (!String.IsNullOrEmpty(Auth.TransactionId))
                client.DefaultRequestHeaders.Add("int-txnId", Auth.TransactionId);
        }

public async Task<HttpResponseMessage> GetAsync(String url)
        {
            return await client.GetAsync(url);
        }

        public async Task<HttpResponseMessage> PostAsync(String url, HttpContent content)
        {
            return await client.PostAsync(url, content);
        }

        public async Task<HttpResponseMessage> PutAsync(String url, HttpContent content)
        {
            return await client.PutAsync(url, content);
        }

        public async Task<HttpResponseMessage> DeleteAsync(String url)
        {
            return await client.DeleteAsync(url);
        }


        #region IDisposable Support
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            if (client != null)
            {
                client.Dispose();
                client = null;
            }
        }
        #endregion
    }
}
