using ScgApi;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FunctionalTests
{
    public class ContactGroupTest : FunctionalTestBase
    {
        String bobId;
        String aliceId;


        async Task CleanUpContacts()
        {
            await CleanUpMdn(Setup.mdnRangeStart);
            await CleanUpMdn(Setup.mdnRangeStart + 1);
        }

        async Task PrepareContacts()
        {
            var res = Contact.Resource(Session);
            bobId = await res.Create(new Contact()
            {
                FirstName = "Bob",
                PrimaryMdn = Setup.mdnRangeStart.ToString()
            });

            aliceId = await res.Create(new Contact()
            {
                FirstName = "Alice",
                PrimaryMdn = (Setup.mdnRangeStart + 1).ToString()
            });
        }

        [Fact]
        public async void CreateGetListUpdateDelete()
        {
            await CleanUpContacts();
            await PrepareContacts(); 

            var res = ContactGroup.Resource(Session);

            String id = await res.Create(new ContactGroup()
            {
                Name = "ci-test"
            });

            Assert.NotEmpty(id);

            var cg = await res.Get(id);

            Assert.Equal(id, cg.Id);
            Assert.Equal("ci-test", cg.Name);

            cg.Name = "Friends";
            await res.Update(id, cg);

            var updatedCg = await res.Get(id);

            Assert.Equal(id, updatedCg.Id);
            Assert.Equal("Friends", updatedCg.Name);

            Assert.True(GetNumItems(res.List()) > 0);

            Assert.Equal(0, GetNumItems(res.ListContacts(id)));
            await res.AddContact(id, bobId);
            Assert.Equal(1, GetNumItems(res.ListContacts(id)));
            await res.AddContact(id, aliceId);
            Assert.Equal(2, GetNumItems(res.ListContacts(id)));

            await res.DeleteContact(id, bobId);
            Assert.Equal(1, GetNumItems(res.ListContacts(id)));

            await res.DeleteContact(id, aliceId);
            Assert.Equal(0, GetNumItems(res.ListContacts(id)));

            Assert.True(GetNumItems(res.List()) > 0);

            await res.Delete(id);
            await CleanUpContacts();
        }
    }
}
