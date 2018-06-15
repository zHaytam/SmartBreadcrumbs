using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace SmartBreadcrumbs
{
    public class BreadcrumbNode
    {

        #region Properties

        public string Title { get; set; }

        public string Action { get; set; }

        public string Controller { get; set; }

        public BreadcrumbNode Parent { get; set; }

        public object RouteValues { get; set; }

        #endregion

        public BreadcrumbNode(BreadcrumbAttribute attr)
        {
            Title = attr.Title;
            string[] tmp = attr.Action.Split('.');
            Controller = tmp[0];
            Action = tmp[1];
        }

        public BreadcrumbNode(string title, string action, string controller, BreadcrumbNode parent, object routeValues = null)
        {
            Title = title;
            Action = action;
            Controller = controller;
            Parent = parent;
            RouteValues = routeValues;
        }

        #region Public Methods

        public string GetUrl(UrlHelper urlHelper)
        {
            return urlHelper.Action(Action, Controller, RouteValues);
        }

        #endregion

    }
}
