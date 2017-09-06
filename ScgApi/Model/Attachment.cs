using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ScgApi
{

    public class AttachmentResource : Resource<Attachment>
    {
        public AttachmentResource(Session session, string path) : base(session, path)
        {
        }

        public async Task<String> CreateAccessToken(String attachmentId)
        {
            String path = ComposePath(attachmentId) + "/access_tokens";
            var status = await Session.PostAsync(path, null);
            var result = JsonConvert.DeserializeObject<CreateResult>(
                await status.Content.ReadAsStringAsync());
            return result.id;
        }

        public async Task Upload(String attachmentId, String path, 
            string contentType = "application/octet-stream")
        {
            var token = await CreateAccessToken(attachmentId);
            using (var stream = File.OpenRead(path))
            {
                String uri = Session.BaseUrl
                    + "/scg-attachment/api/v1/messaging/attachments/"
                    + token
                    + "/content";

                var payload = new StreamContent(stream);
                payload.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                await Session.PostAsync(uri, payload);
            }
        }
    }

    public class Attachment : ModelBase
    {
        [JsonIgnore]
        public static String Path { get { return "messaging/attachments"; } }

        public static AttachmentResource Resource(Session session)
        {
            return new AttachmentResource(session, Path);
        }


        public String Id { get;  set; }
        public bool ShouldSerializeId() { return false; }
        public Int64? ApplicationId { get; set; }
        public bool ShouldSerializeApplicationId() { return false; }
        public String State { get; private set; }
        public Int64? CreatedDate { get; private set; }
        public Int64? LastUpdateDate { get; private set; }
        public Int64? VersionNumber { get; set; }
        public String Filename { get; set; }
        public String Name { get; set; }
        public String Type { get; set; }
        public Int64? Size { get; set; }
    }
}
