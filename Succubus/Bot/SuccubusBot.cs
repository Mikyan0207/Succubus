using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Mikyan.Framework;
using Mikyan.Framework.Services;
using Mikyan.Framework.Stores;
using Newtonsoft.Json;
using NLog;
using Succubus.Database.Context;
using Succubus.Handlers;
using Succubus.Services;
using Victoria;

namespace Succubus.Bot
{
    public class SuccubusBot : Application
    {
        public readonly BotConfiguration BotConfiguration;

        public readonly CommandServiceConfig CommandServiceConfig;

        public readonly NamedResourceStore<byte[]> ConfigurationStore;

        public readonly DbService DbService;

        public readonly DiscordSocketConfig DiscordSocketConfig;

        public SuccubusBot()
        {
            Logger = LogManager.GetCurrentClassLogger();
            ConfigurationStore =
                new NamedResourceStore<byte[]>(new DllResourceStore(new AssemblyName("Succubus.Resources")),
                    @"Configuration");

            ConfigurationStore.AddExtension(".json");

            BotConfiguration =
                JsonConvert.DeserializeObject<BotConfiguration>(Encoding.UTF8.GetString(ConfigurationStore.Get("Bot")));

            DiscordSocketConfig = new DiscordSocketConfig
            {
                MessageCacheSize = 0,
                LogLevel = LogSeverity.Warning,
                ConnectionTimeout = int.MaxValue,
                AlwaysDownloadUsers = false
            };

            CommandServiceConfig = new CommandServiceConfig
            {
                CaseSensitiveCommands = false,
                LogLevel = LogSeverity.Info,
                DefaultRunMode = RunMode.Async
            };

            DbService = new DbService();

            Initialize(DiscordSocketConfig, CommandServiceConfig, BotConfiguration.Token);

            Services = InitializeDefaultServices()
                .LoadFrom(Assembly.GetExecutingAssembly())
                .AddSingleton(new BotService())
                .AddSingleton(new LocalizationService())
                .AddSingleton(DbService)
                .AddDbContext<SuccubusContext>()
                .AddLavaNode(x =>
                {
                    x.SelfDeaf = false;
                })
                .BuildServiceProvider();

            Client.ShardReady += Client_ShardReady;
        }

        public new async Task RunAsync()
        {
            await Services.GetService<CommandHandler>().InitializeAsync().ConfigureAwait(false);
            await Client.LoginAsync(TokenType.Bot, Token).ConfigureAwait(false);
            await Client.StartAsync().ConfigureAwait(false);
            await Task.Delay(-1);
        }

        private Task Client_ShardReady(DiscordSocketClient e)
        {
            var lavaNode = Services.GetService<LavaNode>();

            if (!lavaNode.IsConnected)
                lavaNode.ConnectAsync().ConfigureAwait(false);

            Logger.Info($"{e.CurrentUser.Username} [Shard #{e.ShardId}] Ready ✔️");

            return Task.CompletedTask;
        }
    }
}