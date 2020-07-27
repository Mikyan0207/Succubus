using Discord.WebSocket;
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
    public class Bot : IBot
    {
        private readonly DiscordShardedClient Client;
        private readonly ICommandHandler CommandHandler;
        private readonly NamedResourceStore<byte[]> ConfigurationStore;
        private readonly Configuration BotConfiguration;

        public Bot(DiscordShardedClient client, ICommandHandler commandHandler)
        {
            Client = client;
            CommandHandler = commandHandler;
            ConfigurationStore = new NamedResourceStore<byte[]>(new DllResourceStore(new AssemblyName("Succubus.Resources")), @"Configuration");

            ConfigurationStore.AddExtension(".json");

            BotConfiguration = JsonConvert.DeserializeObject<Configuration>(Encoding.UTF8.GetString(ConfigurationStore.Get("Bot")));
        }

        public async Task RunAsync()
        {
            await Client.LoginAsync(Discord.TokenType.Bot, BotConfiguration.Token).ConfigureAwait(false);
            await Client.StartAsync().ConfigureAwait(false);

            await CommandHandler.InitializeAsync().ConfigureAwait(false);

            await Task.Delay(TimeSpan.FromDays(BotConfiguration.AutoRestart)).ConfigureAwait(false);
            await Client.StopAsync().ConfigureAwait(false);
            await Task.Delay(TimeSpan.FromSeconds(10));

            await RunAsync().ConfigureAwait(false);
        }
    }
}
