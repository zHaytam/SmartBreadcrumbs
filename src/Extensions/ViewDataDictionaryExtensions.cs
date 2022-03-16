using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SmartBreadcrumbs.Nodes;

namespace SmartBreadcrumbs.Extensions
{
    public static class ViewDataDictionaryExtensions
    {
        public static void SetBreadcrumbs(this ViewDataDictionary viewData, BreadcrumbNode breadcrumb)
        {
            viewData["BreadcrumbNode"] = breadcrumb;
        }
    }
}