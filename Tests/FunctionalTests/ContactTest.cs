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
        public void CreateUpdateDelete()
        {
            // Remove any contact using the mdn we want.
            // If it exists, it's likely ti be a leftover from a failed test.
            CleanUpMdn().Wait();

            var res = Contact.Resource(Session);
            var template = new Contact()
            {
                FirstName = "John",
                LastName = "Doe",
                PrimaryMdn = Setup.mdnRangeStart.ToString()
            };

            string id = res.Create(template).Result;
            Assert.NotEmpty(id);

            Assert.True(GetNumItems(res.List()) > 0);

            var contact = res.Get(id).Result;

            Assert.Equal(template.FirstName, contact.FirstName);
            Assert.Equal(template.LastName, contact.LastName);
            Assert.Equal(template.PrimaryMdn, contact.PrimaryMdn);

            contact.FirstName = "Bob";
            res.Update(contact.Id, contact).Wait();

            var updatedContact = res.Get(id).Result;

            Assert.Equal(contact.FirstName, updatedContact.FirstName);
            Assert.Equal(contact.LastName, updatedContact.LastName);
            Assert.Equal(contact.PrimaryMdn, updatedContact.PrimaryMdn);

            res.Delete(id).Wait();

            var deleted = false;
            try
            {
                res.Get(id).Wait();
            } catch (Exception)
            {
                deleted = true;
            }

            Assert.True(deleted);
        }
    }
}
