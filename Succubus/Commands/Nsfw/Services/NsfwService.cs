using Succubus.Database.Models;
using Succubus.Database.Options;
using Succubus.Services;
using System.Threading.Tasks;

namespace Succubus.Commands.Nsfw.Services
{
    public class NsfwService : IService
    {
        private readonly DbService _db;

        public NsfwService(DbService db)
        {
            _db = db;
        }

        public Cosplayer GetCosplayer(string name)
        {
            using (var uow = _db.GetDbContext())
            {
                return uow.Cosplayers.GetCosplayerByName(name.Trim());
            }
        }

        public async Task<Image> GetImageAsync(YabaiOptions options)
        {
            using (var uow = _db.GetDbContext())
            {
                return await uow.Images.GetImageAsync(options).ConfigureAwait(false);
            }
        }

        public Image GetRandomImageFromCosplayer(string name)
        {
            using (var uow = _db.GetDbContext())
            {
                return uow.Images.GetImageFromCosplayer(name.Trim());
            }
        }

        public Image GetRandomImageFromSet(string set)
        {
            using (var uow = _db.GetDbContext())
            {
                return uow.Images.GetImageFromSet(set.Trim());
            }
        }
    }

}
