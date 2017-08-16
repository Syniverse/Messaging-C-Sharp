using ScgApi;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FunctionalTests
{
    public class ContactGroupTest : FunctionalTestBase
    {
        String bobId;
        String aliceId;

        public ContactGroupTest()
        {
            CleanUpMdn(Setup.mdnRangeStart).Wait();
            CleanUpMdn(Setup.mdnRangeStart + 1).Wait();

            var res = Contact.Resource(Session);
            bobId = res.Create(new Contact()
            {
                FirstName = "Bob",
                PrimaryMdn = Setup.mdnRangeStart.ToString()
            }).Result;

            aliceId = res.Create(new Contact()
            {
                FirstName = "Alice",
                PrimaryMdn = (Setup.mdnRangeStart + 1).ToString()
            }).Result;
        }

        public new void Dispose()
        {
            var res = Contact.Resource(Session);
            res.Delete(bobId).Wait();
            res.Delete(aliceId).Wait();

            base.Dispose();
        }

        [Fact]
        public void CreateGetListUpdateDelete()
        {
            var res = ContactGroup.Resource(Session);

            String id = res.Create(new ContactGroup()
            {
                Name = "ci-test"
            }).Result;

            Assert.NotEmpty(id);

            var cg = res.Get(id).Result;

            Assert.Equal(id, cg.Id);
            Assert.Equal("ci-test", cg.Name);

            cg.Name = "Friends";
            res.Update(id, cg).Wait();

            var updatedCg = res.Get(id).Result;

            Assert.Equal(id, updatedCg.Id);
            Assert.Equal("Friends", updatedCg.Name);

            Assert.True(GetNumItems(res.List()) > 0);

            Assert.Equal(0, GetNumItems(res.ListContacts(id)));
            res.AddContact(id, bobId).Wait();
            Assert.Equal(1, GetNumItems(res.ListContacts(id)));
            res.AddContact(id, aliceId).Wait();
            Assert.Equal(2, GetNumItems(res.ListContacts(id)));

            res.DeleteContact(id, bobId).Wait();
            Assert.Equal(1, GetNumItems(res.ListContacts(id)));

            res.DeleteContact(id, aliceId).Wait();
            Assert.Equal(0, GetNumItems(res.ListContacts(id)));

            Assert.True(GetNumItems(res.List()) > 0);

            res.Delete(id).Wait();
        }
    }
}
