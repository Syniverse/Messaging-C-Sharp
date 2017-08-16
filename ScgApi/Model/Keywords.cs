using System;
using System.Collections.Generic;
using System.Text;

namespace ScgApi
{
    public class Keywords : ModelBase
    {
        public static String Path { get { return "messaging/keywords"; } }

        public static Resource<Keywords> Resource(Session session)
        {
            return new Resource<Keywords>(session, Path);
        }

        // Read Only
        public String Id { get; set; }
        public Int64? CreatedDate { get; set; }
        public Int64? LastUpdateDate { get; set; }
        public Int64? ApplicationId { get; set; }

        // Special
        public String VersionNumber { get; set; }

        // Read/Write
        public String Name { get; set; }
        public String Description { get; set; }
        public String Value { get; set; }

        public String Case { get; set; }
        public String SenderId { get; set; }
        public String ValidFrom { get; set; }
        public String ValidTo { get; set; }
        public String AssociatedInfo { get; set; }
        public String CampaignId { get; set; }
        public String Type { get; set; }
        public List<String> Actions { get; set; }
        public String ReplyTemplate { get; set; }

        #region Json directives
        public bool ShouldSerializeId() { return false; }
        public bool ShouldSerializeCreatedDate() { return false; }
        public bool ShouldSerializeLastUpdateDate() { return false; }
        public bool ShouldSerializeApplicationId() { return false; }
        #endregion
    }
}
