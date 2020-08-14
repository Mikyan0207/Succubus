using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using Succubus.Services.Interfaces;
using System.Threading.Tasks;

namespace Succubus.Modules
{
    public class SuccubusModule : BaseCommandModule
    {
        protected virtual async Task<DiscordMessage> EmbedAsync(CommandContext ctx, DiscordEmbedBuilder embed, string message = null)
        {
            await ctx.Channel.TriggerTypingAsync().ConfigureAwait(false);

            return await ctx.Channel.SendMessageAsync(message ?? "", false, embed).ConfigureAwait(false);
        }
    }

    public class SuccubusModule<TService> : SuccubusModule where TService : IService
    {
        public TService Service { get; set; }
    }
}