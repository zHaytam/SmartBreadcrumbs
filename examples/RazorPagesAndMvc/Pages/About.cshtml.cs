using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartBreadcrumbs.Attributes;

namespace RazorPagesAndMvc.Pages
{
    [Breadcrumb("About")]
    public class AboutModel : PageModel
    {

        public void OnGet()
        {
        }

    }
}