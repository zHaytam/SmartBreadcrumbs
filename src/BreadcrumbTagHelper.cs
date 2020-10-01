using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlclient;
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
        private readonly HtmlEncoder _htmlEncoder;
        private readonly IUrlHelper _urlHelper;

        #endregion

        #region Properties

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        #endregion

        public BreadcrumbTagHelper(BreadcrumbManager breadcrumbManager, IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor, HtmlEncoder htmlEncoder)
        {
            _breadcrumbManager = breadcrumbManager;
            _htmlEncoder = htmlEncoder;
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }

        #region Public Methods

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var child = await output.GetChildContentAsync();

            var nodeKey = new NodeKey(ViewContext.ActionDescriptor.RouteValues, ViewContext.HttpContext.Request.Method);
            var node = ViewContext.ViewData["BreadcrumbNode"] as BreadcrumbNode ?? _breadcrumbManager.GetNode(nodeKey.Value);

            output.TagName = BreadcrumbManager.Options.TagName;
            if (!string.IsNullOrWhiteSpace(BreadcrumbManager.Options.AriaLabel))
            {
                output.Attributes.Add("aria-label", BreadcrumbManager.Options.AriaLabel);
            }

            // Tag Classes
            if (!string.IsNullOrEmpty(BreadcrumbManager.Options.TagClasses))
            {
                output.Attributes.Add("class", BreadcrumbManager.Options.TagClasses);
            }

            output.Content.AppendHtml($"<ol class=\"{BreadcrumbManager.Options.OlClasses}\">");

            var sb = new StringBuilder();

            // Go down the hierarchy
            if (node != null)
            {
                if (node.OverwriteTitleOnExactMatch && node.Title.StartsWith("ViewData."))
                    node.Title = ExtractTitle(node.OriginalTitle, false);

                sb.Insert(0, GetLi(node, node.GetUrl(_urlHelper), true));

                while (node.Parent != null)
                {
                    node = node.Parent;

                    // Separator
                    if (BreadcrumbManager.Options.HasSeparatorElement)
                    {
                        sb.Insert(0, BreadcrumbManager.Options.SeparatorElement);
                    }

                    sb.Insert(0, GetLi(node, node.GetUrl(_urlHelper), false));
                }
            }

            // If the node was custom and it had no defaultnode
            if (!BreadcrumbManager.Options.DontLookForDefaultNode && node != _breadcrumbManager.DefaultNode)
            {
                // Separator
                if (BreadcrumbManager.Options.HasSeparatorElement)
                {
                    sb.Insert(0, BreadcrumbManager.Options.SeparatorElement);
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

        private string ExtractTitle(string title, bool encode = true)
        {
            if (!title.StartsWith("ViewData."))
                return encode ? _htmlEncoder.Encode(title) : title;

            string key = title.Substring(9);
            title = ViewContext.ViewData.ContainsKey(key) ? ViewContext.ViewData[key].ToString() : $"{key} Not Found";
            return encode ? _htmlEncoder.Encode(title) : title;
        }

        private static string GetClass(string classes)
        {
            return string.IsNullOrEmpty(classes) ? "" : $" class=\"{classes}\"";
        }

        private string GetLi(BreadcrumbNode node, string link, bool isActive)
        {
            // In case the node's title is still ViewData.Something
            string nodeTitle = ExtractTitle(node.Title);

            var normalTemplate = BreadcrumbManager.Options.LiTemplate;
            var activeTemplate = BreadcrumbManager.Options.ActiveLiTemplate;

            if (!isActive && string.IsNullOrEmpty(normalTemplate))
                return $"<li{GetClass(BreadcrumbManager.Options.LiClasses)}><a href=\"{link}\">{nodeTitle}</a></li>";

            if (isActive && string.IsNullOrEmpty(activeTemplate))
                return $"<li{GetClass(BreadcrumbManager.Options.ActiveLiClasses)}>{nodeTitle}</li>";

            // Templates
            string templateToUse = isActive ? activeTemplate : normalTemplate;

            // The IconClasses will get ignored if the template doesn't have their index.
            return string.Format(templateToUse, nodeTitle, link, node.IconClasses);
        }

        #endregion

    }
}
