using System.Data;
using System.Threading.Tasks;
using Discord;
using Mikyan.Framework.Services;
using Succubus.Services;

namespace Succubus.Commands.Locale.Services
{
    public class LocaleService : IService
    {
        private readonly DbService _dbService;

        public LocaleService(DbService dbService)
        {
            _dbService = dbService;
        }

        public async Task<string> GetServerLocale(IGuild server)
        {
            using var uow = _dbService.GetDbContext();
            var serv = await uow.Servers.GetOrCreate(server).ConfigureAwait(false);

            return serv.Locale;
        }

        public async Task<bool> SetServerLocale(IGuild server, string locale)
        {
            using var uow = _dbService.GetDbContext();
            var serv = await uow.Servers.GetOrCreate(server).ConfigureAwait(false);

            serv.Locale = locale;

            try
            {
                await uow.SaveChangesAsync().ConfigureAwait(false);
                return true;
            }
            catch (DBConcurrencyException)
            {
                return false;
            }
        }
    }
}