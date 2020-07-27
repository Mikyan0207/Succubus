using Discord;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Text;

namespace Succubus.Extensions
{
    public static class IUserExtensions
    {
        public static Uri GetUserAvatarUrl(this IUser usr)
        {
            if (usr.AvatarId == null)
                return null;

            return new Uri(usr.AvatarId.StartsWith("a_", StringComparison.InvariantCulture) ? $"{DiscordConfig.CDNUrl}avatars/{usr.Id}/{usr.AvatarId}.gif" : usr.GetAvatarUrl(ImageFormat.Auto));
        }
    }
}
