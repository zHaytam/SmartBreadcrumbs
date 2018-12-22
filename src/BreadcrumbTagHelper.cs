using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SmartBreadcrumbs
{
    [HtmlTargetElement("breadcrumb")]
    public class BreadcrumbTagHelper : TagHelper
    {
        #region Fields

        private readonly BreadcrumbsManager _breadcrumbsManager;
        private readonly UrlHelper _urlHelper;

        #endregion

        #region Properties

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        #endregion

        public BreadcrumbTagHelper(BreadcrumbsManager breadcrumbsManager, IActionContextAccessor actionContextAccessor)
        {
            _breadcrumbsManager = breadcrumbsManager;
            _urlHelper = new UrlHelper(actionContextAccessor.ActionContext);
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var child = await output.GetChildContentAsync();

            string action = ViewContext.ActionDescriptor.RouteValues["action"];
            string controller = ViewContext.ActionDescriptor.RouteValues["controller"];

            var nodeKey = $"{controller}.{action}";
            var node = ViewContext.ViewData["BreadcrumbNode"] as BreadcrumbNode ?? _breadcrumbsManager.GetNode(nodeKey);

            output.TagName = _breadcrumbsManager.Options.TagName;
            
            // Tag Classes
            if (!string.IsNullOrEmpty(_breadcrumbsManager.Options.TagClasses))
            {
                output.Attributes.Add("class", _breadcrumbsManager.Options.TagClasses);
            }

            output.Content.AppendHtml($"<ol class=\"{_breadcrumbsManager.Options.OlClasses}\">");

            var sb = new StringBuilder();

            if (node != null)
            {
                var fetchTitleFromViewData = node.CacheTitle && node.Title.StartsWith("ViewData.");
                
                if (fetchTitleFromViewData || node.OverwriteTitleOnExactMatch)
                    node.Title = ExtractTitle(node.GetOriginTitle());

                sb.Append($"<li{GetClass(_breadcrumbsManager.Options.ActiveLiClasses)}>{ExtractTitle(node.Title)}</li>");

                while (node.Parent != null)
                {
                    node = node.Parent;

                    // Separator
                    if (_breadcrumbsManager.Options.HasSeparatorElement)
                    {
                        sb.Insert(0, _breadcrumbsManager.Options.SeparatorElement);
                    }

                    sb.Insert(0, $"<li{GetClass(_breadcrumbsManager.Options.LiClasses)}><a href=\"{node.GetUrl(_urlHelper)}\">{node.Title}</a></li>");
                }
            }

            // If the node was custom and it had no defaultnode
            if (node != _breadcrumbsManager.DefaultNode)
            {
                // Separator
                if (_breadcrumbsManager.Options.HasSeparatorElement)
                {
                    sb.Insert(0, _breadcrumbsManager.Options.SeparatorElement);
                }

                sb.Insert(0, $"<li{GetClass(_breadcrumbsManager.Options.LiClasses)}><a href=\"{_breadcrumbsManager.DefaultNode.GetUrl(_urlHelper)}\">{_breadcrumbsManager.DefaultNode.Title}</a></li>");
            }

            output.Content.AppendHtml(sb.ToString());
            output.Content.AppendHtml(child);
            output.Content.AppendHtml("</ol>");

        }

        private string ExtractTitle(string title)
        {
            if (!title.StartsWith("ViewData."))
                return title;

            string key = title.Substring(9);
            return ViewContext.ViewData.ContainsKey(key) ? ViewContext.ViewData[key].ToString() : key;
        }

        private string GetClass(string classes)
        {
            if (string.IsNullOrEmpty(classes))
                return "";

            return $" class=\"{classes}\"";
        }

    }
}
