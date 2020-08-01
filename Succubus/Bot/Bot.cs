using System;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NLog;
using Succubus.Handlers;
using Succubus.Services;
using Succubus.Store;

namespace Succubus.Bot
{
    public class SuccubusBot : IBot
    {
        private static readonly NLog.Logger _Logger = LogManager.GetCurrentClassLogger();
        private readonly BotConfiguration BotConfiguration;
        private readonly DiscordShardedClient Client;
        private readonly CommandService CommandService;
        private readonly NamedResourceStore<byte[]> ConfigurationStore;
        private readonly DbService DbService;
        private IServiceProvider Services;

        public SuccubusBot()
        {
            ConfigurationStore =
                new NamedResourceStore<byte[]>(new DllResourceStore(new AssemblyName("Succubus.Resources")),
                    @"Configuration");

            ConfigurationStore.AddExtension(".json");

            BotConfiguration =
                JsonConvert.DeserializeObject<BotConfiguration>(Encoding.UTF8.GetString(ConfigurationStore.Get("Bot")));

            Client = new DiscordShardedClient(new DiscordSocketConfig
            {
                MessageCacheSize = 0,
                LogLevel = LogSeverity.Warning,
                ConnectionTimeout = int.MaxValue,
                AlwaysDownloadUsers = false
            });

            CommandService = new CommandService(new CommandServiceConfig
            {
                CaseSensitiveCommands = false,
                LogLevel = LogSeverity.Info,
                DefaultRunMode = RunMode.Async
            });

            DbService = new DbService();
        }

        public async Task RunAsync()
        {
            _Logger.Info("Connecting...");

            await Client.LoginAsync(TokenType.Bot, BotConfiguration.Token).ConfigureAwait(false);
            await Client.StartAsync().ConfigureAwait(false);

            _Logger.Info("Connected.");

            try
            {
                InitializeServices();
            }
            catch (Exception ex)
            {
                _Logger.Fatal("Failed to initialize services. Bot will shutdown in 5 seconds\n" + ex.Message);

                await Task.Delay(TimeSpan.FromSeconds(5)).ConfigureAwait(false);
                await Client.StopAsync().ConfigureAwait(false);
                throw;
            }

            _Logger.Info("Services Initialized.");

            var commandHandler = Services.GetService<CommandHandler>();

            await commandHandler.InitializeAsync().ConfigureAwait(false);

            _Logger.Info("CommandHandler Initialized.");
            _Logger.Info("Bot Ready.");

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
                .AddSingleton(DbService)
                .AddSingleton(DbService.GetDbContext())
                .AddSingleton(this)
                .AddMemoryCache();

            s.LoadFrom(Assembly.GetAssembly(typeof(CommandHandler)));

            Services = s.BuildServiceProvider();
        }
    }
}