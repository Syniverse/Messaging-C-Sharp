using ScgApi;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace delete_contact
{
    class Program
    {
        static async Task DeleteContact(String phoneNumber, SessionOptions opts)
        {
            using (var session = new Session(opts))
            {
                var res = Contact.Resource(session);

                // We have to use List() to search for the phone number. 
                // There should be at max one iteration of the loop.
                foreach (var contact in res.List(new Dictionary<String, String>()
                   {
                       {"primary_mdn", phoneNumber }
                   }))
                {
                    Console.WriteLine("Deleting contact {0} with mdn {1}",
                        contact.Id, contact.PrimaryMdn);
                    await res.Delete(contact.Id);
                }
            }
        }

        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: delete_contact phone [auth-file] [api-url]");
                return;
            }

            var opts = new SessionOptions();

            if (args.Length > 1)
                opts.AuthFilePath = args[1];

            if (args.Length > 2)
                opts.BaseUrl = args[2];

            try
            {
                DeleteContact(args[0], opts).Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed: {0}", ex.ToString());
            }
        }
    }
}