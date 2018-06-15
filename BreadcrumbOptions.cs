namespace SmartBreadcrumbs
{
    public class BreadcrumbOptions
    {

        #region Properties

        public string TagName { get; set; }

        public string[] OlClasses { get; set; }

        public string[] LiClasses { get; set; }

        #endregion

        public BreadcrumbOptions()
        {
            TagName = "nav";
            OlClasses = new[] { "breadcrumb" };
            LiClasses = new[] { "breadcrumb-item", "active" };
        }

        public BreadcrumbOptions(string tagName, string[] olClasses, string[] liClasses)
        {
            TagName = tagName;
            OlClasses = olClasses;
            LiClasses = liClasses;
        }

    }
}
