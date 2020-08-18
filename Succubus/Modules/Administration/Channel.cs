using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace Succubus.Modules.Administration
{
    [RequireBotPermissions(Permissions.AccessChannels | Permissions.ManageChannels | Permissions.ManageMessages)]
    [RequireUserPermissions(Permissions.AccessChannels | Permissions.ManageChannels | Permissions.ManageMessages)]
    [Group]
    public class Channel : SuccubusModule
    {
        [Command("clear")]
        public async Task ClearCommandAsync(CommandContext ctx, int amount = 10)
        {
            if (amount > 100)
                amount = 100;

            await ctx.Message.DeleteAsync().ConfigureAwait(false);

            // ReSharper disable once IdentifierTypo
            var msgs = await ctx.Channel.GetMessagesAsync(amount).ConfigureAwait(false);
            
            await ctx.Channel.DeleteMessagesAsync(msgs).ConfigureAwait(false);
        }

        [Command("nuke")]
        public async Task NukeCommandAsync(CommandContext ctx)
        {
            // ReSharper disable once IdentifierTypo
            var msgs = await ctx.Channel.GetMessagesAsync().ConfigureAwait(false);

            while (msgs.Any())
            {
                await ctx.Channel.DeleteMessagesAsync(msgs).ConfigureAwait(false);
                msgs = await ctx.Channel.GetMessagesAsync().ConfigureAwait(false);
            }
        }
    }
}