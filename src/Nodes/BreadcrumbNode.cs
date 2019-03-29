using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs.Attributes;

namespace SmartBreadcrumbs.Nodes
{
    public abstract class BreadcrumbNode
    {

        #region Properties

        public string Title { get; set; }

        public string OriginalTitle { get; }

        public object RouteValues { get; set; }

        public bool OverwriteTitleOnExactMatch { get; set; }

        public string IconClasses { get; set; }

        public BreadcrumbNode Parent { get; set; }

        #endregion

        internal BreadcrumbNode(BreadcrumbAttribute attr) :
            this(attr.Title, attr.OverwriteTitleOnExactMatch, attr.IconClasses, attr.AreaName)
        {

        }

        protected BreadcrumbNode(string title, bool overwriteTitleOnExactMatch = false, string iconClasses = null, string areaName = null)
        {
            Title = title;
            OriginalTitle = Title;
            OverwriteTitleOnExactMatch = overwriteTitleOnExactMatch;
            IconClasses = iconClasses;

            if (!string.IsNullOrWhiteSpace(areaName))
            {
                RouteValues = new
                {
                    area = areaName
                };
            }
        }

        #region Public Methods

        public abstract string GetUrl(IUrlHelper urlHelper);

        #endregion

    }
}
