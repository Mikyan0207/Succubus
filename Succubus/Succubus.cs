using DSharpPlus;
using DSharpPlus.CommandsNext;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using Succubus.Common;
using Succubus.Services;
using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.VoiceNext;
using Succubus.Extensions;

namespace Succubus
{
    public class Succubus
    {
        public DiscordClient Client { get; }

        public CommandsNextExtension CommandService { get; }

        public InteractivityExtension InteractivityService { get; }

        public VoiceNextExtension VoiceService { get; }

        public IServiceProvider Services { get; set; }

        public static Logger Logger { get; set; } = LogManager.GetCurrentClassLogger();

        public ConfigurationService ConfigurationService { get; }

        private static readonly Regex TwitterRegex = new Regex("http(?:s)?:/\\/(?:www)?twitter\\.com\\/([a-zA-Z0-9_]+)\\/status\\/([0-9]+)");

        public Succubus()
        {
            Log.InitializeLogger();

            ConfigurationService = new ConfigurationService();

            Client = new DiscordClient(new DiscordConfiguration
            {
                MessageCacheSize = 0,
                Token = ConfigurationService.Configuration.Token
            });

            Services = new ServiceCollection()
                .AddSingleton(Client)
                .AddSingleton(ConfigurationService)
                .AddSingleton<DatabaseService>()
                .AddSingleton<TranslationService>()
                .LoadSuccubusServices(Assembly.GetCallingAssembly())
                .BuildServiceProvider();

            CommandService = Client.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefixes = new[] { "$", "!", ".", "?" },
                EnableDms = false,
                EnableMentionPrefix = false,
                Services = Services
            });

            InteractivityService = Client.UseInteractivity(new InteractivityConfiguration
            {
                PaginationBehaviour = PaginationBehaviour.WrapAround,
                PaginationDeletion = PaginationDeletion.DeleteMessage,
                Timeout = TimeSpan.FromMinutes(2),
                PollBehaviour = PollBehaviour.KeepEmojis,
            });

            VoiceService = Client.UseVoiceNext(new VoiceNextConfiguration
            {
                AudioFormat = AudioFormat.Default,
                EnableIncoming = false
            });

            CommandService.RegisterCommands(Assembly.GetExecutingAssembly());

            Client.MessageCreated += Client_MessageCreated;
        }

        private async Task Client_MessageCreated(DSharpPlus.EventArgs.MessageCreateEventArgs e)
        {
            if (!TwitterRegex.IsMatch(e.Message.Content))
                return;

            var discordEmbed = e.Message.Embeds.FirstOrDefault();
            var translationService = Services.GetService<TranslationService>();
            if (translationService != null)
            {
                var result = await translationService.TranslateAsync(discordEmbed?.Description, "French")
                    .ConfigureAwait(false);
                var embed = new DiscordEmbedBuilder()
                    .AddField(
                        $"Auto-Translation in {result.TargetLanguage.FullName} by Succubus",
                        $"{result.MergedTranslation}")
                    .WithColor(DiscordColor.Blurple);
                await Client.SendMessageAsync(e.Channel, embed: embed).ConfigureAwait(false);
            }
        }

        public async Task RunAsync()
        {
            await Client.ConnectAsync().ConfigureAwait(false);
            await Task.Delay(-1).ConfigureAwait(false);
        }
    }
}