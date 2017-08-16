using System;
using System.Collections.Generic;
using System.Text;
using ScgApi;
using System.Threading.Tasks;

namespace ScgApi
{

    public class MessageResource : Resource<Message>
    {
        public class StateRequest
        {
            public String State { get; set; }
        }

        public MessageResource(Session session, string path) : base(session, path)
        {
        }

        public async Task SetState(String messageId, String state)
        {
            var res = new Resource<StateRequest>(Session, Path);
            await res.Update(messageId, new StateRequest()
            {
                State = state
            });
        }

        public async Task SetStateProcessed(String messageId)
        {
            await SetState(messageId, "PROCESSED");
        }

        public async Task SetStateConverted(String messageId)
        {
            await SetState(messageId, "CONVERTED");
        }

        public IEnumerable<Attachment> ListAttachments(String msgId,
          Dictionary<String, String> args = null)
        {
            var res = new Resource<Attachment>(Session, GetAttachmentsPath(msgId));
            return res.List(args);
        }

        protected String GetAttachmentsPath(String msgId)
        {
            return ComposePath(msgId) + "/attachments";
        }
    }

    // Read only object
    public class MessageFragmentInfo
    {
        public String FragmentId { get; set; }
        public String FragmentState { get; set; }
        double Charge { get; set; }
        public int? FailureCode { get; set; }
        public String FailureDetails { get; set; }
        public String ProtocolError { get; set; }
        public String ExternalId { get; set; }
        public String DeliveryReportReference { get; set; }
    }

    // Read only object
    public class Message : ModelBase
    {
        public static String Path { get { return "messaging/messages"; } }

        public static MessageResource Resource(Session session)
        {
            return new MessageResource(session, Path);
        }

        public String Id { get ; set; }
        public String MessageRequestId { get; set; }
        public String ExternalTransactionIds { get; set; }
        public String ExternalMessageRequestId { get; set; }
        public String ApplicationId { get; set; }
        public String ApplicationTrackingId { get; set; }
        public String ConversationId { get; set; }
        public String CampaignId { get; set; }
        public String Direction { get; set; }
        public String CustomerSenderId { get; set; }
        public String FromAddress { get; set; }
        public String ToAddress { get; set; }
        public String State { get; set; }
        public String FailureCode { get; set; }
        public String FailureDetails { get; set; }
        public String Subject { get; set; }
        public String Body { get; set; }
        public Int64? SentDate { get; set; }
        public Int64? DeliveredDate { get; set; }
        public Int64? ConvertedDate { get; set; }
        public String ConversionInfoSource { get; set; }
        public String ReplyTo { get; set; }
        public List<String> Attachments { get; set; }
        public String Type { get; set; }
        public String MessageDeliveryProvider { get; set; }
        public String ContactId { get; set; }
        // Todo: Replace with BigDecimal compatible type when available for .NET standard 1.4
        public Double? Price { get; set; }
        public String Language { get; set; }
        public String FailedTranslation { get; set; }
        public String ProtocolError { get; set; }
        public String FailedOriginId { get; set; }
        public String Failover { get; set; }
        public String ScheduledDeliveryTime { get; set; }
        public String ExpiryTime { get; set; }
        public Int64? CreatedDate { get; set; }
        public String LastUpdateDate { get; set; }
        public List<MessageFragmentInfo> FragmentsInfo { get; set; }
    }
}
