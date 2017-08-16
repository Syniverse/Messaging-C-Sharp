using ScgApi;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace send_mms
{
    class Program
    {

        static async Task<String> CreateAndUploadAttachment(Session session, String path)
        {
            var res = Attachment.Resource(session);
            var id = await res.Create(new Attachment()
            {
                Name = "test_upload",
                Filename = "cutecat.jpg",
                Type = "image/jpeg"
            });

            await res.Upload(id, path);

            Console.WriteLine("Created attachment {0}", id);
            return id;
        }

        static async Task SendMms(String senderId, String receipient, String body, 
            String attachmentPath, SessionOptions opts)
        {
            using (var session = new Session(opts))
            {
                var res = MessageRequest.Resource(session);
                String id = await res.Create(new MessageRequest()
                {
                    From = "sender_id:" + senderId,
                    To = new List<String>() { receipient },
                    Body = body,
                    Attachments = new List<string>()
                    {
                        await CreateAndUploadAttachment(session, attachmentPath)
                    }
                });

                Console.WriteLine("Created message request {0}", id);
            }
        }

        static void Main(string[] args)
        {
            if (args.Length < 4)
            {
                Console.WriteLine("Usage: send_mms senderid receipient message attachment-path [auth-file] [api-url]");
                return;
            }

            var opts = new SessionOptions();

            if (args.Length > 4)
                opts.AuthFilePath = args[4];

            if (args.Length > 5)
                opts.BaseUrl = args[5];

            try
            {
                SendMms(args[0], args[1], args[2], args[3], opts).Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed: {0}", ex.ToString());
            }
        }
    }
}