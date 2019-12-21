using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs.Controllers.SubFolder;
using SmartBreadcrumbs.Extensions;
using SmartBreadcrumbs.UnitTests.Pages;
using SmartBreadcrumbs.UnitTests.Pages.SubFolder1;
using Xunit;

namespace SmartBreadcrumbs.UnitTests
{
    public class ReflectionExtensionsTests
    {

        #region IsController

        [Theory]
        [InlineData(typeof(TestController), true)]
        [InlineData(typeof(TestTwoController), true)]
        [InlineData(typeof(TestClassThree), false)]
        public void IsController_ShouldReturnCorrectValue(Type type, bool expected)
        {
            Assert.Equal(expected, type.IsController());
        }

        #endregion

        #region IsRazorPage

        [Theory]
        [InlineData(typeof(TestClassOne), true)]
        [InlineData(typeof(TestClassTwo), true)]
        [InlineData(typeof(TestClassThree), false)]
        public void IsRazorPage_ShouldReturnCorrectValue(Type type, bool expected)
        {
            Assert.Equal(expected, type.IsRazorPage());
        }

        #endregion

        #region IsAction

        [Theory]
        [InlineData(typeof(string), false)]
        [InlineData(typeof(Action), false)]
        [InlineData(typeof(IActionResult), true)]
        [InlineData(typeof(Task<IActionResult>), true)]
        [InlineData(typeof(Task<ActionResult>), true)]
        [InlineData(typeof(Task<JsonResult>), true)]
        [InlineData(typeof(Task<FileResult>), true)]
        [InlineData(typeof(Task<OkResult>), true)]
        [InlineData(typeof(Task<RedirectResult>), true)]
        public void IsAction_ShouldReturnCorrectValue(Type type, bool expected)
        {
            Assert.Equal(expected, type.IsAction());
        }

        #endregion

        #region ExtractRazorPageKey

        [Fact]
        public void ExtractRazorPageKey_ShouldThrowArgumentNullException_WhenTypeIsNull()
        {
            Type type = null;
            Assert.Throws<ArgumentNullException>(() => type.ExtractRazorPageKey());
        }

        [Fact]
        public void ExtractRazorPageKey_ShouldThrowSmartBreadcrumbsException_WhenFullNameDoesntContainPages()
        {
            var type = typeof(TestClassThree);
            var ex = Assert.Throws<SmartBreadcrumbsException>(() => type.ExtractRazorPageKey());
            Assert.Equal($"The full name {type.FullName} doesn't contain 'Pages'.", ex.Message);
        }

        [Theory]
        [InlineData(typeof(TestClassOne), "/SubFolder1/TestClassOne")]
        [InlineData(typeof(TestClassTwo), "/TestClassTwo")]
        [InlineData(typeof(SomePage1Model), "/SomePage1")]
        [InlineData(typeof(SomeModelPageModel), "/SomeModelPage")]
        public void ExtractRazorPageKey_ShouldReturnCorrectPath(Type type, string expectedPath)
        {
            Assert.Equal(expectedPath, type.ExtractRazorPageKey());
        }

        #endregion

        #region ExtractMvcControllerKey

        [Fact]
        public void ExtractMvcControllerKey_ShouldThrowArgumentNullException_WhenControllerTypeIsNull()
        {
            Type type = null;
            Assert.Throws<ArgumentNullException>(() => type.ExtractMvcControllerKey());
        }

        [Theory]
        [InlineData(typeof(TestController), "Test")]
        [InlineData(typeof(TestTwoController), "TestTwo")]
        public void ExtractMvcControllerKey_ShouldReturnCorrectValue(Type fromController, string expectedKey)
        {
            var defaultAction = new BreadcrumbOptions().DefaultAction;
            Assert.Equal($"{expectedKey}.{defaultAction}", fromController.ExtractMvcControllerKey());
        }

        #endregion

        #region ExtractMvcKey

        [Fact]
        public void ExtractMvcKey_ShouldThrowArgumentNullException_WhenControllerTypeIsNull()
        {
            Type type = null;
            Assert.Throws<ArgumentNullException>(() => type.ExtractMvcKey(null));
        }

        [Fact]
        public void ExtractMvcKey_ShouldThrowArgumentNullException_WhenActionMethodIsNull()
        {
            Type type = typeof(TestController);
            Assert.Throws<ArgumentNullException>(() => type.ExtractMvcKey(null));
        }

        [Theory]
        [InlineData(typeof(TestController), "MyAction", "Test.MyAction")]
        [InlineData(typeof(TestTwoController), "MyActionTwo", "TestTwo.MyActionTwo")]
        public void ExtractMvcKey_ShouldReturnCorrectValue(Type fromController, string fromAction, string expectedKey)
        {
            var actionMethod = fromController.GetMethod(fromAction, BindingFlags.Instance | BindingFlags.Public);
            Assert.Equal(expectedKey, fromController.ExtractMvcKey(actionMethod));
        }

        #endregion

    }
}
