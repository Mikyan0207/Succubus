﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using Succubus.Services.Interfaces;

namespace Succubus.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection LoadSuccubusServices(this IServiceCollection collection, Assembly assembly)
        {
            var sw = Stopwatch.StartNew();
            var logger = LogManager.GetCurrentClassLogger();
            var types = assembly.GetTypesWithInterface(typeof(IService));

            foreach (var type in types)
            {
                collection.AddTransient(type);
                logger.Info($"Loading {type.Name} from {type.Assembly.GetName().Name}");
            }

            sw.Stop();
            logger.Info($"Succubus Services loaded in {sw.Elapsed.TotalSeconds:F2}s");

            return collection;
        }

        private static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));

            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types;
            }
        }

        private static IEnumerable<Type> GetTypesWithInterface(this Assembly assembly, Type type)
        {
            return assembly.GetLoadableTypes().Where(x => x.GetInterfaces().Contains(type)).ToList();
        }
    }
}