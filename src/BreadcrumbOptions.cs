namespace SmartBreadcrumbs
{
    public class BreadcrumbOptions
    {

        #region Properties

        public string TagName { get; set; }

        public string OlClasses { get; set; }

        public string LiClasses { get; set; }

        public string ActiveLiClasses { get; set; }

        #endregion

        public BreadcrumbOptions()
        {
            TagName = "nav";
            OlClasses = "breadcrumb";
            LiClasses = "breadcrumb-item";
            ActiveLiClasses = "breadcrumb-item active";
        }

        public BreadcrumbOptions(string tagName, string olClasses, string liClasses, string activeLiClasses)
        {
            TagName = tagName;
            OlClasses = olClasses;
            LiClasses = liClasses;
            ActiveLiClasses = activeLiClasses;
        }

    }
}
