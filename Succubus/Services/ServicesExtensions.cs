using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Succubus.Services
{
    public static class ServicesExtensions
    {
        public static IServiceCollection LoadSuccubusServices(this IServiceCollection collection, Assembly assembly)
        {
            var types = assembly.GetTypesWithInterface();

            foreach (var type in types)
                collection.AddSingleton(type, typeof(IService));

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

        private static IEnumerable<Type> GetTypesWithInterface(this Assembly assembly)
        {
            return assembly.GetLoadableTypes().Where(typeof(IService).IsAssignableFrom);
        }
    }
}