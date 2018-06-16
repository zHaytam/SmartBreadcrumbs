using System;

namespace SmartBreadcrumbs
{
    [AttributeUsage(AttributeTargets.Method)]
    public class BreadcrumbAttribute : Attribute
    {

        #region Properties

        public string Title { get; set; }

        public string Action { get; internal set; }

        public string FromAction { get; set; }

        public bool Default { get; set; }

        #endregion

        public BreadcrumbAttribute(string title)
        {
            Title = title;
        }

    }
}
