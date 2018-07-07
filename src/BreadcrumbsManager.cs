using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Threading.Tasks;

namespace SmartBreadcrumbs
{
    public class BreadcrumbsManager
    {

        #region Fields

        private readonly Dictionary<string, BreadcrumbNode> _nodes;

        #endregion

        #region Properties

        public BreadcrumbOptions Options { get; set; }

        public BreadcrumbNode DefaultNode { get; internal set; }

        #endregion

        public BreadcrumbsManager()
        {
            _nodes = new Dictionary<string, BreadcrumbNode>();
        }

        #region Public Methods

        public void Initialize(Assembly assembly, BreadcrumbOptions options)
        {
            Options = options;
            var controllerType = typeof(Controller);
            var actionResultType = typeof(IActionResult);
            var taResultType = typeof(Task<IActionResult>);

            // Extract all the Breadcrumb attributes
            var breadcrumbAttrs = new List<BreadcrumbAttribute>();

            foreach (var type in assembly.GetTypes())
            {
                if (!controllerType.IsAssignableFrom(type))
                    continue;

                string controller = type.Name.Replace("Controller", "");

                foreach (var method in type.GetMethods(BindingFlags.Instance | BindingFlags.Public))
                {
                    if (!actionResultType.IsAssignableFrom(method.ReturnType) && !taResultType.IsAssignableFrom(method.ReturnType))
                        continue;

                    var attr = method.GetCustomAttribute<BreadcrumbAttribute>();
                    if (attr == null)
                        continue;

                    if (attr.FromAction != null && !attr.FromAction.Contains("."))
                    {
                        if (!string.IsNullOrEmpty(attr.FromController))
                            attr.FromAction = $"{attr.FromController.Replace("Controller", "")}.{attr.FromAction}";
                        else
                            attr.FromAction = $"{controller}.{attr.FromAction}";
                    }

                    attr.Action = $"{controller}.{method.Name}";
                    breadcrumbAttrs.Add(attr);
                }
            }

            GenerateHierarchy(breadcrumbAttrs);
        }

        public BreadcrumbNode GetNode(string key) => _nodes.ContainsKey(key) ? _nodes[key] : null;

        #endregion

        #region Private Methods

        private void GenerateHierarchy(IReadOnlyCollection<BreadcrumbAttribute> breadcrumbAttrs)
        {
            // Default node
            var defaultBreadcrumbAttr = breadcrumbAttrs.FirstOrDefault(ba => ba.Default);
            if (defaultBreadcrumbAttr == null)
                throw new System.Exception("Default breadcrumb attribute not found.");

            DefaultNode = new BreadcrumbNode(defaultBreadcrumbAttr);
            if (!_nodes.ContainsKey(defaultBreadcrumbAttr.Action))
                _nodes.Add(defaultBreadcrumbAttr.Action, DefaultNode);

            foreach (var attr in breadcrumbAttrs)
            {
                if (attr.Default)
                    continue;

                // Create the node if needed
                if (!_nodes.ContainsKey(attr.Action))
                    _nodes.Add(attr.Action, new BreadcrumbNode(attr));

                // If this is a parentless node, set the parent to the default node
                if (string.IsNullOrEmpty(attr.FromAction))
                {
                    _nodes[attr.Action].Parent = DefaultNode;
                    continue;
                }

                // Create the parent node if needed
                if (!_nodes.ContainsKey(attr.FromAction))
                    _nodes.Add(attr.FromAction, new BreadcrumbNode(breadcrumbAttrs.First(ba => ba.Action == attr.FromAction)));

                // Set the node's parent
                _nodes[attr.Action].Parent = _nodes[attr.FromAction];
            }
        }

        #endregion

    }

}
