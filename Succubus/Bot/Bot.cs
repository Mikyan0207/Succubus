using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Succubus.Handlers;
using Succubus.Store;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Succubus.Bot
{
    public class SuccubusBot : IBot
    {
        private readonly DiscordShardedClient Client;
        private readonly CommandService CommandService;
        private readonly NamedResourceStore<byte[]> ConfigurationStore;
        private readonly BotConfiguration BotConfiguration;
        private readonly IServiceProvider Services;

        public SuccubusBot()
        {
            ConfigurationStore = new NamedResourceStore<byte[]>(new DllResourceStore(new AssemblyName("Succubus.Resources")), @"Configuration");
            
            ConfigurationStore.AddExtension(".json");

            BotConfiguration = JsonConvert.DeserializeObject<BotConfiguration>(Encoding.UTF8.GetString(ConfigurationStore.Get("Bot")));

            Client = new DiscordShardedClient(new DiscordSocketConfig
            {
                MessageCacheSize = 0,
                LogLevel = Discord.LogSeverity.Warning,
                ConnectionTimeout = int.MaxValue,
                AlwaysDownloadUsers = false
            });

            CommandService = new CommandService(new CommandServiceConfig
            {
                CaseSensitiveCommands = false,
                LogLevel = Discord.LogSeverity.Info,
                DefaultRunMode = RunMode.Async
            });
        }

        public async Task RunAsync()
        {
            await Client.LoginAsync(Discord.TokenType.Bot, BotConfiguration.Token).ConfigureAwait(false);
            await Client.StartAsync().ConfigureAwait(false);

            try
            {
                InitializeServices();
            }
            catch
            {
                throw;
            }

            var commandHandler = Services.GetService<CommandHandler>();

            await commandHandler.InitializeAsync().ConfigureAwait(false);

            await Task.Delay(TimeSpan.FromDays(BotConfiguration.AutoRestart)).ConfigureAwait(false);
            await Client.StopAsync().ConfigureAwait(false);
            await Task.Delay(TimeSpan.FromSeconds(10));

            await RunAsync().ConfigureAwait(false);
        }

        private void InitializeServices()
        {
            var s = new ServiceCollection()
                .AddSingleton(CommandService)
                .AddSingleton(Client)
                .AddSingleton(this)
                .AddMemoryCache();

        }
    }
}
