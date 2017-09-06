using ScgApi;
using System;
using System.Threading.Tasks;

namespace check_message_request_state
{
    class Program
    {
        static async Task CheckState(String messageRequestId, SessionOptions opts)
        {
            using (var session = new Session(opts))
            {
                var res = MessageRequest.Resource(session);

                var mrq = await res.Get(messageRequestId);

                Console.WriteLine("Message request {0} has state {1}",
                    mrq.Id, mrq.State);

                foreach (var msg in res.ListMessages(mrq.Id))
                {
                    Console.WriteLine("  Message {0} has state {1}",
                        msg.Id, msg.State);
                }
            }
        }

        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: check_message_request_state request-id [auth-file] [api-url]");
                return;
            }

            var opts = new SessionOptions();

            if (args.Length > 1)
                opts.AuthFilePath = args[1];

            if (args.Length > 2)
                opts.BaseUrl = args[2];

            try
            {
                CheckState(args[0], opts).Wait();
            } catch(Exception ex)
            {
                Console.WriteLine("Failed: {0}", ex.ToString());
            }
        }
    }
}