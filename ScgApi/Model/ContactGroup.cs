using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ScgApi
{
    public class AddContactData
    {
        public List<String> Contacts { get; set; }
    }

    public class ContactGroupResource : Resource<ContactGroup>
    {
        public ContactGroupResource(Session session, string path) : base(session, path)
        {
        }

        public IEnumerable<Contact> ListContacts(String groupId,
            Dictionary<String, String> args = null)
        {
            var res = new Resource<Contact>(Session, GetContactsPath(groupId));
            return res.List(args);
        }

        public async Task AddContact(String groupId, String contactId)
        {
            var list = new List<String>() { contactId };
            await AddContacts(groupId, list);
        }

        public async Task AddContacts(String groupId, List<String> contactIds)
        {
            var res = new Resource<AddContactData>(Session, GetContactsPath(groupId));
            var data = new AddContactData()
            {
                Contacts = contactIds
            };
            await res.PostAsync(data);
        }

        public async Task DeleteContact(String groupId, String contactId)
        {
            var res = new Resource<Object>(Session, GetContactsPath(groupId));
            await res.Delete(contactId);
        }

        protected String GetContactsPath(String groupId)
        {
            return ComposePath(groupId) + "/contacts";
        }
    }

    public class ContactGroup : ModelBase
    {
        public static String Path { get { return "contact_groups"; } }

        public static ContactGroupResource Resource(Session session)
        {
            return new ContactGroupResource(session, Path);
        }

        public String Id { get; set; }
        public String Status { get; set; }
        public Int64? MemberCount { get; set; }
        public Int64? CreatedDate { get; set; }
        public Int64? LastUpdateDate { get; set; }
        public String Type { get; set; }
        public Int64? ApplicationId { get; set; }

        // Special members
        public int? VersionNumber { get; set; }

        // Read/write members
        public String ExternalId { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public String Criteria { get; set; }

        #region Json directives
        public bool ShouldSerializeId() { return false; }
        public bool ShouldSerializeStatus() { return false; }
        public bool ShouldSerializeMemberCount() { return false; }
        public bool ShouldSerializeType() { return false; }
        public bool ShouldSerializeCreatedDate() { return false; }
        public bool ShouldSerializeLastUpdateDate() { return false; }
        public bool ShouldSerializeApplicationId() { return false; }
        #endregion
    }
}
