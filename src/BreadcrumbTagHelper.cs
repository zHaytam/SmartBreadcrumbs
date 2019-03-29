using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SmartBreadcrumbs.Nodes;

namespace SmartBreadcrumbs
{
    [HtmlTargetElement("breadcrumb")]
    public class BreadcrumbTagHelper : TagHelper
    {

        #region Fields

        private readonly BreadcrumbManager _breadcrumbManager;
        private readonly IUrlHelper _urlHelper;

        #endregion

        #region Properties

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        #endregion

        public BreadcrumbTagHelper(BreadcrumbManager breadcrumbManager, IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor)
        {
            _breadcrumbManager = breadcrumbManager;
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }

        #region Public Methods

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var child = await output.GetChildContentAsync();

            string nodeKey = GetNodeKey(ViewContext.ActionDescriptor.RouteValues);
            var node = ViewContext.ViewData["BreadcrumbNode"] as BreadcrumbNode ?? _breadcrumbManager.GetNode(nodeKey);

            output.TagName = _breadcrumbManager.Options.TagName;

            // Tag Classes
            if (!string.IsNullOrEmpty(_breadcrumbManager.Options.TagClasses))
            {
                output.Attributes.Add("class", _breadcrumbManager.Options.TagClasses);
            }

            output.Content.AppendHtml($"<ol class=\"{_breadcrumbManager.Options.OlClasses}\">");

            var sb = new StringBuilder();

            // Go down the hierarchy
            if (node != null)
            {
                if (node.OverwriteTitleOnExactMatch && node.Title.StartsWith("ViewData."))
                    node.Title = ExtractTitle(node.OriginalTitle);

                sb.Insert(0, GetLi(node, node.GetUrl(_urlHelper), true));

                while (node.Parent != null)
                {
                    node = node.Parent;

                    // Separator
                    if (_breadcrumbManager.Options.HasSeparatorElement)
                    {
                        sb.Insert(0, _breadcrumbManager.Options.SeparatorElement);
                    }

                    sb.Insert(0, GetLi(node, node.GetUrl(_urlHelper), false));
                }
            }

            // If the node was custom and it had no defaultnode
            if (node != _breadcrumbManager.DefaultNode)
            {
                // Separator
                if (_breadcrumbManager.Options.HasSeparatorElement)
                {
                    sb.Insert(0, _breadcrumbManager.Options.SeparatorElement);
                }

                sb.Insert(0, GetLi(_breadcrumbManager.DefaultNode,
                    _breadcrumbManager.DefaultNode.GetUrl(_urlHelper),
                    false));
            }

            output.Content.AppendHtml(sb.ToString());
            output.Content.AppendHtml(child);
            output.Content.AppendHtml("</ol>");
        }

        #endregion

        #region Private Methods

        private string GetNodeKey(IDictionary<string, string> routeValues)
        {
            return routeValues.ContainsKey("page") ? 
                routeValues["page"] : 
                $"{routeValues["controller"]}.{routeValues["action"]}";
        }

        private string ExtractTitle(string title)
        {
            if (!title.StartsWith("ViewData."))
                return title;

            string key = title.Substring(9);
            return ViewContext.ViewData.ContainsKey(key) ? ViewContext.ViewData[key].ToString() : $"{key} Not Found";
        }

        private static string GetClass(string classes)
        {
            return string.IsNullOrEmpty(classes) ? "" : $" class=\"{classes}\"";
        }

        private string GetLi(BreadcrumbNode node, string link, bool isActive)
        {
            // In case the node's title is still ViewData.Something
            string nodeTitle = ExtractTitle(node.Title);

            var normalTemplate = _breadcrumbManager.Options.LiTemplate;
            var activeTemplate = _breadcrumbManager.Options.ActiveLiTemplate;

            if (!isActive && string.IsNullOrEmpty(normalTemplate))
                return $"<li{GetClass(_breadcrumbManager.Options.LiClasses)}><a href=\"{link}\">{nodeTitle}</a></li>";

            if (isActive && string.IsNullOrEmpty(activeTemplate))
                return $"<li{GetClass(_breadcrumbManager.Options.LiClasses)}>{nodeTitle}</li>";

            // Templates
            string templateToUse = isActive ? activeTemplate : normalTemplate;

            // The IconClasses will get ignored if the template doesn't have their index.
            return string.Format(templateToUse, nodeTitle, link, node.IconClasses);
        }

        #endregion

    }
}
