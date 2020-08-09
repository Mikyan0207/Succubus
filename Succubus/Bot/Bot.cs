﻿using System;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Mikyan.Framework;
using Mikyan.Framework.Stores;
using Newtonsoft.Json;
using Succubus.Handlers;
using Succubus.Services;

namespace Succubus.Bot
{
    public class SuccubusBot : Application
    {
        public readonly BotConfiguration BotConfiguration;

        public readonly NamedResourceStore<byte[]> ConfigurationStore;

        public readonly DbService DbService;

        public readonly DiscordSocketConfig DiscordSocketConfig;

        public readonly CommandServiceConfig CommandServiceConfig;

        public SuccubusBot()
        {
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
                .AddSingleton(DbService)
                .AddSingleton(DbService.GetDbContext())
                .BuildServiceProvider();
        }

        public new async Task RunAsync()
        {
            await Services.GetService<CommandHandler>().InitializeAsync().ConfigureAwait(false);
            await Client.LoginAsync(TokenType.Bot, Token).ConfigureAwait(false);
            await Client.StartAsync().ConfigureAwait(false);
            await Task.Delay(-1);
        }
    }
}