using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs.Attributes;

namespace RazorPagesAndMvc.Areas.JavaScript.Controllers
{
    [Area("JavaScript")]
    [Route("Java-Script/Adv")]
    public class AdvancedController : Controller
    {
        public AdvancedController()
        {

        }

        [HttpGet]
        [Route("")]
        [Breadcrumb("Advanced", AreaName = "JavaScript", FromAction = "Index", FromController = typeof(HomeController))]
        public IActionResult Index()
        {
            return View();
        }
    }
}
