using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RazorPagesAndMvc.Areas.JavaScript.Controllers
{
    [Area("JavaScript")]
    [Route("Java-Script/Easy")]
    public class EasyController : Controller
    {
        public EasyController()
        {

        }

        [HttpGet]
        [Route("")]
        [Breadcrumb("Easy", AreaName = "JavaScript", FromAction = "Index", FromController = typeof(HomeController))]
        public IActionResult Index()
        {
            return View();
        }
    }
}
