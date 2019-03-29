using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs.Attributes;

namespace RazorPagesAndMvc.Controllers.Java
{
    [Route("Java/Easy/[action]")]
    public class EasyJavaController : Controller
    {

        [Breadcrumb("Easy", FromAction = "Index", FromController = typeof(JavaController))]
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