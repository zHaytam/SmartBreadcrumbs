using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs.Attributes;

namespace RazorPagesAndMvc.Controllers.Java
{
    public class JavaController : Controller
    {

        [Breadcrumb("Java")]
        public IActionResult Index()
        {
            return View();
        }

    }
}