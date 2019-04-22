using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs.Attributes;

namespace RazorPagesAndMvc.Controllers.Java
{
    [Route("Java/Advanced/[action]")]
    public class AdvancedJavaController : Controller
    {
        [HttpGet]
        [Breadcrumb("Advanced", FromAction = "Index", FromController = typeof(JavaController))]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Breadcrumb("Advanced", FromAction = "Index", FromController = typeof(JavaController))]
        public IActionResult Index(int i)
        {
            return View();
        }

        [HttpGet]
        [Breadcrumb("Post 1", FromAction = "Index")]
        public IActionResult Post1()
        {
            return View();
        }

    }
}