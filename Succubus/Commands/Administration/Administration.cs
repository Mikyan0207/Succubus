using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Mikyan.Framework.Commands;
using Mikyan.Framework.Commands.Colors;
using Succubus.Commands.Administration.Services;

namespace Succubus.Commands.Administration
{
    public class Administration : Module<AdministrationService>
    {
        [Command("Ban", RunMode = RunMode.Async)]
        [Summary("Ban someone from your server")]
        [RequireOwner]
        [RequireContext(ContextType.Guild)]
        public async Task BanAsync(IGuildUser user, string message = null)
        {
            if (Context.Guild.OwnerId != Context.User.Id)
            {
                await SendErrorAsync("[Administration] Ban", "Only the server owner can ban someone");
                return;
            }

            await Context.Guild.AddBanAsync(user, 7, message, new RequestOptions() {RetryMode = RetryMode.AlwaysRetry}).ConfigureAwait(false);
            await EmbedAsync(
                new EmbedBuilder()
                    .WithTitle($"User banned")
                    .AddField($"{user.Username}", $"{user.Id}", true)
                    .WithCurrentTimestamp()
                    .WithColor(DefaultColors.Purple)
            ).ConfigureAwait(false);
        }
    }
}