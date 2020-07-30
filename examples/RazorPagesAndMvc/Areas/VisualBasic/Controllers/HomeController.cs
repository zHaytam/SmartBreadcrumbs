using Microsoft.AspNetCore.Mvc;
using RazorPagesAndMvc.Pages;
using SmartBreadcrumbs.Attributes;

namespace RazorPagesAndMvc.Areas.VisualBasic.Controllers
{
    [Area("VisualBasic")]
    public class HomeController : Controller
    {
        public HomeController()
        {

        }

        [HttpGet]
        [Breadcrumb("Visual Basic", AreaName = "VisualBasic", FromPage = typeof(IndexModel))]
        public IActionResult Index()
        {
            return View();
        }
    }
}
