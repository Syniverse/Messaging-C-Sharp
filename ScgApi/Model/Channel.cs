using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ScgApi
{

    public class AddSenderIdData
    {
        public List<String> SenderIds { get; set;  }
    }

    public class ChannelResource : Resource<Channel>
    {
        public ChannelResource(Session session, string path) : base(session, path)
        {
        }

        public IEnumerable<SenderId> ListSenderIds(String channelId,
            Dictionary<String, String> args = null)
        {
            var res = new Resource<SenderId>(Session, GetSidPath(channelId));
            return res.List(args);
        }

        public async Task AddSenderId(String channelId, String senderId)
        {
            var list = new List<String>() { senderId };
            await AddSenderIds(channelId, list);
        }

        public async Task AddSenderIds(String channelId, List<String> senderIds)
        {
            var res = new Resource<AddSenderIdData>(Session, GetSidPath(channelId));
            var data = new AddSenderIdData()
            {
                SenderIds = senderIds
            };
            await res.PostAsync(data);
        }

        public async Task DeleteSenderId(String channelId, String senderId)
        {
            var res = new Resource<Object>(Session, GetSidPath(channelId));
            await res.Delete(senderId);
        }

        protected String GetSidPath(String channelId)
        {
            return ComposePath(channelId) + "/sender_ids";
        }
    }

    public class Channel : ModelBase
    {
        public static String Path { get { return "messaging/channels"; } }

        public static ChannelResource Resource(Session session)
        {
            return new ChannelResource(session, Path);
        }

        // Read only
        public String Id { get; set; }
        public String Ownership { get; set; }
        public Int64? CreatedDate { get; set; }
        public Int64? LastUpdateDate { get; set; }
        public Int64? ApplicationId { get; set; }

        // Special
        public int? VersionNumber { get; set; }

        // Read / Write
        public String Name { get; set; }
        public String Priority { get; set; }
        public String Role { get; set; }
        public String Description { get; set; }

        #region Json directives
        public bool ShouldSerializeId() { return false; }
        public bool ShouldSerializeOwnership() { return false; }
        public bool ShouldSerializeCreatedDate() { return false; }
        public bool ShouldSerializeLastUpdateDate() { return false; }
        public bool ShouldSerializeApplicationId() { return false; }
        #endregion
    }
}
