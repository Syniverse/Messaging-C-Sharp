using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using ScgApi;

namespace FunctionalTests
{
    public class BridgeTest : FunctionalTestBase
    {
        [Fact]
        public async void CreateGetLisstDelete()
        {
            var res = Bridge.Resource(Session);
            var template = new Bridge()
            {
                CallIds = new List<string>()
            };

            String id = await res.Create(template);
            Assert.NotEmpty(id);

            var bridge = await res.Get(id);
            Assert.Equal(id, bridge.Id);

            Assert.True(GetNumItems(res.List()) > 0);

            await res.Delete(id);
        }
    }
}
