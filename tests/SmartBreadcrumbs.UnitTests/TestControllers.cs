using Microsoft.AspNetCore.Mvc;

namespace SmartBreadcrumbs.UnitTests
{
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