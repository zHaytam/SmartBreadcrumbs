using System;
using SmartBreadcrumbs.Attributes;
using SmartBreadcrumbs.UnitTests.Pages;
using SmartBreadcrumbs.UnitTests.Pages.SubFolder1;
using Xunit;

namespace SmartBreadcrumbs.UnitTests
{
    public class BreadcrumbAttributeTests
    {

        [Fact]
        public void ExtractFromKey_ShouldThrowException_WhenFromControllerIsNullWhileFromActionIsnt()
        {
            var type = typeof(TestClassTwo);
            var attr = new BreadcrumbAttribute
            {
                FromAction = "Method"
            };

            var ex = Assert.Throws<SmartBreadcrumbsException>(() => attr.ExtractFromKey(type));
            Assert.Equal($"Can't infer FromController as '{type.Name}' is a Razor Page.", ex.Message);
        }

        [Fact]
        public void ExtractFromKey_ShouldThrowException_WhenControllerIsntValid()
        {
            var controllerType = typeof(TestClassThree);
            var attr = new BreadcrumbAttribute
            {
                FromAction = "Test",
                FromController = controllerType
            };

            var ex = Assert.Throws<SmartBreadcrumbsException>(() => attr.ExtractFromKey(null));
            Assert.Equal($"'{controllerType.Name}' is used in FromController but isn't a Controller.", ex.Message);
        }

        [Fact]
        public void ExtractFromKey_ShouldThrowException_WhenFromActionIsntAMethodInFromController()
        {
            var controllerType = typeof(TestController);
            var attr = new BreadcrumbAttribute
            {
                FromAction = "MyActio",
                FromController = controllerType
            };

            var ex = Assert.Throws<SmartBreadcrumbsException>(() => attr.ExtractFromKey(controllerType));
            Assert.Equal($"{controllerType.Name} doesn't contain a valid action named {attr.FromAction}.", ex.Message);
        }

        [Fact]
        public void ExtractFromKey_ShouldReturnCorrectValue_WhenActionAndControllerAreUsed()
        {
            var attr = new BreadcrumbAttribute
            {
                FromAction = "MyAction",
                FromController = typeof(TestController)
            };

            Assert.Equal("Test.MyAction", attr.ExtractFromKey(null));
        }

        [Fact]
        public void ExtractFromKey_ShouldThrowException_WhenFromPageIsntARazorPage()
        {
            var fromPageType = typeof(TestClassThree);
            var attr = new BreadcrumbAttribute
            {
                FromPage = fromPageType
            };

            var ex = Assert.Throws<SmartBreadcrumbsException>(() => attr.ExtractFromKey(null));
            Assert.Equal($"'{fromPageType.Name}' is used in FromPage but isn't a Razor Page.", ex.Message);
        }

        [Theory]
        [InlineData(typeof(TestClassOne), "/SubFolder1/TestClassOne")]
        [InlineData(typeof(TestClassTwo), "/TestClassTwo")]
        public void ExtractFromKey_ShouldReturnCorrectValue_WhenFromPageIsUsed(Type fromPageType, string expectedFromKey)
        {
            var attr = new BreadcrumbAttribute
            {
                FromPage = fromPageType
            };

            Assert.Equal(expectedFromKey, attr.ExtractFromKey(null));
        }

    }
}
