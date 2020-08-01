using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Succubus.Commands.Notifications.Services;

namespace Succubus.Commands.Notifications
{
    public class NotificationCommands : SuccubusModule<NotificationService>
    {
        private DiscordShardedClient Client { get; }

        public NotificationCommands(DiscordShardedClient client)
        {
            Client = client;
        }

        [Command("Follow", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        public async Task FollowAsync([Remainder] string channelName)
        {
            var ch = await Service.GetChannel(channelName).ConfigureAwait(false);

            if (ch == null)
                return;

            if (!Context.Guild.Roles.Any(x => x.Name == ch.Name))
                await Context.Guild
                    .CreateRoleAsync(ch.Name, GuildPermissions.None, Service.GetColor(ch.Name), false, true)
                    .ConfigureAwait(false);

            if ((Context.User as SocketGuildUser).Roles.Any(x => x.Name == ch.Name))
                return;

            await (Context.User as SocketGuildUser).AddRoleAsync(Context.Guild.Roles.FirstOrDefault(x => x.Name == ch.Name));
        }

        [Command("Unfollow", RunMode = RunMode.Async)]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        public async Task UnfollowAsync([Remainder] string channelName)
        {
            var ch = await Service.GetChannel(channelName).ConfigureAwait(false);

            if (ch == null)
                return;

            if ((Context.User as SocketGuildUser).Roles.Any(x => x.Name == ch.Name))
            {
                await (Context.User as SocketGuildUser)
                    .RemoveRoleAsync((Context.User as SocketGuildUser).Roles.FirstOrDefault(x => x.Name == ch.Name))
                    .ConfigureAwait(false);
            }
        }

        [Command("Live", RunMode = RunMode.Async)]
        public async Task LiveAsync([Remainder] string channel)
        {
            var ch = await Service.GetChannel(channel).ConfigureAwait(false);
            var res = await Service.IsLive(channel);

            if (res == null)
                return;

            var embedBuilder = new EmbedBuilder();

            embedBuilder.WithAuthor(e =>
            {
                e.Name = res.ChannelName + " 🔴";
                e.IconUrl = ch.Icon;
            });

            embedBuilder.WithFooter(e =>
            {
                e.Text = $"Requested by {Context.Message.Author.Username}";
            }).WithTimestamp(res.PublishedAt);

            embedBuilder.WithTitle($"{res.Title}");
            embedBuilder.WithDescription($"{res.Description}");
            embedBuilder.WithUrl(res.LiveUrl);
            embedBuilder.WithImageUrl(res.ThumbnailUrl);
            embedBuilder.WithColor(Service.GetColor(ch.Name));

            await ReplyAsync($"{Context.Guild.Roles.FirstOrDefault(x => x.Name == ch.Name)?.Mention} {ch.Name} is now live!", false, embedBuilder.Build()).ConfigureAwait(false);
        }
    }
}