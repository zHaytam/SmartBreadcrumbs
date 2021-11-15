using System;

namespace SmartBreadcrumbs
{
    public class BreadcrumbOptions
    {
        #region Properties

        /// <summary>
        /// The parent element tag name.
        /// </summary>
        public string TagName { get; set; }

        /// <summary>
        /// The parent element tag classes (separated by a space).
        /// </summary>
        public string TagClasses { get; set; }

        /// <summary>
        /// The classes of OL elements.
        /// </summary>
        public string OlClasses { get; set; }

        /// <summary>
        /// The classes of LI elements.
        /// </summary>
        public string LiClasses { get; set; }

        /// <summary>
        /// The classes of the active LI element.
        /// </summary>
        public string ActiveLiClasses { get; set; }

        /// <summary>
        /// In case you want to insert a separator element between items.\n
        /// <para>Example: &lt;li class="separator"&gt;/&lt;/li&gt;</para>
        /// </summary>
        public string SeparatorElement { get; set; }

        /// <summary>
        /// Example: &lt;li&gt;&lt;a href="{1}"&gt;{0}&lt;/a&gt;&lt;/li&gt;
        /// <para>PS: IconClasses will have an index of 2.</para>
        /// </summary>
        public string LiTemplate { get; set; }

        /// <summary>
        /// Example: &lt;li class="active"&gt;&lt;a href="{1}"&gt;{0}&lt;/a&gt;&lt;/li&gt;
        /// <para>PS: IconClasses will have an index of 2.</para>
        /// </summary>
        public string ActiveLiTemplate { get; set; }

        /// <summary>
        /// Set to true if you don't want to have a default node in your project.
        /// This means that you will need to handle the first node that the user will see.
        /// <para>Use this with caution.</para>
        /// </summary>
        public bool DontLookForDefaultNode { get; set; }

        public bool HasSeparatorElement => !string.IsNullOrEmpty(SeparatorElement);

        /// <summary>
        /// Whether to use DefaultAction when a method doesn't specify FromAction, FromController and FromPage.
        /// </summary>
        public bool InferFromAction { get; set; }

        /// <summary>
        /// The action to call when only a controller is known, e.g. nameof(HomeController.Index).
        /// </summary>
        public string DefaultAction { get; set; }

        /// <summary>
        /// Whether to use the method's name when no Title was provided.
        /// </summary>
        public bool FallbackTitleToMethodName { get; set; }

        /// <summary>
        /// Application path used as the root of discovery for Razor Page files.
        /// Defaults to the "Pages" directory under application root.
        /// </summary>
        public string RazorPagesRootDirectory { get; set; }

        /// <summary>
        /// A description that describes the navigation to screen reader users.
        /// </summary>
        public string AriaLabel { get; set; }

        /// <summary>
        /// The type of resource that will be used for the translation of the BreadcrumbNode title
        /// </summary>
        public Type ResourceType { get; set; }

        #endregion Properties

        public BreadcrumbOptions()
        {
            TagName = "nav";
            OlClasses = "breadcrumb";
            LiClasses = "breadcrumb-item";
            ActiveLiClasses = "breadcrumb-item active";
            InferFromAction = true;
            DefaultAction = "Index";
            FallbackTitleToMethodName = true;
            RazorPagesRootDirectory = "Pages";
            AriaLabel = "breadcrumbs";
            ResourceType = null;
        }

        public BreadcrumbOptions(string tagName, string olClasses, string liClasses,
            string activeLiClasses, string tagClasses = null, string separatorElement = null,
            bool inferFromAction = true, string defaultAction = "Index", bool fallbackTitleToMethodName = true,
            string razorPagesRootDirectory = "Pages", string ariaLabel = "breadcrumbs", Type resourceType = null)
        {
            TagName = tagName;
            OlClasses = olClasses;
            LiClasses = liClasses;
            ActiveLiClasses = activeLiClasses;
            TagClasses = tagClasses;
            SeparatorElement = separatorElement;
            InferFromAction = inferFromAction;
            DefaultAction = defaultAction;
            FallbackTitleToMethodName = fallbackTitleToMethodName;
            RazorPagesRootDirectory = razorPagesRootDirectory;
            AriaLabel = ariaLabel;
            ResourceType = resourceType;
        }
    }
}