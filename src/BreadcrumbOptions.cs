namespace SmartBreadcrumbs
{
    public class BreadcrumbOptions
    {
        #region Properties

        /// <summary>
        /// The parent element tag name.
        /// </summary>
        public string ParentTagName { get; set; }

        /// <summary>
        /// The parent element tag classes (seperated by a space).
        /// </summary>
        public string ParentTagClasses { get; set; }

        /// <summary>
        /// The list element tag name. Either 'ol' for an ordered list, or 'ul' for an unordered list.
        /// </summary>
        public string ListTagName { get; set; }

        /// <summary>
        /// The list element tag classes (separated by a space).
        /// </summary>
        public string ListTagClasses { get; set; }

        /// <summary>
        /// The classes of the LI element (separated by a space).
        /// </summary>
        public string ListItemClasses { get; set; }

        /// <summary>
        /// The classes of the active LI element (separated by a space).
        /// </summary>
        public string ActiveListItemClasses { get; set; }

        /// <summary>
        /// In case you want to insert a seperator element between items.\n
        /// <para>Example: &lt;li class="separator"&gt;/&lt;/li&gt;</para>
        /// </summary>
        public string SeparatorElement { get; set; }

        /// <summary>
        /// Example: &lt;li&gt;&lt;a href="{1}"&gt;{0}&lt;/a&gt;&lt;/li&gt;
        /// <para>PS: IconClasses will have an index of 2.</para>
        /// </summary>
        public string ListItemTemplate { get; set; }

        /// <summary>
        /// Example: &lt;li class="active"&gt;&lt;a href="{1}"&gt;{0}&lt;/a&gt;&lt;/li&gt;
        /// <para>PS: IconClasses will have an index of 2.</para>
        /// </summary>
        public string ActiveListItemTemplate { get; set; }

        /// <summary>
        /// Set to true if you don't want to have a default node in your project.
        /// This means that you will need to handle the first node that the user will see.
        /// <para>Use this with caution.</para>
        /// </summary>
        public bool DontLookForDefaultNode { get; set; }

        public bool HasSeparatorElement => !string.IsNullOrEmpty(SeparatorElement);

        /// <summary>
        /// The action to call when only a controller is known, e.g. nameof(HomeController.Index).
        /// </summary>
        public string DefaultAction { get; }

        #endregion

        public BreadcrumbOptions()
        {
            ParentTagName = "nav";
            ListTagClasses = "breadcrumb";
            ListItemClasses = "breadcrumb-item";
            ActiveListItemClasses = "breadcrumb-item active";
            DefaultAction = "Index";
        }

        public BreadcrumbOptions(string parentTagName, string listTagName, string listClasses, string listItemClasses, string activeListItemClasses, string parentTagClasses = null, string separatorElement = null, string defaultAction = "Index")
        {
            ParentTagName = parentTagName;
            ListTagName = listTagName;
            ListTagClasses = listClasses;
            ListItemClasses = listItemClasses;
            ActiveListItemClasses = activeListItemClasses;
            ParentTagClasses = parentTagClasses;
            SeparatorElement = separatorElement;
            DefaultAction = defaultAction;
        }
    }
}
