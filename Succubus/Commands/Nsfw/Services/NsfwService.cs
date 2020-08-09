using Discord;
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

        public NsfwService(DbService db)
        {
            DbService = db;
        }

        public Cosplayer GetCosplayer(string name)
        {
            using (var uow = DbService.GetDbContext())
            {
                return uow.Cosplayers.GetCosplayerByName(name.Trim());
            }
        }

        public async Task<Set> GetSetAsync(YabaiOptions options)
        {
            using var uow = DbService.GetDbContext();
            return await uow.Sets.GetSetAsync(options).ConfigureAwait(false);
        }

        public async Task<bool> AddImageToCollectionAsync(IUser discordUser, string setName, int number)
        {
            using (var uow = DbService.GetDbContext())
            {
                return await uow.Users.AddImageToCollectionAsync(discordUser, setName, number).ConfigureAwait(false);
            }
        }

        public async Task<bool> RemoveImageFromCollectionAsync(IUser discordUser, string setName, int number)
        {
            using (var uow = DbService.GetDbContext())
            {
                return await uow.Users.RemoveImageFromCollectionAsync(discordUser, setName, number)
                    .ConfigureAwait(false);
            }
        }
    }
}