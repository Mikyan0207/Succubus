using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using NLog;
using Succubus.Services.Interfaces;

namespace Succubus.Services
{
    public class CommandHandler : IService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private DiscordShardedClient Client { get; }

        private CommandService CommandService { get; }

        private PrefixService PrefixService { get; }

        private IServiceProvider Services { get; }


        public CommandHandler(DiscordShardedClient client, CommandService commandService, PrefixService prefixService, IServiceProvider provider)
        {
            Client = client;
            CommandService = commandService;
            PrefixService = prefixService;
            Services = provider;

            Client.MessageReceived += HandleMessageAsync;
        }

        private Task HandleMessageAsync(SocketMessage message)
        {
            Task.Run(async () =>
            {
                await MessageHandler(message).ConfigureAwait(false);
            });

            return Task.CompletedTask;
        }

        private async Task MessageHandler(SocketMessage message)
        {
            if (message.Author.IsBot)
                return;

            if (!(message is SocketUserMessage msg))
                return;

            var channel = msg.Channel as ISocketMessageChannel;
            var guild = (msg.Channel as SocketTextChannel)?.Guild;

            await TryExecuteCommandAsync(guild, channel, msg).ConfigureAwait(false);
        }

        private async Task TryExecuteCommandAsync(SocketGuild guild, ISocketMessageChannel channel, SocketUserMessage message)
        {
            var prefix = PrefixService.GetPrefix(guild.Id);

            if (!message.Content.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase))
                return;

            await ExecuteCommandAsync(new ShardedCommandContext(Client, message), message.Content.Substring(1))
                .ConfigureAwait(false);
        }

        private async Task ExecuteCommandAsync(ICommandContext context, string content)
        {
            var searchResult = CommandService.Search(context, content);

            if (!searchResult.IsSuccess)
                return;

            var cmd = searchResult.Commands
                .OrderBy(x => x.Command.Priority)
                .FirstOrDefault()
                .Command;

            await CommandService.ExecuteAsync(context, content, Services).ConfigureAwait(false);
        }
    }
}