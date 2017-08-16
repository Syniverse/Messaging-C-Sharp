using System;
using System.Collections.Generic;
using System.Text;
using ScgApi;

namespace ScgApi
{
    public class Call : ModelBase
    {
        public static String Path { get { return "calling/calls"; } }

        public static ScgApi.Resource<Call> Resource(Session session)
        {
            return new Resource<Call>(session, Path);
        }

        public String Id { get; set; }
        public String ExternalId { get; set; }
        public Int64? StartTime { get; set; }
        public Int64? AnswerTime { get; set; }
        public Int64? EndTime { get; set; }
        public int? ChargeableDuration { get; set; }
        public String FailureCode { get; set; }
        public String FailureDetails { get; set; }
        public Int64? CreatedDate { get; set; }
        public Int64? LastUpdatedDate { get; set; }

        public int? VersionNumber { get; set; }

        public String From { get; set; }
        public String FromAddress { get; set; }
        public String To { get; set; }
        public int? AnswerTimeout { get; set; }
        public String State { get; set; }
        public String Direction { get; set; }
        public String BridgeId { get; set; }
        public bool? RecordingEnabled { get; set; }

        #region Json directives
        public bool ShouldSerializeId() { return false; }
        public bool ShouldSerializeExternalId() { return false; }
        public bool ShouldSerializeStartTime() { return false; }
        public bool ShouldSerializeAnswerTime() { return false; }
        public bool ShouldSerializeEndTime() { return false; }
        public bool ShouldSerializeChargeableDuration() { return false; }
        public bool ShouldSerializeFailureCode() { return false; }
        public bool ShouldSerializeFailureDetails() { return false; }
        public bool ShouldSerializeCreatedDate() { return false; }
        public bool ShouldSerializeLastUpdatedDate() { return false; }
        #endregion
    }
}
