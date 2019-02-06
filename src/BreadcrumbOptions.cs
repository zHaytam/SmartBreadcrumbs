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
        /// The parent element tag classes (seperated by a space).
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
        /// In case you want to insert a seperator element between items.\n
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

        public bool HasSeparatorElement => !string.IsNullOrEmpty(SeparatorElement);

        #endregion

        public BreadcrumbOptions()
        {
            TagName = "nav";
            OlClasses = "breadcrumb";
            LiClasses = "breadcrumb-item";
            ActiveLiClasses = "breadcrumb-item active";
        }

        public BreadcrumbOptions(string tagName, string olClasses, string liClasses, string activeLiClasses, string tagClasses = null, string separatorElement = null)
        {
            TagName = tagName;
            OlClasses = olClasses;
            LiClasses = liClasses;
            ActiveLiClasses = activeLiClasses;
            TagClasses = tagClasses;
            SeparatorElement = separatorElement;
        }

    }
}
