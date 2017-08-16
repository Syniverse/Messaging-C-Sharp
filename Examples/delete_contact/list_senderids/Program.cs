using ScgApi;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace check_message_request_state
{
    class Program
    {
        static async Task DeleteContact(String phoneNumber, SessionOptions opts)
        {
            using (var session = new Session(opts))
            {
                var res = Contact.Resource(session);

                foreach (var contact in res.List(new Dictionary<String, String>()
                   {
                       {"primary_mdn", phoneNumber }
                   }))
                {
                    Console.WriteLine("Deleting contact {} with mdn {}",
                        contact.Id, contact.PrimaryMdn);
                    await res.Delete(contact.Id);
                }
            }
        }

        static void Main(string[] args)
        {
            if (args.Length < 2)
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
                Console.WriteLine("Failed: ", ex.ToString());
            }
        }
    }
}