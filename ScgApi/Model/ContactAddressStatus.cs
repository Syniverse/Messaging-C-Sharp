using System;
using System.Collections.Generic;
using System.Text;

namespace ScgApi
{
    class ContactAddressStatus : ModelBase
    {
        public static String Path { get { return "consent/contact_address_statuses"; } }

        public static ScgApi.Resource<SenderIdType> Resource(Session session)
        {
            return new Resource<SenderIdType>(session, Path);
        }

         // Read Only
        public String Id { get; set; }
        public String ConsentStatus { get; set; }
        public Int64? ApplicationId { get; set; }
        public Int64? CreatedDate { get; set; }
        public Int64? LastUpdateDate { get; set; }

        // Special
        public int? VersionNumber { get; set; }

        // Read/Write
        public String AddressType { get; set; }
        public String Address { get; set; }
        public String SenderId { get; set; }

        #region Json directives
        public bool ShouldSerializeId() { return false; }
        public bool ShouldSerializeConsentStatus() { return false; }
        public bool ShouldSerializeCreatedDate() { return false; }
        public bool ShouldSerializeLastUpdateDate() { return false; }
        public bool ShouldSerializeApplicationId() { return false; }
        #endregion
    }
}
