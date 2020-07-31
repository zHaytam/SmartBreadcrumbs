using SmartBreadcrumbs.Nodes;
using System.Collections.Generic;
using Xunit;

namespace SmartBreadcrumbs.UnitTests
{
    public class NodeKeyTests
    {
        [Fact]
        public void Ctor_ShouldNotThrow_WhenKeyIsMissing()
        {
            var routeValues = new Dictionary<string, string>
            {
                { "controller", "Home" },
                { "action", "Index" }
            };

            var ex = Record.Exception(() => new NodeKey(routeValues, "GET"));

            Assert.Null(ex);
        }
    }
}
