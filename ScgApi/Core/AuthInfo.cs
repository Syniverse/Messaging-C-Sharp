using Newtonsoft.Json;
using System;
using System.IO;

namespace ScgApi
{
    public class AuthInfo
    {
        [JsonProperty(PropertyName = "token")]
        public String Token { get; set; }

        // Properties for internal use
        [JsonProperty(PropertyName = "appid")]
        public String AppId { get; set; }

        [JsonProperty(PropertyName = "companyid")]
        public String CompanyId { get; set; }

        [JsonProperty(PropertyName = "transactionid")]
        public String TransactionId { get; set; }

        public static AuthInfo LoadFromFile(String path)
        {
            using (StreamReader file = File.OpenText(path))
            {
                JsonSerializer serializer = new JsonSerializer();
                return serializer.Deserialize(file, typeof(AuthInfo)) as AuthInfo;
            }
        }
    }
}
