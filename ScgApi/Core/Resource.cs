using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScgApi
{
    public class Resource<dataT>
    {
        protected Serialize<dataT> serializer = new Serialize<dataT>();
        protected Serialize<Page> pageSerializer = new Serialize<Page>();

        public Session Session { get; private set; }
        public String Path { get; private set; }

        public class Page
        {
            public List<dataT> list;
            public int limit = 0;
            public int total = 0;
        }

        public class CreateResult
        {
            public String id;
        }

        String GetFullUri(Dictionary<String, String> args, String id = null)
        {
            string uri = Path;
            if (!String.IsNullOrEmpty(id))
            {
                uri += "/" + id;
            }

            if (args != null)
            {
                int cnt = 0;
                foreach (var v in args)
                {
                    uri += (cnt++ == 0 ? "?" : "&")
                        + Uri.EscapeDataString(v.Key);

                    if (!String.IsNullOrEmpty(v.Value))
                    {
                        uri += "=" + Uri.EscapeDataString(v.Value);
                    }
                }
            }

            return uri;
        }

        public Resource(Session session, String path)
        {
            Session = session;
            Path = path;
        }

        protected String ComposePath(String id)
        {
            return Path + "/" + id;
        }

        protected void CheckStatus(HttpResponseMessage status)
        {
            if (!status.IsSuccessStatusCode)
            {
                throw new ExceptionRequestFailed("Request failed: " + status.ReasonPhrase,
                    status.StatusCode, status.ReasonPhrase);
            }
        }

        public async Task<HttpResponseMessage> PostAsync(dataT model)
        {
            var path = Path;
            String json = serializer.ToJson(model);
            var payload = new StringContent(json, Encoding.UTF8, "application/json");
            var status = await Session.PostAsync(path, payload);
            CheckStatus(status);
            return status;
        }

        public async Task<String> Create(dataT model)
        {
            var status = await PostAsync(model);
            string data = await status.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<CreateResult>(data);
            return result.id;
        }

        public async Task Update(String id, dataT model)
        {
            var path = ComposePath(id);
            String json = serializer.ToJson(model);
            var payload = new StringContent(json, Encoding.UTF8, "application/json");
            var status = await Session.PostAsync(path, payload);
            CheckStatus(status);
        }

        public async Task Replace(String id, dataT model)
        {
            var path = ComposePath(id);
            String json = serializer.ToJson(model);
            var payload = new StringContent(json, Encoding.UTF8, "application/json");
            var status = await Session.PutAsync(path, payload);
            CheckStatus(status);
        }

        public async Task<Page> ListPage(Dictionary<String, String> args = null, int offset = 0)
        {
            if (offset != 0)
            {
                if (args == null)
                {
                    args = new Dictionary<String, String>();
                }
                args["offset"] = offset.ToString();
            }

            using (var status = await Session.GetAsync(GetFullUri(args)))
            {
                CheckStatus(status);

                using (var stream = await status.Content.ReadAsStreamAsync())
                {
                    return pageSerializer.ToModel(stream);
                }
            }
        }

        public IEnumerable<dataT> List(Dictionary<String, String> args = null)
        {
            var iter = System.Reactive.Linq.Observable.Create<dataT>(
                async obs =>
                {
                    int offset = 0;
                    Page page;
                    do
                    {
                        page = await ListPage(args, offset);
                        foreach (var v in page.list)
                        {
                            obs.OnNext(v);
                        }

                        offset += page.list.Count;

                    } while (page.list.Count > 0 && page.total > offset);
                });

            return iter.ToEnumerable();
        }

        public async Task<dataT> Get(String id)
        {
            var path = ComposePath(id);
            var status = await Session.GetAsync(path);
            CheckStatus(status);
            string data = await status.Content.ReadAsStringAsync();
            var instance = serializer.ToModel(data);
            return instance;
        }

        public async Task Delete(string id)
        {
            var path = ComposePath(id);
            var status = await Session.DeleteAsync(path);
            CheckStatus(status);
        }
    }
}
