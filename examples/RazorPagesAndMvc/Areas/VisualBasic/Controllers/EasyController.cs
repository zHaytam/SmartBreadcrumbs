using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RazorPagesAndMvc.Areas.VisualBasic.Controllers
{
    [Area("VisualBasic")]
    public class EasyController : Controller
    {
        public EasyController()
        {

        }

        [HttpGet]
        [Breadcrumb("Easy", AreaName = "VisualBasic", FromAction = "Index", FromController = typeof(HomeController))]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Breadcrumb("More Easy", AreaName = "VisualBasic", FromAction = "Index", FromController = typeof(HomeController))]
        public IActionResult More()
        {
            return View();
        }

        [HttpGet]
        [Breadcrumb("Sub Easy", AreaName = "VisualBasic", FromAction = "Index")]
        public IActionResult Sub()
        {
            return View();
        }
    }
}
