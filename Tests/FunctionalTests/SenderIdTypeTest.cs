using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using ScgApi;

namespace FunctionalTests
{
    public class SenderIdTypeTest : FunctionalTestBase
    {
        [Fact]
        public void List()
        {
            var res = SenderIdType.Resource(Session);

            Assert.True(GetNumItems(res.List()) > 0);
        }
    }
}
