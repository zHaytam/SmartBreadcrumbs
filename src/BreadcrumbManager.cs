using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SmartBreadcrumbs.Attributes;
using SmartBreadcrumbs.Extensions;
using SmartBreadcrumbs.Nodes;

namespace SmartBreadcrumbs
{
    public class BreadcrumbManager
    {

        #region Fields

        private readonly Dictionary<string, BreadcrumbNode> _nodes;

        #endregion

        #region Properties

        public BreadcrumbOptions Options { get; }

        public BreadcrumbNode DefaultNode { get; internal set; }

        #endregion

        public BreadcrumbManager(BreadcrumbOptions options)
        {
            _nodes = new Dictionary<string, BreadcrumbNode>();
            Options = options;
        }

        #region Public Methods

        public void Initialize(Assembly assembly)
        {
            var entries = new Dictionary<string, BreadcrumbNodeEntry>();

            foreach (var type in assembly.GetTypes())
            {
                // Razor pages
                if (HasBreadcrumb(type, out BreadcrumbNodeEntry extractedEntry))
                {
                    entries.Add(extractedEntry.Key, extractedEntry);
                }

                // Controller actions
                foreach (var entry in TryExtractingEntries(type))
                {
                    entries.Add(entry.Key, entry);
                }
            }

            GenerateHierarchy(entries);
        }

        public BreadcrumbNode GetNode(string key) => _nodes.ContainsKey(key) ? _nodes[key] : null;

        #endregion

        #region Private Methods

        private void GenerateHierarchy(Dictionary<string, BreadcrumbNodeEntry> entries)
        {
            // Mandatory single default entry
            BreadcrumbNodeEntry defaultEntry = entries.Values.Single(e => e.Default);
            DefaultNode = defaultEntry.Node;
            _nodes.Add(defaultEntry.Key, DefaultNode);

            foreach (var entry in entries.Values)
            {
                if (entry.Default)
                    continue;

                var fromKey = entry.FromKey ?? defaultEntry.Key;
                if (!entries.ContainsKey(fromKey))
                    throw new SmartBreadcrumbsException($"No node exists that has a '{fromKey}' as a key.\n" +
                                                        $"Make sure that razor page or controller action has a BreadcrumbAttribute.");

                var fromEntry = entries[fromKey];
                entry.Node.Parent = fromEntry.Node;
                _nodes.Add(entry.Key, entry.Node);
            }
        }

        private static bool HasBreadcrumb(Type type, out BreadcrumbNodeEntry entry)
        {
            entry = null;

            if (!type.IsRazorPage())
                return false;

            var attr = type.GetCustomAttribute<BreadcrumbAttribute>();
            if (attr == null)
                return false;

            string path = type.ExtractRazorPageKey();
            entry = new BreadcrumbNodeEntry
            {
                Key = path,
                Node = new RazorPageBreadcrumbNode(path, attr),
                FromKey = attr.ExtractFromKey(type),
                Default = attr.Default
            };

            return true;
        }

        private static IEnumerable<BreadcrumbNodeEntry> TryExtractingEntries(Type type)
        {
            if (!type.IsController())
                yield break;

            foreach (var method in type.GetMethods(BindingFlags.Instance | BindingFlags.Public))
            {
                if (!method.ReturnType.IsAction())
                    continue;

                var attr = method.GetCustomAttribute<BreadcrumbAttribute>();
                if (attr == null)
                    continue;

                string key = type.ExtractMvcKey(method);
                yield return new BreadcrumbNodeEntry
                {
                    Key = key,
                    Node = new MvcBreadcrumbNode(method.Name, type.Name.Replace("Controller", ""), attr),
                    FromKey = attr.ExtractFromKey(type),
                    Default = attr.Default
                };
            }
        }
        
        #endregion

    }

    internal class BreadcrumbNodeEntry
    {

        public string Key { get; set; }

        public BreadcrumbNode Node { get; set; }

        public string FromKey { get; set; }

        public bool Default { get; set; }

    }
}
