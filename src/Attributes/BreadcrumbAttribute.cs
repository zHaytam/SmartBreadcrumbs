using System;
using System.Reflection;
using SmartBreadcrumbs.Extensions;

namespace SmartBreadcrumbs.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class BreadcrumbAttribute : Attribute
    {

        #region Properties

        /// <summary>
        /// The title of this breadcrumb item.
        /// <para>Can be "ViewData.Something", where Something is the key.</para>
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The type of the page this breadcrumb item comes from.
        /// </summary>
        public Type FromPage { get; set; }

        /// <summary>
        /// The method name of the action this breadcrumb item comes from.
        /// </summary>
        public string FromAction { get; set; }

        /// <summary>
        /// The type of the controller this breadcrumb item comes from.
        /// <para>If this is null, the same controller as the method is used.</para>
        /// <para>PS: This can't be null when used on a Breadcrumb.</para>
        /// </summary>
        public Type FromController { get; set; }

        /// <summary>
        /// Wheither to replace the node's title with what was found in ViewData.
        /// <para>Only usable when the Title is "ViewData.Something".</para>
        /// </summary>
        public bool OverwriteTitleOnExactMatch { get; set; }

        /// <summary>
        /// Only usable when you're providing a LiTemplate and ActiveLiTemplate.
        /// <para>Example: &lt;li&gt;&lt;a href=\"{1}\"&gt;&lt;i class=\"{2}\">&lt;/i&gt;{0}&lt;/a&gt;&lt;/li&gt;</para>
        /// </summary>
        public string IconClasses { get; set; }

        /// <summary>
        /// The area's name of this breadcrumb item (if needed).
        /// </summary>
        public string AreaName { get; set; }

        /// <summary>
        /// Whether this is the default breadcrumb or not.
        /// </summary>
        public virtual bool Default => false;

        #endregion

        public BreadcrumbAttribute() { }

        public BreadcrumbAttribute(string title)
        {
            Title = title;
        }

        #region Public Methods

        public virtual string ExtractFromKey(Type type)
        {
            if (!string.IsNullOrWhiteSpace(FromAction))
            {
                var fromControllerType = FromController;

                if (fromControllerType == null)
                {
                    // Try to infer it from the type
                    if (!type.IsController())
                        throw new SmartBreadcrumbsException($"Can't infer FromController as '{type.Name}' is a Razor Page.");

                    fromControllerType = type;
                }

                if (!fromControllerType.IsController())
                    throw new SmartBreadcrumbsException($"'{fromControllerType.Name}' is used in FromController but isn't a Controller.");

                var actionMethod = fromControllerType.GetMethod(FromAction, BindingFlags.Instance | BindingFlags.Public);
                if (actionMethod == null || actionMethod.ReturnType.IsAction() == false)
                    throw new SmartBreadcrumbsException($"{fromControllerType.Name} doesn't contain a valid action named {FromAction}.");

                return fromControllerType.ExtractMvcKey(actionMethod);
            }

            if (FromPage == null)
                return null; // Will use default as parent

            if (!FromPage.IsRazorPage())
                throw new SmartBreadcrumbsException($"'{FromPage.Name}' is used in FromPage but isn't a Razor Page.");

            return FromPage.ExtractRazorPageKey();
        }

        #endregion

    }
}
