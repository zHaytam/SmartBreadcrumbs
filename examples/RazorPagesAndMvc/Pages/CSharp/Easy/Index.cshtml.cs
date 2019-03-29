using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartBreadcrumbs.Attributes;

namespace RazorPagesAndMvc.Pages.CSharp.Easy
{
    [Breadcrumb("Easy", FromPage = typeof(CSharp.IndexModel))]
    public class IndexModel : PageModel
    {

        public void OnGet()
        {

        }

    }
}