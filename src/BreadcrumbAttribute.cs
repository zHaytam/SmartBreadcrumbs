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
        /// If FromController is empty and FromAction is just an {action} it will automaticly
        /// add the current controller.
        /// You syntax {controller}.{action} or user FromController instead
        /// </summary>
        public virtual string FromAction { get; set; }

        /// <summary>
        /// Optional. You can use HomeController or just Home
        /// </summary>
        public virtual string FromController { get; set; }

        /// <summary>
        /// Wheither to cache ViewData titles (e.g. ViewData.Something)
        /// </summary>
        public virtual bool CacheTitle { get; set; }

        /// <summary>
        /// Wheither to replace the node's title with what was found in ViewData.
        /// </summary>
        public virtual bool OverwriteOnExactMatch { get; set; }

        /// <summary>
        /// Only usable when you're providing a LiTemplate and ActiveLiTemplate.
        /// <para>Example: &lt;li&gt;&lt;a href=\"{1}\"&gt;&lt;i class=\"{2}\">&lt;/i&gt;{0}&lt;/a&gt;&lt;/li&gt;</para>
        /// </summary>
        public virtual string IconClasses { get; set; }

        public virtual bool Default => false;

        #endregion

        public BreadcrumbAttribute(string title)
        {
            Title = title;
        }

    }

    public class DefaultBreadcrumbAttribute : BreadcrumbAttribute
    {

        public override bool Default => true;

        public DefaultBreadcrumbAttribute(string title) : base(title) { }

    }
}
