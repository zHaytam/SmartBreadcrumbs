using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace SmartBreadcrumbs.Nodes
{
    /// <summary>
    /// Class used to read routing values into a standard key for finding breadcrumb nodes.
    /// </summary>
    public sealed class NodeKey
    {
        private readonly string _page;
        private readonly string _area;
        private readonly string _action;
        private readonly string _controller;
        private readonly string _method;

        public NodeKey(IDictionary<string, string> routeValues, string method)
        {
            if (routeValues == null)
                throw new ArgumentNullException(nameof(routeValues));

            if (string.IsNullOrWhiteSpace(method))
                throw new ArgumentNullException(nameof(method));

            routeValues.TryGetValue("page", out _page);
            routeValues.TryGetValue("area", out _area);
            routeValues.TryGetValue("action", out _action);
            routeValues.TryGetValue("controller", out _controller);
            _method = method;
        }

        /// <summary>
        /// Unique key value for a given breadcrumb node. 
        /// Assesses page, area, controller, action, and method values to construct string key value that represents a node.
        /// </summary>
        public string Value
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_page))
                    return _page;

                if (string.IsNullOrWhiteSpace(_controller))
                    throw new SmartBreadcrumbsException("Route values do not contain 'page' or 'controller' when attempting to get node key.");

                string controller = !string.IsNullOrWhiteSpace(_area) ? $"{_area}.{_controller}" : _controller;

                if (string.IsNullOrWhiteSpace(_action))
                    return controller;

                if (!HttpMethods.IsGet(_method))
                    return $"{controller}.{_action}#{_method}";

                return $"{controller}.{_action}";
            }
        }
    }
}
