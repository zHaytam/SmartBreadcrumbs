using Microsoft.AspNetCore.Mvc;
using RazorPagesAndMvc.Pages.CSharp;
using SmartBreadcrumbs.Attributes;

namespace RazorPagesAndMvc.Controllers.Other
{
    [Route("Other/[action]")]
    public class OtherController : Controller
    {

        [Breadcrumb("<script>alert('test')</script>", FromPage = typeof(IndexModel))]
        public IActionResult XssTest()
        {
            return View();
        }

    }
}