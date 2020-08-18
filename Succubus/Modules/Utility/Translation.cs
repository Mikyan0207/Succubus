using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Succubus.Services;

namespace Succubus.Modules.Utility
{
    public class Translation : SuccubusModule
    {
        private readonly TranslationService _service;

        public Translation(TranslationService translationService)
        {
            _service = translationService;
        }

        [Command("translate"), Aliases("t", "tr")]
        [Description("Translate text to another language")]
        public async Task TranslateCommandAsync(CommandContext ctx,
            [Description("Target Language")] string to,
            [Description("Text to translate")][RemainingText] string text)
        {
            if (string.IsNullOrWhiteSpace(to) || string.IsNullOrWhiteSpace(text))
                return;

            var result = await _service.TranslateAsync(text, to).ConfigureAwait(false);

            await EmbedAsync(ctx,
                new DiscordEmbedBuilder()
                    .AddField(
                        $"Translated in {result.TargetLanguage.FullName}",
                        $"{result.MergedTranslation}")
                    .WithColor(DiscordColor.Blurple)
            ).ConfigureAwait(false);
        }
    }
}