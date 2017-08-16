using ScgApi;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace send_sms
{
    class Program
    {
        static async Task SendSms(String senderId, String receipient, String body, SessionOptions opts)
        {
            using (var session = new Session(opts))
            {
                var res = MessageRequest.Resource(session);
                String id = await res.Create(new MessageRequest()
                {
                    From = "sender_id:" + senderId,
                    To = new List<String>() { receipient },
                    Body = body
                });

                Console.WriteLine("Created message request {0}", id);
            }
        }

        static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Usage: send_sms senderid receipient message [auth-file] [api-url]");
                return;
            }

            var opts = new SessionOptions();

            if (args.Length > 3)
                opts.AuthFilePath = args[3];

            if (args.Length > 4)
                opts.BaseUrl = args[4];

            try
            {
                SendSms(args[0], args[1], args[2], opts).Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed: {0}", ex.ToString());
            }
        }
    }
}