using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using Succubus.Common;
using Succubus.Services;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Succubus.Database;
using Succubus.Services.Extensions;

namespace Succubus
{
    public class Succubus
    {
        public DiscordShardedClient Client { get; set; }

        public CommandService CommandService { get; set; }

        public IServiceProvider Services { get; set; }

        public static Logger Logger { get; set; } = LogManager.GetCurrentClassLogger();

        public ConfigurationService ConfigurationService { get; }

        public Succubus()
        {
            Log.InitializeLogger();

            ConfigurationService = new ConfigurationService("Succubus.Resources", "SuccubusConfiguration.json");

            Client = new DiscordShardedClient(new DiscordSocketConfig
            {
                AlwaysDownloadUsers = false,
                MessageCacheSize = 0,
                DefaultRetryMode = RetryMode.AlwaysRetry,
                GatewayIntents = GatewayIntents.Guilds | GatewayIntents.GuildMessages | GatewayIntents.GuildBans | GatewayIntents.GuildVoiceStates | GatewayIntents.GuildMembers,
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
                .AddSingleton(ConfigurationService)
                .AddSingleton(new DatabaseService())
                .LoadSuccubusServices(Assembly.GetCallingAssembly())
                .BuildServiceProvider()
                .LoadSuccubusModules(Assembly.GetCallingAssembly(), CommandService);

            Services.GetService<CommandHandler>();

            Client.ShardReady += Client_ShardReady;
            Client.JoinedGuild += Client_JoinedGuild;
            Client.LeftGuild += Client_LeftGuild;
            CommandService.CommandExecuted += CommandService_CommandExecuted;
        }

        public async Task RunAsync()
        {
            await Client.LoginAsync(TokenType.Bot, ConfigurationService.Configuration.Token).ConfigureAwait(false);
            await Client.StartAsync().ConfigureAwait(false);
            await Task.Delay(-1).ConfigureAwait(false);
        }

        private static Task Client_ShardReady(DiscordSocketClient e)
        {
            Logger.Info($"{e.CurrentUser.Username} [Shard #{e.ShardId}] Ready ✔️");

            return Task.CompletedTask;
        }

        private static Task Client_LeftGuild(SocketGuild e)
        {
            Logger.Info($"Guild Left 💧\n[\n\t"
                        + $"{"Name",-12}{e.Name}\n\t"
                        + $"{"Owner",-12}{e.Owner.Username}\n\t"
                        + $"{"Members",-12}{e.MemberCount}\n\t"
                        + $"{"Created",-12}{e.CreatedAt.DateTime}\n]");

            return Task.CompletedTask;
        }

        private static Task Client_JoinedGuild(SocketGuild e)
        {
            Logger.Info($"Guild Joined ⚡\n[\n\t"
                        + $"{"Name",-12}{e.Name}\n\t"
                        + $"{"Owner",-12}{e.Owner.Username}\n\t"
                        + $"{"Members",-12}{e.MemberCount}\n\t"
                        + $"{"Created",-12}{e.CreatedAt.DateTime}\n]");

            return Task.CompletedTask;
        }

        private static Task CommandService_CommandExecuted(Optional<CommandInfo> info, ICommandContext ctx, IResult res)
        {
            var e = info.Value;

            if (res.IsSuccess)
            {
                Logger.Info($"Command Executed ✔️\n[\n\t"
                            + $"{"Name",-12}{e.Name}\n\t"
                            + $"{"User",-12}{ctx.Message.Author.Username}\n\t"
                            + $"{"Channel",-12}{ctx.Channel.Name}\n\t"
                            + $"{"Guild",-12}{ctx.Guild.Name ?? "Direct Message"}\n\t"
                            + $"{"Date",-12}{ctx.Message.Timestamp}\n\t"
                            + $"{"Raw Message",-12}{ctx.Message.Content}\n]");
            }
            else
            {
                Logger.Error($"Command Errored ❌\n[\n\t"
                             + $"{"Name",-12}{e.Name}\n\t"
                             + $"{"User",-12}{ctx.Message.Author.Username}\n\t"
                             + $"{"Channel",-12}{ctx.Channel.Name}\n\t"
                             + $"{"Guild",-12}{ctx.Guild.Name ?? "Direct Message"}\n\t"
                             + $"{"Date",-12}{ctx.Message.Timestamp}\n\t"
                             + $"{"Raw Message",-12}{ctx.Message.Content}\n\t"
                             + $"{"Reason",-12}{res.ErrorReason}\n]");
            }

            return Task.CompletedTask;
        }
    }
}