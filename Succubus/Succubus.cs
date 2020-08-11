using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using Succubus.Database;
using Succubus.Services;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Succubus
{
    public class Succubus
    {
        public DiscordShardedClient Client { get; set; }

        public CommandService CommandService { get; set; }

        public IServiceProvider Services { get; set; }

        public Logger Logger { get; set; }

        public Succubus()
        {
            Client = new DiscordShardedClient(new DiscordSocketConfig
            {
                AlwaysDownloadUsers = false,
                MessageCacheSize = 0,
                DefaultRetryMode = RetryMode.AlwaysRetry,
                GatewayIntents = GatewayIntents.Guilds | GatewayIntents.GuildMessages | GatewayIntents.GuildBans | GatewayIntents.GuildVoiceStates,
                TotalShards = 10
            });

            CommandService = new CommandService(new CommandServiceConfig
            {
                CaseSensitiveCommands = false,
                DefaultRunMode = RunMode.Sync,
                LogLevel = LogSeverity.Info,
            });

            Services = new ServiceCollection()
                .AddSingleton(Client)
                .AddSingleton(CommandService)
                .AddDbContext<SuccubusContext>()
                .LoadSuccubusServices(Assembly.GetExecutingAssembly())
                .BuildServiceProvider();

            Logger = LogManager.GetCurrentClassLogger();

            Client.ShardReady += Client_ShardReady;
        }

        private Task Client_ShardReady(DiscordSocketClient e)
        {
            Logger.Info($"{e.CurrentUser.Username} [Shard #{e.ShardId}] Ready ✔️");

            return Task.CompletedTask;
        }
    }
}