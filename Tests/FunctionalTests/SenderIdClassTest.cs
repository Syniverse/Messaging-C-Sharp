using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using ScgApi;

namespace FunctionalTests
{
    public class SenderIdClassTest : FunctionalTestBase
    {
        [Fact]
        public void List()
        {
            var res = SenderIdClass.Resource(Session);

            Assert.True(GetNumItems(res.List()) > 0);
        }
    }
}
