using ScgApi;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace update_contact
{
    class Program
    {
        static async Task UpdateContact(String id, String newName, SessionOptions opts)
        {
            using (var session = new Session(opts))
            {
                var res = Contact.Resource(session);

                var contact = await res.Get(id);

                string oldName = contact.FirstName;
                contact.FirstName = newName;

                await res.Update(contact.Id, contact);

                Console.WriteLine("Updated the name for contact {0} from {1} to {2}",
                    contact.Id, oldName, contact.FirstName);
            }
        }

        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: update_contact contact-id new-first-name [auth-file] [api-url]");
                return;
            }

            var opts = new SessionOptions();

            if (args.Length > 2)
                opts.AuthFilePath = args[2];

            if (args.Length > 3)
                opts.BaseUrl = args[3];

            try
            {
                UpdateContact(args[0], args[1], opts).Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed: {0}", ex.ToString());
            }
        }
    }
}