using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs.Attributes;

namespace SmartBreadcrumbs.Nodes
{
    public class MvcBreadcrumbNode : BreadcrumbNode
    {

        #region Properties

        public string Action { get; set; }

        public string Controller { get; set; }

        #endregion

        internal MvcBreadcrumbNode(string action, string controller, BreadcrumbAttribute attr) : base(attr)
        {
            Action = action;
            Controller = controller;
        }

        public MvcBreadcrumbNode(string action, string controller, string title, bool overwriteTitleOnExactMatch = false, string iconClasses = null, string areaName = null)
            : base(title, overwriteTitleOnExactMatch, iconClasses, areaName)
        {
            Action = action;
            Controller = controller;
        }

        #region Public Methods

        public override string GetUrl(IUrlHelper urlHelper) => urlHelper.Action(Action, Controller, RouteValues);

        #endregion

    }
}
