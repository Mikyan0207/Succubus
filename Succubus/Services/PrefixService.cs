using Discord;
using NLog;
using Succubus.Database;
using Succubus.Database.Models;
using System.Data;
using System.Linq;

namespace Succubus.Services
{
    public class PrefixService : IService
    {
        private readonly SuccubusContext _context;
        private readonly Logger _logger;

        public PrefixService(SuccubusContext context)
        {
            _context = context;
            _logger = LogManager.GetCurrentClassLogger();
        }

        public string GetPrefix(IGuild guild)
        {
            return GetPrefix(guild.Id);
        }

        public string GetPrefix(ulong guildId)
        {
            return _context.Guilds
                .FirstOrDefault(x => x.GuildId == guildId)
                ?.Prefix;
        }

        public string GetPrefix(string name)
        {
            return _context.Guilds
                .FirstOrDefault(x => x.Name == name)
                ?.Prefix;
        }

        public void SetPrefix(IGuild guild, string prefix)
        {
            var g = _context.Guilds.FirstOrDefault(x => x.GuildId == guild.Id);

            if (g != null)
            {
                g.Prefix = prefix;
            }
            else
            {
                g = new Guild
                {
                    GuildId = guild.Id,
                    Name = guild.Name,
                    Locale = "fr-FR",
                    Prefix = prefix
                };
            }

            try
            {
                _context.Update(g);
                _context.SaveChanges();
            }
            catch (DBConcurrencyException e)
            {
                _logger.Error($"[SetPrefix] Failed to update {g.Name}'s prefix ({prefix})", e);
            }
        }

        public void SetPrefix(ulong guildId, string prefix)
        {
            var g = _context.Guilds.FirstOrDefault(x => x.GuildId == guildId);

            if (g != null)
            {
                g.Prefix = prefix;
            }
            else
            {
                g = new Guild
                {
                    GuildId = guildId,
                    Name = "",
                    Locale = "fr-FR",
                    Prefix = prefix
                };
            }

            try
            {
                _context.Update(g);
                _context.SaveChanges();
            }
            catch (DBConcurrencyException e)
            {
                _logger.Error($"[SetPrefix] Failed to update {g.GuildId}'s prefix ({prefix})", e);
            }
        }
    }
}