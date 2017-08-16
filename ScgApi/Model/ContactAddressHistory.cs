using System;
using System.Collections.Generic;
using System.Text;

namespace ScgApi
{
    class ContactAddressHistory : ModelBase
    {
        public static String Path { get { return "consent/contact_address_history"; } }

        public static ScgApi.Resource<SenderIdType> Resource(Session session)
        {
            return new Resource<SenderIdType>(session, Path);
        }

        public String Id { get; set; }
        public String Msisdn { get; set; }
        public String SenderId { get; set; }
        public String Source { get; set; }
        public String Status { get; set; }
        public String Timestamp { get; set; }
        public String Message { get; set; }
        public String Keyword { get; set; }
        public Int64? ApplicationId { get; set; }
        public Int64? CreatedDate { get; set; }
        public Int64? LastUpdateDate { get; set; }
        public int? VersionNumber { get; set; }
    }
}
