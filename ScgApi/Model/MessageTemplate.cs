using System;
using System.Collections.Generic;
using System.Text;

namespace ScgApi
{
    class MessageTemplate : ModelBase
    {
        public static String Path { get { return "messaging/message_templates"; } }

        public static ScgApi.Resource<MessageTemplate> Resource(Session session)
        {
            return new Resource<MessageTemplate>(session, Path);
        }

        // Read Only
        public String Id { get; set; }
        public Int64? ApplicationId { get; set; }
        public Int64? CreatedDate { get; set; }
        public Int64? LastUpdateDate { get; set; }

        // Read/Write
        public String Designation { get; set; }
        public String Name { get; set; }
        public String Pattern { get; set; }

        #region Json directives
        public bool ShouldSerializeId() { return false; }
        public bool ShouldSerializeApplicationId() { return false; }
        public bool ShouldSerializeCreatedDate() { return false; }
        public bool ShouldSerializeLastUpdatedDate() { return false; }
        #endregion
    }
}
