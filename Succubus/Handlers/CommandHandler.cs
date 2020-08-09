using Discord.Commands;
using Discord.WebSocket;
using Mikyan.Framework.Services;
using NLog;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Succubus.Handlers
{
    public class CommandHandler : IService
    {
        private static readonly NLog.Logger _Logger = LogManager.GetCurrentClassLogger();
        private readonly DiscordShardedClient Client;
        private readonly CommandService CommandService;
        private readonly IServiceProvider Services;

        public CommandHandler(DiscordShardedClient client, CommandService commandService, IServiceProvider services)
        {
            Client = client;
            CommandService = commandService;
            Services = services;
        }

        public async Task InitializeAsync()
        {
            await CommandService.AddModulesAsync(Assembly.GetEntryAssembly(), Services).ConfigureAwait(false);

            Client.MessageReceived += HandleCommand;
        }

        private Task HandleCommand(SocketMessage message)
        {
            if (!(message is SocketUserMessage msg))
                return Task.CompletedTask;

            Task.Run(async () => await HandleCommand(msg).ConfigureAwait(false));

            return Task.CompletedTask;
        }

        private async Task HandleCommand(SocketUserMessage message)
        {
            if (message.Author.IsBot)
                return;

            var context = new ShardedCommandContext(Client, message);
            var argPos = 0;

            if (context.Message.HasStringPrefix("!", ref argPos, StringComparison.CurrentCultureIgnoreCase))
            {
                await HandleCommandAsync(context, argPos);

                _Logger.Info(
                    $"Command Executed\nUser: {message.Author.Username}\nChannel: {message.Channel.Name}\nCommand: {message.Content}");
            }
        }

        public async Task HandleCommandAsync(ShardedCommandContext context, int argPos)
        {
            try
            {
                var searchResult = CommandService.Search(context, argPos);

                // If no command were found, return
                if (searchResult.Commands == null || searchResult.Commands.Count == 0) return;

                var result = await CommandService.ExecuteAsync(context, argPos, Services).ConfigureAwait(false);

                if (!result.IsSuccess)
                {
                    // TO DO: Error Message+Log
                }
            }
            catch (Exception e)
            {
                _Logger.Error(e.Message);
            }
        }
    }
}