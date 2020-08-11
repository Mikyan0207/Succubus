using Discord;
using NLog;
using Succubus.Database.Models;
using System.Data;
using System.Linq;

namespace Succubus.Services
{
    public class PrefixService : IService
    {
        private DatabaseService DbService { get; set; }
        
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly string _defaultPrefix;

        public PrefixService(DatabaseService dbService, ConfigurationService configService)
        {
            DbService = dbService;
            _defaultPrefix = configService.Configuration.DefaultPrefix;
        }

        public string GetDefaultPrefix() => _defaultPrefix;

        public string GetPrefix(IGuild guild)
        {
            return GetPrefix(guild.Id);
        }

        public string GetPrefix(ulong guildId)
        {
            return DbService
                .GetDbContext()
                .Guilds
                .FirstOrDefault(x => x.GuildId == guildId)
                ?.Prefix ?? GetDefaultPrefix();
        }

        public string GetPrefix(string name)
        {
            return DbService
                .GetDbContext()
                .Guilds
                .FirstOrDefault(x => x.Name == name)
                ?.Prefix;
        }

        public void SetPrefix(IGuild guild, string prefix)
        {
            var g = DbService
                .GetDbContext()
                .Guilds
                .FirstOrDefault(x => x.GuildId == guild.Id);

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
                DbService.GetDbContext().Update(g);
                DbService.GetDbContext().SaveChanges();
            }
            catch (DBConcurrencyException e)
            {
                Logger.Error($"[SetPrefix] Failed to update {g.Name}'s prefix ({prefix})", e);
            }
        }

        public void SetPrefix(ulong guildId, string prefix)
        {
            var g = DbService
                .GetDbContext()
                .Guilds
                .FirstOrDefault(x => x.GuildId == guildId);

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
                DbService.GetDbContext().Update(g);
                DbService.GetDbContext().SaveChanges();
            }
            catch (DBConcurrencyException e)
            {
                Logger.Error($"[SetPrefix] Failed to update {g.GuildId}'s prefix ({prefix})", e);
            }
        }
    }
}