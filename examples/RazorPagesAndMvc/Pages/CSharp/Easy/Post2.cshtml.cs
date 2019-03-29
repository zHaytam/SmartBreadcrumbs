using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartBreadcrumbs.Attributes;

namespace RazorPagesAndMvc.Pages.CSharp.Easy
{
    [Breadcrumb("Post 2", FromPage = typeof(IndexModel))]
    public class Post2Model : PageModel
    {

        public void OnGet()
        {

        }

    }
}