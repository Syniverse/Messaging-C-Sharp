using ScgApi;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace send_sms_to_grp
{
    class Program
    {   
        static async Task<String> CreateGroup(Session session, String name)
        {
            var res = ContactGroup.Resource(session);
            String id = await res.Create(new ContactGroup()
            {
                Name = name
            });

            return id;
        }

        static async Task SendSmsToGroup(String senderId, String aliceMdn, 
            String bobMdn, String body, SessionOptions opts)
        {
            using (var session = new Session(opts))
            {
                // Create a group
                var grpRes = ContactGroup.Resource(session);
                String friendsId = await grpRes.Create(new ContactGroup()
                {
                    Name = "Friends"
                });

                // Create two contacts
                var contactRes = Contact.Resource(session);
                String aliceId = await contactRes.Create(new Contact()
                {
                    FirstName = "Alice",
                    PrimaryMdn = aliceMdn
                });
                String bobId = await contactRes.Create(new Contact()
                {
                    FirstName = "Bob",
                    PrimaryMdn = bobMdn
                });

                // Add the contacts to the group
                await grpRes.AddContact(friendsId, aliceId);
                await grpRes.AddContact(friendsId, bobId);

                // Send the message request
                var resMrq = MessageRequest.Resource(session);
                String mrqId = await resMrq.Create(new MessageRequest()
                {
                    From = "sender_id:" + senderId,
                    To = new List<String>() { "group:" + friendsId },
                    Body = body
                });

                Console.WriteLine("Created message request {0} to group {1}", 
                    mrqId, friendsId);
            }
        }

        static void Main(string[] args)
        {
            if (args.Length < 4)
            {
                Console.WriteLine("Usage: send_sms senderid receipient1 receipient2 message [auth-file] [api-url]");
                return;
            }

            var opts = new SessionOptions();

            if (args.Length > 4)
                opts.AuthFilePath = args[4];

            if (args.Length > 5)
                opts.BaseUrl = args[5];

            try
            {
                SendSmsToGroup(args[0], args[1], args[2], args[3], opts).Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed: {0}", ex.ToString());
            }
        }
    }
}