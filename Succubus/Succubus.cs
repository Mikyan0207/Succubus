using Microsoft.Extensions.DependencyInjection;
using NLog;
using Succubus.Common;
using Succubus.Services;
using System;
using System.Reflection;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using Succubus.Services.Extensions;

namespace Succubus
{
    public class Succubus
    {
        public DiscordClient Client { get; }

        public CommandsNextExtension CommandService { get; }

        public IServiceProvider Services { get; set; }

        public static Logger Logger { get; set; } = LogManager.GetCurrentClassLogger();

        public ConfigurationService ConfigurationService { get; }

        public Succubus()
        {
            Log.InitializeLogger();

            ConfigurationService = new ConfigurationService();

            Client = new DiscordClient(new DiscordConfiguration
            {
                MessageCacheSize = 0,
                Token = ConfigurationService.Configuration.Token,
            });

            Services = new ServiceCollection()
                .AddSingleton(Client)
                .AddSingleton(ConfigurationService)
                .AddSingleton(new DatabaseService())
                .LoadSuccubusServices(Assembly.GetCallingAssembly())
                .BuildServiceProvider();

            CommandService = Client.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefixes = new [] { "$", "!", "." },
                EnableDms = false,
                EnableMentionPrefix = false,
                Services = Services
            });

            CommandService.RegisterCommands(Assembly.GetExecutingAssembly());
        }

        public async Task RunAsync()
        {
            await Client.ConnectAsync().ConfigureAwait(false);
            await Task.Delay(-1).ConfigureAwait(false);
        }
    }
}