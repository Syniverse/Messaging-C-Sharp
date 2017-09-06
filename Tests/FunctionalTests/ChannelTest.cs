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
        public async void CreateGetListUpdateDelete()
        {
            var res = Channel.Resource(Session);
            var template = new Channel()
            {
                Name = "ci-test"
            };

            String id = await res.Create(template);
            Assert.NotEmpty(id);

            var channel = await res.Get(id);

            Assert.Equal(id, channel.Id);
            Assert.Equal("ci-test", channel.Name);

            Assert.True(GetNumItems(res.List()) > 0);

            channel.Name = "Updated Name";
            await res.Update(channel.Id, channel);

            var updatedChannel = await res.Get(id);
            Assert.Equal(id, updatedChannel.Id);
            Assert.Equal("Updated Name", updatedChannel.Name);

            await res.Delete(id);
        }

        [Fact]
        public async void AddRemoveSenderId()
        {
            var res = Channel.Resource(Session);
            var template = new Channel()
            {
                Name = "ci-test"
            };

            String id = await res.Create(template);
            Assert.NotEmpty(id);

            await res.AddSenderId(id, Setup.senderIdSms);

            Assert.Equal(1, GetNumItems(res.ListSenderIds(id)));

            await res.DeleteSenderId(id, Setup.senderIdSms);

            Assert.Equal(0, GetNumItems(res.ListSenderIds(id)));

            await res.Delete(id);
        }
    }
}
