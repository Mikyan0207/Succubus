using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Mikyan.Framework.Commands;
using Mikyan.Framework.Commands.Attributes;
using Mikyan.Framework.Commands.Colors;
using Mikyan.Framework.Commands.Parsers;
using Succubus.Commands.Locale.Options;
using Succubus.Commands.Locale.Services;
using Succubus.Services;

namespace Succubus.Commands.Locale
{
    [Name("Locale")]
    public class LocaleCommands : Module<LocaleService>
    {
        private readonly LocalizationService _localizationService;

        public LocaleCommands(LocalizationService localizationService)
        {
            _localizationService = localizationService;
        }

        [Command("locale", RunMode = RunMode.Async)]
        [Summary("Get or Set server language")]
        [Options(typeof(LocaleOptions))]
        [RequireUserPermission(GuildPermission.ManageGuild)]
        public async Task LocaleAsync(params string[] args)
        {
            var options = OptionsParser.Parse<LocaleOptions>(args);

            if (string.IsNullOrEmpty(options.Locale))
            {
                var locale = await Service.GetServerLocale(Context.Guild).ConfigureAwait(false);

                await EmbedAsync(
                    new EmbedBuilder()
                        .AddField(e =>
                        {
                            e.Name = _localizationService.GetText("locale:get_locale_title",
                                new Dictionary<string, object> { { "Name", Context.Guild.Name } }, locale);
                            e.Value = _localizationService.GetText("locale:get_locale",
                                new Dictionary<string, object> { { "Code", locale } }, locale);
                            e.IsInline = true;
                        })
                        .WithColor(DefaultColors.Purple)
                ).ConfigureAwait(false);
            }
            else
            {
                var result = await Service.SetServerLocale(Context.Guild, options.Locale).ConfigureAwait(false);

                if (result)
                {
                    await SendConfirmationAsync(
                        _localizationService.GetText("locale:set_locale_title",
                            new Dictionary<string, object> { { "Name", Context.Guild.Name } }, options.Locale),
                        _localizationService.GetText("locale:set_locale",
                            new Dictionary<string, object> { { "Code", options.Locale } }, options.Locale)
                    ).ConfigureAwait(false);
                }
                else
                {
                    await SendErrorAsync(
                        _localizationService.GetText("locale:set_locale_error_title",
                            new Dictionary<string, object> { { "Name", Context.Guild.Name } }, options.Locale),
                        _localizationService.GetText("locale:set_locale_error",
                            new Dictionary<string, object> { { "Code", options.Locale } }, options.Locale)
                    ).ConfigureAwait(false);
                }
            }


        }
    }
}