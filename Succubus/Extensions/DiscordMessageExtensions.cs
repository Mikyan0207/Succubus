using System;
using System.Threading.Tasks;
using DSharpPlus.Entities;

namespace Succubus.Extensions
{
    public static class DiscordMessageExtensions
    {
        public static DiscordMessage DeleteAfter(this DiscordMessage message, string reason, TimeSpan countdown)
        {
            Task.Run(async () =>
            {
                await Task.Delay(countdown).ConfigureAwait(false);
                await message.DeleteAsync(reason).ConfigureAwait(false);
            });

            return message;
        }
    }
}