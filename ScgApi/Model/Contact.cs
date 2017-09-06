using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScgApi
{
    public class Contact : ModelBase
    {
        public static String Path { get { return "contacts"; } }

        public static ScgApi.Resource<Contact> Resource(Session session)
        {
            return new Resource<Contact>(session, Path);
        }

        public class Address
        {
            public int? Priority { get; set; }
            public String Designation { get; set; }
            public String Use { get; set; }
            public String Source { get; set; }
            public String Status { get; set; }
            public String Line1 { get; set; }
            public String Line2 { get; set; }
            public String City { get; set; }
            public String State { get; set; }
            public String Province { get; set; }
            public String Zip { get; set; }
            public String Country { get; set; }
        }

        public class Account
        {
           public String Priority { get; set; }
           public String Designation { get; set; }
           public String Source { get; set; }
           public String State { get; set; }
           public String Username { get; set; }
           public String Domain { get; set; }
           public String AccessToken { get; set; }
        }

        public class Device
        {
            public String Priority { get; set; }
            public String Designation { get; set; }
            public String Source { get; set; }
            public String State { get; set; }
            public String Msisdn { get; set; }
            public String Carrier { get; set; }
            public String MacAddress { get; set; }
            public String Uuid { get; set; }
            public String Manufacturer { get; set; }
            public String Model { get; set; }
            public String Os { get; set; }
        }

        public class Demographic
        {
            public String Name { get; set; }
            public String Source { get; set; }
            public String Score { get; set; }
        }

        public class Interest
        {
            public String Code { get; set; }
            public String Name { get; set; }
            public String Source { get; set; }
            public String Score { get; set; }
        }

        // Read only members
        public String Id { get; set; }
        public Int64? ApplicationId { get; set; }
        // Special members
        public Int64? VersionNumber { get; set; }

        // Read/write members
        public String ExternalId { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String BirthDate { get; set; }
        public Int64? FirstAcquisitionDate { get; set; }
        public Int64 LastAcquisitionDate { get; set; }
        public String PrimaryMdn { get; set; }
        public String PrimaryAddrLine1 { get; set; }
        public String PrimaryAddrLine2 { get; set; }
        public String PrimaryAddrCity { get; set; }
        public String PrimaryAddrZip { get; set; }
        public String PrimaryAddrState { get; set; }
        public String PrimaryEmailAddr { get; set; }
        public String PrimarySocialHandle { get; set; }
        public List<Address> AddressList { get; set; }
        public List<Account> AccountList { get; set; }
        public List<Device> DeviceList { get; set; }
        public List<Interest> InterestList { get; set; }
        public List<Demographic> DemographicList { get; set; }
        public String ExtendedAttributes { get; set; }
        public String VoicePreference { get; set; }
        public String PreferredLanguage { get; set; }
        public List<String> SocialHandles { get; set; }
        public Dictionary<String, String> FastAccess { get; set; }
        public String FastAccess1 { get; set; }
        public String FastAccess2 { get; set; }
        public String FastAccess3 { get; set; }
        public String FastAccess4 { get; set; }
        public String FastAccess5 { get; set; }
        public String FastAccess6 { get; set; }
        public String FastAccess7 { get; set; }
        public String FastAccess8 { get; set; }
        public String FastAccess9 { get; set; }
        public String FastAccess10 { get; set; }
        public String FastAccess11 { get; set; }
        public String FastAccess12 { get; set; }
        public String FastAccess13 { get; set; }
        public String FastAccess14 { get; set; }
        public String FastAccess15 { get; set; }
        public String FastAccess16 { get; set; }
        public String FastAccess17 { get; set; }
        public String FastAccess18 { get; set; }
        public String FastAccess19 { get; set; }
        public String FastAccess20 { get; set; }

        #region Json directives
        public bool ShouldSerializeId() { return false; }
        public bool ShouldSerializeApplicationId() { return false; }
        #endregion
    }
}
