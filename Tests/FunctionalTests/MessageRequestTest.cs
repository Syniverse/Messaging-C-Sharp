using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using ScgApi;

namespace FunctionalTests
{
    public class MessageRequestTest : FunctionalTestBase
    {
        [Fact]
        public async void CreateGetListDelete()
        {
            var res = MessageRequest.Resource(Session);
            String id = await res.Create(new MessageRequest()
            {
                From = "sender_id:" + Setup.senderIdSms,
                To = new List<String>(){Setup.mdnRangeStart.ToString()},
                Body = "Hello World",
                TestMessageFlag = true
            });

            Assert.NotEmpty(id);

            var messageRequest = await res.Get(id);

            Assert.Equal(id, messageRequest.Id);
            Assert.Equal("Hello World", messageRequest.Body);

            Assert.True(GetNumItems(res.List()) > 0);

            // If the server processed the request, assert that the
            // list of messages contains 1 item.
            for (int i = 0; i < 60; i++)
            {
                var mrq = await res.Get(id);

                var readyStates = new List<String>() { "TRANSMITTING", "COMPLETED"};
                if (readyStates.Contains(mrq.State))
                {
                    Assert.True(GetNumItems(res.ListMessages(id)) == 1);
                    break;
                }

                var failedStates = new List<String>() { "REJECTED", "CANCELED" };
                if (failedStates.Contains(mrq.State))
                {
                    break;
                }
            }

            await res.Delete(id);
        }

        //// Broken?
        //[Fact]
        //public async void CreateResume()
        //{
        //    var res = MessageRequest.Resource(Session);
        //    String id = await res.Create(new MessageRequest()
        //    {
        //        From = "sender_id:" + Setup.senderIdSms,
        //        To = new List<String>() { Setup.mdnRangeStart.ToString() },
        //        Body = "Hello World",
        //        PauseBeforeTransmit = true
        //    });

        //    for(int i = 0; i < 60; i++)
        //    {
        //        var mrq = await res.Get(id);
        //        if (mrq.State == "PAUSED")
        //        {
        //            await res.Resume(id);
        //            break;
        //        }

        //        var doneStates = new List<String>() {"REJECTED", "COMPLETED", "CANCELED"};
        //        if (doneStates.Contains(mrq.State)) {
        //            break;
        //        }

        //        System.Threading.Thread.Sleep(1000);
        //    }
        //}
    }
}
