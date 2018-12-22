﻿using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace SmartBreadcrumbs
{
    public static class BreadcrumbExtensions
    {

        public static void AddBreadcrumbs(this IServiceCollection services, Assembly assembly)
        {
            AddBreadcrumbs(services, assembly, new BreadcrumbOptions());
        }

        public static void AddBreadcrumbs(this IServiceCollection services, Assembly assembly, Action<BreadcrumbOptions> optionsSetter)
        {
            var options = new BreadcrumbOptions();
            optionsSetter.Invoke(options);
            AddBreadcrumbs(services, assembly, options);
        }

        private static void AddBreadcrumbs(IServiceCollection services, Assembly assembly, BreadcrumbOptions options)
        {
            var bm = new BreadcrumbsManager();
            bm.Initialize(assembly, options);
            services.AddSingleton(bm);
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
        }

    }
}
