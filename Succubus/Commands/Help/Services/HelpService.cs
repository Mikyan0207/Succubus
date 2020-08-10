using System;
using System.Linq;
using CommandLine;
using Discord;
using Discord.Commands;
using Mikyan.Framework.Commands.Attributes;
using Mikyan.Framework.Commands.Colors;
using Mikyan.Framework.Services;

namespace Succubus.Commands.Help.Services
{
    public class HelpService : IService
    {
        private readonly CommandService _commandService;

        public HelpService(CommandService commandService)
        {
            _commandService = commandService;
        }

        public Embed GetModuleCommands(string name)
        {
            var commands = _commandService.Commands.Where(c =>
                    c.Module.Name.ToLowerInvariant().StartsWith(name, StringComparison.OrdinalIgnoreCase))
                .OrderBy(c => c.Aliases[0])
                .ToList();

            if (!commands.Any())
                return new EmbedBuilder()
                    .WithTitle($"Module `{name}` not found. Try again.")
                    .WithColor(DefaultColors.Error)
                    .Build();

            var eb = new EmbedBuilder()
                .WithTitle($"List of commands for `{commands[0].Module.Name}`")
                .WithColor(DefaultColors.Info)
                .WithCurrentTimestamp();

            foreach (var command in commands)
            {
                eb.AddField(e =>
                {
                    e.Name = command.Name;
                    e.Value = !string.IsNullOrEmpty(command.Summary) ? command.Summary : "No description";
                    e.IsInline = false;
                });
            }

            return eb.Build();
        }

        public Embed GetCommand(ShardedCommandContext ctx, string info)
        {
            var command = _commandService.Search(ctx, info).Commands.FirstOrDefault().Command;

            if (command == null)
                return null;

            var eb = new EmbedBuilder
            {
                Color = DefaultColors.Info,
                Timestamp = DateTimeOffset.Now
            };

            var cmdName = $"**`{command.Aliases.First()}`**";

            if (command.Aliases.Skip(1).FirstOrDefault() != null)
                cmdName += $" / **`{command.Aliases.Skip(1).FirstOrDefault()}`**";

            eb.AddField(e =>
            {
                e.Name = cmdName;
                e.Value = command.Summary;
                e.IsInline = true;
            });

            var options = ((OptionsAttribute) command.Attributes.FirstOrDefault(x => x is OptionsAttribute))?.Type;

            if (options == null)
                return eb.Build();

            var flags = options.GetProperties()
                .Select(x => x.GetCustomAttributes(true).FirstOrDefault(y => y is OptionAttribute))
                .Where(x => x != null)
                .Cast<OptionAttribute>()
                .Select(x =>
                {
                    var r = $"`--{x.LongName}`";

                    if (!string.IsNullOrEmpty(x.ShortName))
                        r += $" / `-{x.ShortName}`";

                    if (!string.IsNullOrEmpty(x.HelpText))
                        r += $"\n{x.HelpText}\n";

                    return r;
                }).ToList();

            eb.AddField(e =>
            {
                e.Name = "Options";
                e.Value = string.Join("\n", flags);
                e.IsInline = false;
            });

            return eb.Build();
        }
    }
}