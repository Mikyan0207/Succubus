using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Succubus.Common;
using Succubus.Common.Stores;
using Succubus.Services.Interfaces;

namespace Succubus.Services
{
    public class LocalizationService : IService
    {
        private DatabaseService DbService { get; }

        public CultureInfo DefaultCultureInfo { get; set; }

        private ConcurrentDictionary<CultureInfo, ConcurrentDictionary<string, string>> Responses { get; }

        private ConcurrentDictionary<CultureInfo, IEnumerable<Command>> Commands { get; }

        public LocalizationService(DatabaseService dbService)
        {
            DbService = dbService;
            Responses = new ConcurrentDictionary<CultureInfo, ConcurrentDictionary<string, string>>();
            Commands = new ConcurrentDictionary<CultureInfo, IEnumerable<Command>>();

            {
                using var store = new NamedStore(new Store("Succubus.Resources"), "Commands");

                foreach (var resource in store.GetResources())
                {
                    var content = JsonConvert.DeserializeObject<List<Command>>(store.Get(resource));
                    Commands.TryAdd(
                        new CultureInfo(resource.Substring(resource.IndexOf('_'), resource.LastIndexOf('.'))),
                        content);
                }
            }
            {
                using var store = new NamedStore(new Store("Succubus.Resources"), "Responses");

                foreach (var resource in store.GetResources())
                {
                    var responses = JsonConvert.DeserializeObject<ConcurrentDictionary<string, string>>(store.Get(resource));

                    Responses.TryAdd(
                        new CultureInfo(resource.Substring(resource.IndexOf('_'), resource.LastIndexOf('.'))),
                        responses);
                }
            }
        }

        public string GetText(string key, CultureInfo cultureInfo, params object[] replacements)
        {
            if (!Responses.TryGetValue(cultureInfo, out var dict))
                return default;

            var result =  dict.TryGetValue(key, out var value) ? value : default;

            try
            {
                return string.Format(result ?? "", replacements);
            }
            catch (FormatException)
            {
                // TODO: Add Logger message
                return default;
            }
        }

        public Command GetCommand(CultureInfo info, string name)
        {
            if (Commands.TryGetValue(info, out var commands))
                return commands.FirstOrDefault(x => string.Equals(x?.Name, name, StringComparison.InvariantCultureIgnoreCase));

            // TODO: throw CommandNotFoundException
            return default;
        }
    }
}