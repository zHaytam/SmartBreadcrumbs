using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace SmartBreadcrumbs
{
    public static class BreadcrumbExtensions
    {

        public static void UseBreadcrumbs(this IServiceCollection services, Assembly assembly)
        {
            UseBreadcrumbs(services, assembly, new BreadcrumbOptions());
        }

        public static void UseBreadcrumbs(this IServiceCollection services, Assembly assembly, Action<BreadcrumbOptions> optionsSetter)
        {
            var options = new BreadcrumbOptions();
            optionsSetter.Invoke(options);
            UseBreadcrumbs(services, assembly, options);
        }

        private static void UseBreadcrumbs(IServiceCollection services, Assembly assembly, BreadcrumbOptions options)
        {
            var bm = new BreadcrumbsManager();
            bm.Initialize(assembly, options);
            services.AddSingleton(bm);
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
        }

    }
}
