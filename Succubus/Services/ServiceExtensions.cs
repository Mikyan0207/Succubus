using Microsoft.Extensions.DependencyInjection;
using Succubus.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Succubus.Services
{
    public static class ServiceExtensions
    {
        public static IEnumerable<Type> LoadFrom(this IServiceCollection collection, Assembly assembly)
        {
            List<Type> added = new List<Type>();
            Type[] allTypes;

            try
            {
                allTypes = assembly.GetTypes();
            }
            catch
            {
                return Enumerable.Empty<Type>();
            }

            var services = new Queue<Type>(allTypes
                .Where(x => x.GetInterfaces().Contains(typeof(IService)) && !x.GetTypeInfo().IsInterface && !x.GetTypeInfo().IsAbstract)
                .ToArray());

            added.AddRange(services);

            var interfaces = new HashSet<Type>(allTypes
                .Where(x => x.GetInterfaces().Contains(typeof(IService)) && x.GetTypeInfo().IsInterface));

            while (services.Count > 0)
            {
                var serviceType = services.Dequeue();

                if (collection.FirstOrDefault(x => x.ServiceType == serviceType) != null)
                    continue;

                var interfaceType = interfaces.FirstOrDefault(x => serviceType.GetInterfaces().Contains(x));

                if (interfaceType != null)
                {
                    added.Add(interfaceType);
                    collection.AddSingleton(interfaceType, serviceType);
                }
                else
                {
                    collection.AddSingleton(serviceType, serviceType);
                }
            }

            return added;
        }
    }
}
