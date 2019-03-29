using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartBreadcrumbs.Attributes;

namespace RazorPagesAndMvc.Pages
{
    [DefaultBreadcrumb("Index Page")]
    public class IndexModel : PageModel
    {

        public void OnGet()
        {

        }

    }
}
