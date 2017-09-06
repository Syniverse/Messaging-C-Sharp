using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using ScgApi;
using System.IO;
using System.Threading.Tasks;

namespace FunctionalTests
{
    public class MessageTest : FunctionalTestBase
    {
        async Task<String> CreateAttachment()
        {
            var res = Attachment.Resource(Session);
            var id = await res.Create(new Attachment()
            {
                Name = "test_upload",
                Filename = "cutecat.jpg",
                Type = "image/jpeg"
            });

            var path = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".tmp";
            try
            {
                File.WriteAllText(path, "Cute Cat Payload");
                await res.Upload(id, path);
            }
            finally
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }

            return id;
        }

        [Fact]
        public async void SendMmsListDelete()
        {
            String attachmentId = await CreateAttachment();
            var res = MessageRequest.Resource(Session);
            String id = await res.Create(new MessageRequest()
            {
                From = "sender_id:" + Setup.senderIdSms,
                To = new List<String>() { Setup.mdnRangeStart.ToString() },
                Body = "Hello World",
                Attachments = new List<String>() { attachmentId },
                TestMessageFlag = true
            });

            // If the server processed the request, verify the attachment
            for (int i = 0; i < 60; i++)
            {
                var mrq = await res.Get(id);

                var readyStates = new List<String>() { "TRANSMITTING", "COMPLETED" };
                if (readyStates.Contains(mrq.State))
                {
                    Assert.True(GetNumItems(res.ListMessages(id)) == 1);
                    var msgRes = Message.Resource(Session);

                    foreach (var message in res.ListMessages(id))
                    {
                        int numAttachments = 0;
                        foreach(var att in msgRes.ListAttachments(message.Id))
                        {
                            numAttachments++;
                            Assert.Equal(attachmentId, att.Id);
                        }

                        Assert.Equal(1, numAttachments);
                    }

                    break;
                }

                var failedStates = new List<String>() { "REJECTED", "CANCELED" };
                if (failedStates.Contains(mrq.State))
                {
                    break;
                }
            }
        }
    }
}
