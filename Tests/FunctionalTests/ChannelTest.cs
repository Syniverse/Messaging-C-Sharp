using ScgApi;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FunctionalTests
{
    public class ChannelTest : FunctionalTestBase
    {
        [Fact]
        public void CreateGetListUpdateDelete()
        {
            var res = Channel.Resource(Session);
            var template = new Channel()
            {
                Name = "ci-test"
            };

            String id = res.Create(template).Result;
            Assert.NotEmpty(id);

            var channel = res.Get(id).Result;

            Assert.Equal(id, channel.Id);
            Assert.Equal("ci-test", channel.Name);

            Assert.True(GetNumItems(res.List()) > 0);

            channel.Name = "Updated Name";
            res.Update(channel.Id, channel).Wait();

            var updatedChannel = res.Get(id).Result;
            Assert.Equal(id, updatedChannel.Id);
            Assert.Equal("Updated Name", updatedChannel.Name);

            res.Delete(id).Wait();
        }

        [Fact]
        public void AddRemoveSenderId()
        {
            var res = Channel.Resource(Session);
            var template = new Channel()
            {
                Name = "ci-test"
            };

            String id = res.Create(template).Result;
            Assert.NotEmpty(id);

            res.AddSenderId(id, Setup.senderIdSms).Wait();

            Assert.Equal(1, GetNumItems(res.ListSenderIds(id)));

            res.DeleteSenderId(id, Setup.senderIdSms).Wait();

            Assert.Equal(0, GetNumItems(res.ListSenderIds(id)));

            res.Delete(id).Wait();
        }
    }
}
