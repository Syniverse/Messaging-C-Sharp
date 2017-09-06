using ScgApi;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace list_contacts
{
    class Program
    {
        static void ListContacts(SessionOptions opts)
        {
            using (var session = new Session(opts))
            {
                var res = Contact.Resource(session);

                foreach (var contact in res.List())
                {
                    Console.WriteLine("Contact id {0},  mdn {1}",
                        contact.Id, contact.PrimaryMdn);
                }
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Usage: list_contacts [auth-file] [api-url]");
         
            var opts = new SessionOptions();

            if (args.Length > 0)
                opts.AuthFilePath = args[0];

            if (args.Length > 1)
                opts.BaseUrl = args[1];

            try
            {
                ListContacts(opts);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed: {0}", ex.ToString());
            }
        }
    }
}