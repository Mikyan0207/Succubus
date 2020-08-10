using System;
using System.Linq;
using CommandLine;
using Discord;
using Discord.Commands;
using Mikyan.Framework.Commands.Attributes;
using Mikyan.Framework.Services;

namespace Succubus.Commands.Help.Services
{
    public class HelpService : IService
    {
        private CommandService _commandService;

        public HelpService(CommandService commandService)
        {
            _commandService = commandService;
        }

        public Embed GetCommand(ShardedCommandContext ctx, string info)
        {
            var command = _commandService.Search(ctx, info).Commands.FirstOrDefault().Command;

            if (command == null)
                return null;

            var eb = new EmbedBuilder()
            {
                Color = new Color(21, 101, 192),
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
