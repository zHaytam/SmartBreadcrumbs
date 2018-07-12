namespace SmartBreadcrumbs
{
    public class BreadcrumbOptions
    {

        #region Properties

        public string TagName { get; set; }

        public string TagClasses { get; set; }

        public string OlClasses { get; set; }

        public string LiClasses { get; set; }

        public string ActiveLiClasses { get; set; }

        public string SeparatorElement { get; set; }

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
