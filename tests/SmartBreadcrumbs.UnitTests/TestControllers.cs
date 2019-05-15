using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs.Attributes;

namespace SmartBreadcrumbs.UnitTests
{
    [DefaultBreadcrumb]
    public class MainController : Controller
    {
        public IActionResult Index()
        {
            return Ok();
        }

        [Breadcrumb]
        public IActionResult Privacy()
        {
            return Ok();
        }
    }

    [Breadcrumb]
    public class SecondController : Controller
    {
        public IActionResult Index()
        {
            return Ok();
        }

        [Breadcrumb(FromAction = nameof(MainController.Privacy), FromController = typeof(MainController))]
        public IActionResult Privacy()
        {
            return Ok();
        }

        [Breadcrumb]
        public IActionResult About()
        {
            return Ok();
        }
    }

    public class TestController : Controller
    {

        public IActionResult MyAction()
        {
            return Ok();
        }

    }
}

namespace SmartBreadcrumbs.Controllers.SubFolder
{

    public class TestTwoController : Controller
    {

        public IActionResult MyActionTwo()
        {
            return Ok();
        }

    }

}