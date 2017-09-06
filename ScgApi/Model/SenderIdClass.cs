using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ScgApi
{
    // Read Only class
    public class SenderIdClass : ModelBase
    {
        public static String Path { get { return "messaging/sender_id_classes"; } }

        public static ScgApi.Resource<SenderIdClass> Resource(Session session)
        {
            return new Resource<SenderIdClass>(session, Path);
        }

        public String Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public String Designation { get; set; }
        public String ApplicableCountries { get; set; }
        public Int64? CountryPeakThroughput { get; set; }
        public Int64? CountryPeakTotalThroughput { get; set; }
        public Int64? CountryDailyThroughput { get; set; }
        public String DeliveryWindow { get; set; }
        public Int64? LastUpdateDate { get; set; }
    }
}
