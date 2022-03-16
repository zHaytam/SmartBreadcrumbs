using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs.Attributes;
using SmartBreadcrumbs.Extensions;
using SmartBreadcrumbs.Nodes;

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

        public IActionResult Post2()
        {
            var builder = new FluentBreadcrumbNodeBuilder()
                .AddMvcNode("Index", "Java", "Java")
                    .AddMvcNode(nameof(Index), "EasyJava", "Easy")
                        .AddMvcNode(nameof(Post2), "EasyJava", "Post 2")
                        .EndNode()
                    .EndNode()
                .EndNode();

            ViewData.SetBreadcrumbs(builder.Build());

            return View();
        }
    }
}