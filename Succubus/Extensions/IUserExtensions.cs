using Discord;
using System;

namespace Succubus.Extensions
{
    // ReSharper disable once InconsistentNaming
    public static class IUserExtensions
    {
        public static Uri GetUserAvatarUrl(this IUser usr)
        {
            if (usr.AvatarId == null)
                return null;

            return new Uri(usr.AvatarId.StartsWith("a_", StringComparison.InvariantCulture)
                ? $"{DiscordConfig.CDNUrl}avatars/{usr.Id}/{usr.AvatarId}.gif"
                : usr.GetAvatarUrl());
        }
    }
}