using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IUrlHelper _urlHelper;

        #endregion

        #region Properties

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        #endregion

        public BreadcrumbTagHelper(BreadcrumbsManager breadcrumbsManager, IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor)
        {
            _breadcrumbsManager = breadcrumbsManager;
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
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

                sb.Insert(0, GetLi(node, node.GetUrl(_urlHelper), true));

                while (node.Parent != null)
                {
                    node = node.Parent;

                    // Separator
                    if (_breadcrumbsManager.Options.HasSeparatorElement)
                    {
                        sb.Insert(0, _breadcrumbsManager.Options.SeparatorElement);
                    }

                    sb.Insert(0, GetLi(node, node.GetUrl(_urlHelper), false));
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

                sb.Insert(0, GetLi(_breadcrumbsManager.DefaultNode,
                    _breadcrumbsManager.DefaultNode.GetUrl(_urlHelper),
                    false));
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
            return ViewContext.ViewData.ContainsKey(key) ? ViewContext.ViewData[key].ToString() : $"{key} Not Found";
        }

        private string GetClass(string classes)
        {
            return string.IsNullOrEmpty(classes) ? "" : $" class=\"{classes}\"";
        }

        private string GetLi(BreadcrumbNode node, string link, bool isActive)
        {
            var normalTemplate = _breadcrumbsManager.Options.LiTemplate;
            var activeTemplate = _breadcrumbsManager.Options.ActiveLiTemplate;

            if (!isActive && string.IsNullOrEmpty(normalTemplate))
                return $"<li{GetClass(_breadcrumbsManager.Options.LiClasses)}><a href=\"{link}\">{node.Title}</a></li>";

            if (isActive && string.IsNullOrEmpty(activeTemplate))
                return $"<li{GetClass(_breadcrumbsManager.Options.LiClasses)}>{node.Title}</li>";

            // Templates
            string templateToUse = isActive ? activeTemplate : normalTemplate;
            
            // The IconClasses will get ignored if the template doesn't have their index.
            return string.Format(templateToUse, node.Title, link, node.IconClasses);
        }

    }
}