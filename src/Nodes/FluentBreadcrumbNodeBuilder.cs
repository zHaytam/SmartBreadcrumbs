namespace SmartBreadcrumbs.Nodes
{
    public class FluentBreadcrumbNodeBuilder
    {
        private readonly FluentBreadcrumbNodeBuilder parent;
        private BreadcrumbNode node;
        private bool isFirstNodeInitializing;

        private BreadcrumbNode lastNode;

        public FluentBreadcrumbNodeBuilder()
        {
            isFirstNodeInitializing = true;
        }

        public FluentBreadcrumbNodeBuilder(BreadcrumbNode node, FluentBreadcrumbNodeBuilder parent = null)
        {
            this.node = node;
            this.parent = parent;
        }

        public FluentBreadcrumbNodeBuilder AddNode(BreadcrumbNode node)
        {
            if (isFirstNodeInitializing)
            {
                this.node = node;
                isFirstNodeInitializing = false;
                return this;
            }
            else
            {
                return new FluentBreadcrumbNodeBuilder(node, this);
            }
        }

        public FluentBreadcrumbNodeBuilder AddMvcNode(
            string action, string controller, string title, bool overwriteTitleOnExactMatch = false, string iconClasses = null, string areaName = null)
        {
            var node = new MvcBreadcrumbNode(action, controller, title, overwriteTitleOnExactMatch, iconClasses, areaName);

            if (isFirstNodeInitializing)
            {
                this.node = node;
                isFirstNodeInitializing = false;
                return this;
            }
            else
            {
                return new FluentBreadcrumbNodeBuilder(node, this);
            }
        }

        public FluentBreadcrumbNodeBuilder AddMvcControllerNode(
            string controller, string title, bool overwriteTitleOnExactMatch = false, string iconClasses = null, string areaName = null)
        {
            var node = new MvcControllerBreadcrumbNode(controller, title, overwriteTitleOnExactMatch, iconClasses, areaName);

            if (isFirstNodeInitializing)
            {
                this.node = node;
                isFirstNodeInitializing = false;
                return this;
            }
            else
            {
                return new FluentBreadcrumbNodeBuilder(node, this);
            }
        }

        public FluentBreadcrumbNodeBuilder AddRazorPageNode(
            string path, string title, bool overwriteTitleOnExactMatch = false, string iconClasses = null, string areaName = null)
        {
            var node = new RazorPageBreadcrumbNode(path, title, overwriteTitleOnExactMatch, iconClasses, areaName);

            if (isFirstNodeInitializing)
            {
                this.node = node;
                isFirstNodeInitializing = false;
                return this;
            }
            else
            {
                return new FluentBreadcrumbNodeBuilder(node, this);
            }
        }

        public FluentBreadcrumbNodeBuilder EndNode()
        {
            if (parent != null)
            {
                node.Parent = parent.node;

                if (lastNode == null) // Should only happen one time
                {
                    parent.lastNode = node;
                }
                else // Bubble the last node up the chain.. (to use inside Build())
                {
                    parent.lastNode = lastNode;
                }

                return parent;
            }

            return this;
        }

        public FluentBreadcrumbNodeBuilder SetIconClasses(string iconClasses)
        {
            node.IconClasses = iconClasses;
            return this;
        }

        public FluentBreadcrumbNodeBuilder SetOverwriteTitleOnExactMatch(bool overwriteTitleOnExactMatch)
        {
            node.OverwriteTitleOnExactMatch = overwriteTitleOnExactMatch; ;
            return this;
        }

        public FluentBreadcrumbNodeBuilder SetRouteValues(object routeValues)
        {
            node.RouteValues = routeValues;
            return this;
        }

        public FluentBreadcrumbNodeBuilder SetTitle(string title)
        {
            node.Title = title;
            return this;
        }

        public BreadcrumbNode Build()
        {
            return lastNode;
        }
    }
}