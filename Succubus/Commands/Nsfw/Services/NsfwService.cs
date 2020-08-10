using Mikyan.Framework.Services;
using Succubus.Commands.Nsfw.Options;
using Succubus.Database.Models;
using Succubus.Services;
using System.Threading.Tasks;

namespace Succubus.Commands.Nsfw.Services
{
    public class NsfwService : IService
    {
        private DbService DbService { get; }

        public BotService BotService { get; }

        public NsfwService(DbService db, BotService botService)
        {
            DbService = db;
            BotService = botService;
        }

        public async Task<Cosplayer> GetCosplayerAsync(string name)
        {
            using var uow = DbService.GetDbContext();
            return await uow.Cosplayers.GetCosplayerAsync(name).ConfigureAwait(false);
        }

        public async Task<Set> GetSetAsync(YabaiOptions options)
        {
            using var uow = DbService.GetDbContext();
            return await uow.Sets.GetSetAsync(options).ConfigureAwait(false);
        }

        public async Task<Set> GetSetAsync(string name)
        {
            using var uow = DbService.GetDbContext();
            return await uow.Sets.GetSetAsync(name).ConfigureAwait(false);
        }
    }
}