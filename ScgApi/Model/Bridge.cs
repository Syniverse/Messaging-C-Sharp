using System;
using System.Collections.Generic;
using System.Text;

namespace ScgApi
{
    public class Bridge : ModelBase
    {
        public static String Path { get { return "calling/bridges"; } }

        public static ScgApi.Resource<Bridge> Resource(Session session)
        {
            return new Resource<Bridge>(session, Path);
        }

        // Read Only
        public String Id { get; set; }
        public String ExternalId { get; set; }
        public Int64? CompletedTime { get; set; }
        public Int64? ActivatedTime { get; set; }
        public Int64? CreatedDate { get; set; }
        public Int64? LastUpdatedDate { get; set; }

        // Read Write
        public int? VersionNumber { get; set; }

        public String State { get; set; }
        public bool? BridgeAudio { get; set; }
        public List<String> CallIds { get; set; }

        #region Json directives
        public bool ShouldSerializeId() { return false; }
        public bool ShouldSerializeExternalId() { return false; }
        public bool ShouldSerializeCompletedTime() { return false; }
        public bool ShouldSerializeActivatedTime() { return false; }
        public bool ShouldSerializeCreatedDate() { return false; }
        public bool ShouldSerializeLastUpdatedDate() { return false; }
        #endregion

    }
}
