using System;

namespace SmartBreadcrumbs.Attributes
{
    public class DefaultBreadcrumbAttribute : BreadcrumbAttribute
    {

        #region Properties

        public override bool Default => true;

        #endregion

        public DefaultBreadcrumbAttribute() { }

        public DefaultBreadcrumbAttribute(string title)
        {
            Title = title;
        }

        #region Public Methods

        public override string ExtractFromKey(Type type) => null;

        #endregion

    }
}
