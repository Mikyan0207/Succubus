using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using FormatWith;
using Microsoft.Extensions.Configuration;
using Mikyan.Framework.Services;
using Mikyan.Framework.Stores;

namespace Succubus.Services
{
    public class LocalizationService : IService
    {
        private readonly ConcurrentDictionary<string, IConfiguration> _locales;

        public LocalizationService()
        {
            _locales = new ConcurrentDictionary<string, IConfiguration>();

            using var store =
                new NamedResourceStore<byte[]>(new DllResourceStore(new AssemblyName("Succubus.Resources")),
                    @"Locales");
            store.AddExtension(".yml");

            var files = store.GetResources();

            foreach (var file in files)
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Path.Combine(
                        Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.Parent.FullName,
                        "Succubus.Resources/Locales/"))
                    .AddYamlFile(file);
                var locale = builder.Build();

                _locales.TryAdd(Path.GetFileNameWithoutExtension(file), locale);
            }
        }

        public KeyValuePair<string, IConfiguration> GetLocale(string id)
        {
            return _locales.AsEnumerable().SingleOrDefault(x => x.Key == id);
        }

        public string GetText(string id, object obj, string locale)
        {
            return _locales.TryGetValue(locale, out var value)
                ? value?[id].FormatWith(obj, MissingKeyBehaviour.Ignore)
                : default;
        }

        public string GetText(string id, Dictionary<string, object> obj, string locale)
        {
            return _locales.TryGetValue(locale, out var value)
                ? value[id].FormatWith(obj, MissingKeyBehaviour.Ignore)
                : default;
        }

        public string GetText(string id, KeyValuePair<string, object> obj, string locale)
        {
            return _locales.TryGetValue(locale, out var value)
                ? value[id].FormatWith(obj, MissingKeyBehaviour.Ignore)
                : default;
        }
    }
}