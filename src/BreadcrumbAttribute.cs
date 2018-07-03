using System;

namespace SmartBreadcrumbs
{
    [AttributeUsage(AttributeTargets.Method)]
    public class BreadcrumbAttribute : Attribute
    {
        public BreadcrumbAttribute(string title)
        {
            Title = title;
        }

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

        public virtual bool Default { get { return false; } }

        #endregion

    }

    public class DefaultBreadcrumbAttribute : BreadcrumbAttribute
    {
        public DefaultBreadcrumbAttribute(string title) : base(title) { }

        public override bool Default { get { return true; } }
    }
}
