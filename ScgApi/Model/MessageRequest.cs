using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ScgApi
{
    public class MessageRequestResource : Resource<MessageRequest>
    {
        public class StateRequest
        {
            public String State { get; set; }
        }

        public MessageRequestResource(Session session, string path) : base(session, path)
        {
        }

        // Broken?
        //public async Task SetState(String mrqId, String state)
        //{
        //    var res = new Resource<StateRequest>(Session, Path);
        //    await res.Replace(mrqId, new StateRequest()
        //    {
        //        State = state
        //    });
        //}

        //public async Task Resume(String mrqId)
        //{
        //    await SetState(mrqId, "TRANSMITTING");
        //}

        //public async Task Cancel(String mrqId)
        //{
        //    await SetState(mrqId, "CANCELED");
        //}

        public IEnumerable<Message> ListMessages(String mrqId,
           Dictionary<String, String> args = null)
        {
            var res = new Resource<Message>(Session, GetMessagesPath(mrqId));
            return res.List(args);
        }

        protected String GetMessagesPath(String mrqId)
        {
            return ComposePath(mrqId) + "/messages";
        }
    }


    // The MessageRequest as such cannot be updated, but 
    // some state changes are available from the MessageRequestResource
    public class MessageRequest : ModelBase
    {
        public static String Path { get { return "messaging/message_requests"; } }

        public static MessageRequestResource Resource(Session session)
        {
            return new MessageRequestResource(session, Path);
        }

        // These properties cannot be set at the client side
        public String Id { get; set; }
        public Int64? RecipientCount { get; set; }
        public Int64? SentCount { get; set; }
        public Int64? DeliveredCount { get; set; }
        public Int64? ReadCount { get; set; }
        public Int64? ConvertedCount { get; set; }
        public Int64? CanceledCount { get; set; }
        public Int64? FailedCount { get; set; }
        public Int64? CreatedDate { get; set; }
        public Int64? LastUpdateDate { get; set; }
        public String State { get; set; }
        public int? TranslationsCount { get; set; }
        public int? TranslationsFailedCount { get; set; }
        public int? TranslationsPerformedCount { get; set; }

        // Properties that may be set when a message-request is created
        public String From { get; set; }
        public String ConversationId { get; set; }
        public List<String> To { get; set; }
        public String CampaignId { get; set; }
        public String ProgramId { get; set; }
        public String Subject { get; set; }
        public String ApplicationId { get; set; }
        public String ExternalId { get; set; }
        public List<String> Attachments { get; set; }
        public String Body { get; set; }
        public String ConsentRequirement { get; set; }
        public String Criteria { get; set; }
        public String ScheduledDeliveryTime { get; set; }
        public String ScheduledDeliveryTimeZone { get; set; }
        public String ExpiryTime { get; set; }
        public bool? TestMessageFlag { get; set; }
        public bool? PauseBeforeTransmit { get; set; }
        public String PauseExpiryTime { get; set; }
        public List<String> ContactDeliveryAddressPriority { get; set; }
        public String Failover { get; set; }
        // Todo: Replace with BigDecimal compatible type when available for .NET standard 1.4
        public Double? PriceThreshold { get; set; }
        public List<String> SenderIdSortCriteria { get; set; }
        public String SrcLanguage { get; set; }
        public String DstLanguage { get; set; }
        public Boolean Translate { get; set; }
    }
}
