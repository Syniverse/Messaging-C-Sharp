using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ScgApi
{
    // Read Only class
    public class SenderIdType : ModelBase
    {
        public static String Path { get { return "messaging/sender_id_types"; } }

        public static ScgApi.Resource<SenderIdType> Resource(Session session)
        {
            return new Resource<SenderIdType>(session, Path);
        }

        public String Id { get; set; }

        public String Name { get; set; }

        public String Description { get; set; }

        public List<String> Capabilities { get; set; }

        public List<String> AllowedMimeTypes { get; set; }

        public List<String> BlockedMimeTypes { get; set; }

        public String GatewayId { get; set; }

        public Int64? LastUpdateDate { get; set; }

        public String CredentialParameterList { get; set; }
    }
}
