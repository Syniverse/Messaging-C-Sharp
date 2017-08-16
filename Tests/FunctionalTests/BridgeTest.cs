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
        public void CreateGetLisstDelete()
        {
            var res = Bridge.Resource(Session);
            var template = new Bridge()
            {
                CallIds = new List<string>()
            };

            String id = res.Create(template).Result;
            Assert.NotEmpty(id);

            var bridge = res.Get(id).Result;
            Assert.Equal(id, bridge.Id);

            Assert.True(GetNumItems(res.List()) > 0);

            res.Delete(id).Wait();
        }
    }
}
