using System;

namespace SmartBreadcrumbs
{
    [AttributeUsage(AttributeTargets.Method)]
    public class BreadcrumbAttribute : Attribute
    {

        #region Properties

        public virtual string Title { get; set; }

        public virtual string Action { get; internal set; }

        /// <summary>
        /// If FromController is empty and FromAction is just an {action} it will automaticly add the current controller
        /// You syntax {controller}.{action} or user FromController instead
        /// </summary>
        public virtual string FromAction { get; set; }

        /// <summary>
        /// Optional. You can use HomeController or just Home
        /// </summary>
        public virtual string FromController { get; set; }

        public virtual bool CacheTitle { get; set; }

        public bool OverwriteOnExactMatch { get; set; }

        public virtual bool Default => false;

        #endregion

        public BreadcrumbAttribute(string title)
        {
            Title = title;
        }

    }

    public class DefaultBreadcrumbAttribute : BreadcrumbAttribute
    {
        public DefaultBreadcrumbAttribute(string title) : base(title) { }

        public override bool Default => true;
    }
}
