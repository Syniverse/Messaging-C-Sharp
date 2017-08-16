using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Reflection;


namespace ScgApi
{
    public class Serialize<objT>
    {
        JsonSerializer serializer;
        DefaultContractResolver contractResolver;

        public Serialize()
        {
            serializer = new JsonSerializer();
            contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            };
            serializer.ContractResolver = contractResolver;
        }


        public String ToJson(objT model)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                NullValueHandling = NullValueHandling.Ignore
            };

            return JsonConvert.SerializeObject(model, settings);
        }

        public objT ToModel(Stream stream)
        {
            using (var sr = new StreamReader(stream))
            {
                using (var jtr = new JsonTextReader(sr))
                {
                    return serializer.Deserialize<objT>(jtr);
                }
            }
        }

        public objT ToModel(String json)
        {
            using (var sr = new StringReader(json))
            {
                using (var jtr = new JsonTextReader(sr))
                {
                    return serializer.Deserialize<objT>(jtr);
                }
            }
        }
    }
}
