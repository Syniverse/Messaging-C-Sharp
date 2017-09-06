using System;
using System.Collections.Generic;
using System.Text;

namespace ScgApi
{

    public class SenderIdResource : Resource<SenderId>
    {
        public class StateRequest
        {
            public String State { get; set; }
        }

        public SenderIdResource(Session session, string path) : base(session, path)
        {
        }


    }

    public class SenderId : ModelBase
    {
        public static String Path { get { return "messaging/sender_ids"; } }

        public static SenderIdResource Resource(Session session)
        {
            return new SenderIdResource(session, Path);
        }

        // Read Only
        public String Id { get; set; }
        public Int64? ApplicationId { get; set; }
        public Int64? CreatedDate { get; set; }
        public Int64? LastUpdateDate { get; set; }

        // Read Write
        public int? VersionNumber { get; set; }
        public String ParentId { get; set; }
        public String Name { get; set; }
        public String Ownership { get; set; }
        public String ClassId { get; set; }
        public String TypeId { get; set; }
        public String State { get; set; }
        public String Address { get; set; }
        public String ContentType { get; set; }
        public List<String> MessageTemplates { get; set; }
        public String Country { get; set; }
        public String Operators { get; set; }
        public String Credentials { get; set; }
        public String TwoWayRequired { get; set; }
        public String KeepSenderAddress { get; set; }
        public String DrRequired { get; set; }
        public String ConsentManagedBy { get; set; }
        public List<String> Capabilities { get; set; }
        public Boolean CheckWhitelist { get; set; }

        #region Json directives
        public bool ShouldSerializeId() { return false; }
        public bool ShouldSerializeCreatedDate() { return false; }
        public bool ShouldSerializeLastUpdateDate() { return false; }
        public bool ShouldSerializeApplicationId() { return false; }
        #endregion

    }
}
