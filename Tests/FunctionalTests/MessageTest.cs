using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using ScgApi;
using System.IO;

namespace FunctionalTests
{
    public class MessageTest : FunctionalTestBase
    {
        String CreateAttachment()
        {
            var res = Attachment.Resource(Session);
            var id = res.Create(new Attachment()
            {
                Name = "test_upload",
                Filename = "cutecat.jpg",
                Type = "image/jpeg"
            }).Result;

            var path = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".tmp";
            try
            {
                File.WriteAllText(path, "Cute Cat Payload");
                res.Upload(id, path).Wait();
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
        public void SendMmsListDelete()
        {
            String attachmentId = CreateAttachment();
            var res = MessageRequest.Resource(Session);
            String id = res.Create(new MessageRequest()
            {
                From = "sender_id:" + Setup.senderIdSms,
                To = new List<String>() { Setup.mdnRangeStart.ToString() },
                Body = "Hello World",
                Attachments = new List<String>() { attachmentId },
                TestMessageFlag = true
            }).Result;

            // If the server processed the request, verify the attachment
            for (int i = 0; i < 60; i++)
            {
                var mrq = res.Get(id).Result;

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
