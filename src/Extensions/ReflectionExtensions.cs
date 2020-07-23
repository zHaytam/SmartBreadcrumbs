using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;

namespace SmartBreadcrumbs.Extensions
{
    public static class ReflectionExtensions
    {
        #region Fields

        private static readonly Type PageModelType = typeof(PageModel);
        private static readonly Type ControllerType = typeof(Controller);
        private static readonly Type ActionResultType = typeof(IActionResult);
        private static readonly Type GenericTaskType = typeof(Task<>);

        #endregion Fields

        public static bool IsController(this Type type)
            => type != null && ControllerType.IsAssignableFrom(type);

        public static bool IsRazorPage(this Type type)
            => type != null && PageModelType.IsAssignableFrom(type);

        public static bool IsAction(this Type type)
            => type != null && (ActionResultType.IsAssignableFrom(type) || IsActionTypeGenericTaskOfIActionResult(type));

        public static IEnumerable<string> ExtractHttpMethodAttributes(this MethodInfo actionMethod)
            => actionMethod.GetCustomAttributes<HttpMethodAttribute>(true)
                .SelectMany(m => m.HttpMethods)
                .Distinct();

        public static string ExtractRazorPageKey(this Type pageType)
        {
            if (pageType == null)
                throw new ArgumentNullException(nameof(pageType));

            string razorPagesRootDirectory = BreadcrumbManager.Options.RazorPagesRootDirectory;

            string fullName = pageType.FullName;
            int pagesIndex = fullName.IndexOf($".{razorPagesRootDirectory}.");
            if (pagesIndex == -1)
                throw new SmartBreadcrumbsException($"The full name {fullName} doesn't contain '{razorPagesRootDirectory}'.");

            int startIndex = pagesIndex + razorPagesRootDirectory.Length + 1;
            int endIndex = fullName.EndsWith("Model") ? fullName.Length - 5 : fullName.Length;
            return fullName.Substring(startIndex, endIndex - startIndex).Replace('.', '/');
        }

        public static string ExtractMvcKey(this Type controllerType, MethodInfo actionMethod)
        {
            if (controllerType == null)
                throw new ArgumentNullException(nameof(controllerType));

            if (actionMethod == null)
                throw new ArgumentNullException(nameof(actionMethod));

            return $"{controllerType.Name.Replace("Controller", "")}.{actionMethod.Name}";
        }

        public static string ExtractMvcControllerKey(this Type controllerType)
        {
            if (controllerType == null)
                throw new ArgumentNullException(nameof(controllerType));

            return $"{controllerType.Name.Replace("Controller", "")}.{BreadcrumbManager.Options.DefaultAction}";
        }

        private static bool IsActionTypeGenericTaskOfIActionResult(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == GenericTaskType)
            {
                Type genericType = type.GetGenericArguments()[0];
                return typeof(IActionResult).IsAssignableFrom(genericType);
            }

            return false;
        }
    }
}