using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SmartBreadcrumbs
{
    public class BreadcrumbTagHelper : TagHelper
    {

        #region Fields

        private readonly BreadcrumbsManager _breadcrumbsManager;
        private readonly UrlHelper _urlHelper;

        #endregion

        #region Properties

        [ViewContext]
        public ViewContext ViewContext { get; set; }

        #endregion

        public BreadcrumbTagHelper(BreadcrumbsManager breadcrumbsManager, IActionContextAccessor actionContextAccessor)
        {
            _breadcrumbsManager = breadcrumbsManager;
            _urlHelper = new UrlHelper(actionContextAccessor.ActionContext);
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            string action = ViewContext.ActionDescriptor.RouteValues["action"];
            string controller = ViewContext.ActionDescriptor.RouteValues["controller"];

            var nodeKey = $"{controller}.{action}";
            var node = ViewContext.ViewData["BreadcrumbNode"] as BreadcrumbNode ?? _breadcrumbsManager.GetNode(nodeKey);

            if (node == null)
                return;

            output.TagName = _breadcrumbsManager.Options.TagName;
            output.Content.AppendHtml($"<ol class=\"{string.Join(' ', _breadcrumbsManager.Options.OlClasses)}\">");

            var sb = new StringBuilder();
            sb.Append($"<li class=\"{string.Join(' ', _breadcrumbsManager.Options.LiClasses)}\">{ExtractTitle(node.Title)}</li>");

            while (node.Parent != null)
            {
                node = node.Parent;
                sb.Insert(0, $"<li class=\"breadcrumb-item\"><a href=\"{node.GetUrl(_urlHelper)}\">{node.Title}</a></li>");
            }

            // If the node was custom and it had no defaultnode
            if (node != _breadcrumbsManager.DefaultNode)
            {
                sb.Insert(0, $"<li class=\"breadcrumb-item\"><a href=\"{_breadcrumbsManager.DefaultNode.GetUrl(_urlHelper)}\">{_breadcrumbsManager.DefaultNode.Title}</a></li>");
            }

            output.Content.AppendHtml(sb.ToString());
        }

        private string ExtractTitle(string title)
        {
            if (!title.StartsWith("ViewData."))
                return title;

            string key = title.Substring(9);
            return ViewContext.ViewData.ContainsKey(key) ? ViewContext.ViewData[key].ToString() : key;
        }

    }
}
