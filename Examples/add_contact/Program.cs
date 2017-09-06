using ScgApi;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace add_contact
{
    class Program
    {
        static async Task AddContact(String firstName, String phone, SessionOptions opts)
        {
            using (var session = new Session(opts))
            {
                var contactRes = Contact.Resource(session);
                String id = await contactRes.Create(new Contact()
                {
                    FirstName = firstName,
                    PrimaryMdn = phone
                });

                Console.WriteLine("Added contact {0} with name {1}",
                    id, firstName, firstName);
            }
        }

        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: add_contact first-name phone [auth-file] [api-url]");
                return;
            }

            var opts = new SessionOptions();

            if (args.Length > 2)
                opts.AuthFilePath = args[2];

            if (args.Length > 3)
                opts.BaseUrl = args[3];

            try
            {
                AddContact(args[0], args[1], opts).Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed: {0}", ex.ToString());
            }
        }
    }
}