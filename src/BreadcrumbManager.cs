using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Http;
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

        public static BreadcrumbOptions Options { get; private set; } = new BreadcrumbOptions();

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
                    //No need to check the other IF and FOREACH when HasBreadcrumb returns true.
                    continue;
                }

                // Controllers
                if (TryGetBreadcrumbNodeEntry(type, out BreadcrumbNodeEntry controllerEntry))
                {
                    entries.Add(controllerEntry.Key, controllerEntry);
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
            string defaultNodeKey = null;
            if (!Options.DontLookForDefaultNode)
            {
                var defaultEntry = entries.Values.Single(e => e.Default);
                DefaultNode = defaultEntry.Node;
                defaultNodeKey = defaultEntry.Key;
                _nodes.Add(defaultNodeKey, DefaultNode);
            }

            foreach (var entry in entries.Values)
            {
                if (entry.Default)
                    continue;

                var fromKey = entry.FromKey ?? defaultNodeKey;
                if (fromKey != null && !entries.ContainsKey(fromKey))
                    throw new SmartBreadcrumbsException($"No node exists that has a '{fromKey}' as a key.\n" +
                                                        $"Make sure that razor page or controller action has a BreadcrumbAttribute.");

                entry.Node.Parent = fromKey != null ? entries[fromKey].Node : null;
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

            //if no title is given, then fallback to type name (razor page name).
            if (string.IsNullOrWhiteSpace(attr.Title))
                attr.Title = type.Name.Replace("Page", string.Empty);

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
        private bool TryGetBreadcrumbNodeEntry(Type type, out BreadcrumbNodeEntry entry)
        {
            entry = null;
            if (!type.IsController()) return false;

            var attr = type.GetCustomAttribute<BreadcrumbAttribute>();
            if (attr == null)
                return false;

            //if no title is given, then fallback to controller name.
            if (string.IsNullOrWhiteSpace(attr.Title))
                attr.Title = type.Name.Replace("Controller", string.Empty);

            string key = type.ExtractMvcControllerKey();
            entry = new BreadcrumbNodeEntry
            {
                Key = key,
                Node = new MvcControllerBreadcrumbNode(type.Name.Replace("Controller", string.Empty), attr),
                FromKey = attr.ExtractFromKey(type),
                Default = attr.Default
            };

            return true;
        }
        private IEnumerable<BreadcrumbNodeEntry> TryExtractingEntries(Type type)
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

                //if no fromController/page and no fromAction is given and we are not handling the defaultAction, infer fromController/Action from type.
                if (attr.FromController == null && attr.FromPage == null &&
                    string.IsNullOrWhiteSpace(attr.FromAction) && Options.DefaultAction != method.Name)
                {
                    attr.FromAction = Options.DefaultAction;
                }

                //if no title is given, then fallback to method name.
                if (string.IsNullOrWhiteSpace(attr.Title))
                    attr.Title = method.Name;

                string key = type.ExtractMvcKey(method);
                //Get all HttpXXX attributes as strings
                IEnumerable<string> httpMethods = method.ExtractHttpMethodAttributes();
                
                //this prevents duplication if a identically named action exists which only differs in httpmethod
                if (httpMethods.Where(m => m != HttpMethods.Get).Count() == 0)
                {
                    yield return new BreadcrumbNodeEntry
                    {
                        Key = $"{key}",
                        Node = new MvcBreadcrumbNode(method.Name, type.Name.Replace("Controller", ""), attr),
                        FromKey = attr.ExtractFromKey(type),
                        Default = attr.Default
                    };
                }
                else
                {
                    //skip the GET as this is considered default
                    foreach (var httpMethod in httpMethods.Where(m => m != HttpMethods.Get))
                    {
                        yield return new BreadcrumbNodeEntry
                        {
                            //foreach httpmethod besides GET append httpmethod as #method. e.g. Index#POST
                            Key = $"{key}#{httpMethod}",
                            Node = new MvcBreadcrumbNode(method.Name, type.Name.Replace("Controller", ""), attr),
                            FromKey = attr.ExtractFromKey(type),
                            Default = attr.Default
                        };
                    }
                }                
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
