using Succubus.Database.Models;
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

        public async Task<Image> GetRandomImageAsync()
        {
            using (var uow = _db.GetDbContext())
            {
                return await uow.Images.GetRandomImage();
            }
        }

        public Image GetRandomImageFromCosplayer(string name)
        {
            using (var uow = _db.GetDbContext())
            {
                return uow.Images.GetImageFromCosplayer(name);
            }
        }
    }

}
