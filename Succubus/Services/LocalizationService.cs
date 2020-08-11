using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using Succubus.Common;
using Succubus.Services.Interfaces;

namespace Succubus.Services
{
    public class LocalizationService : IService
    {
        private DatabaseService DbService { get; }

        public CultureInfo DefaultCultureInfo { get; set; }

        private ConcurrentDictionary<CultureInfo, ConcurrentDictionary<string, string>> Responses { get; }

        private ConcurrentDictionary<CultureInfo, ImmutableList<Command>> Commands { get; }

        public LocalizationService(DatabaseService dbService)
        {
            DbService = dbService;
            Responses = new ConcurrentDictionary<CultureInfo, ConcurrentDictionary<string, string>>();
            Commands = new ConcurrentDictionary<CultureInfo, ImmutableList<Command>>();
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