using ScgApi;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FunctionalTests
{
    public class ContactTest : FunctionalTestBase
    {

        [Fact]
        public async void CreateUpdateDelete()
        {
            // Remove any contact using the mdn we want.
            // If it exists, it's likely ti be a leftover from a failed test.
            await CleanUpMdn();

            var res = Contact.Resource(Session);
            var template = new Contact()
            {
                FirstName = "John",
                LastName = "Doe",
                PrimaryMdn = Setup.mdnRangeStart.ToString()
            };

            string id = await res.Create(template);
            Assert.NotEmpty(id);

            Assert.True(GetNumItems(res.List()) > 0);

            var contact = await res.Get(id);

            Assert.Equal(template.FirstName, contact.FirstName);
            Assert.Equal(template.LastName, contact.LastName);
            Assert.Equal(template.PrimaryMdn, contact.PrimaryMdn);

            contact.FirstName = "Bob";
            await res.Update(contact.Id, contact);

            var updatedContact = await res.Get(id);

            Assert.Equal(contact.FirstName, updatedContact.FirstName);
            Assert.Equal(contact.LastName, updatedContact.LastName);
            Assert.Equal(contact.PrimaryMdn, updatedContact.PrimaryMdn);

            await res.Delete(id);

            var deleted = false;
            try
            {
                await res.Get(id);
            } catch (Exception)
            {
                deleted = true;
            }

            Assert.True(deleted);
        }
    }
}
