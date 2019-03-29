using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs.Attributes;

namespace RazorPagesAndMvc.Controllers.Java
{
    public class AdvancedJavaController : Controller
    {

        [Breadcrumb("Advanced", FromAction = "Index", FromController = typeof(JavaController))]
        public IActionResult Index()
        {
            return View();
        }

        [Breadcrumb("Post 1", FromAction = "Index")]
        public IActionResult Post1()
        {
            return View();
        }

    }
}