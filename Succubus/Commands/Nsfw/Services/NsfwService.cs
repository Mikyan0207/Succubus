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

        public Cosplayer GetCosplayer(string name)
        {
            using (var uow = _db.GetDbContext())
            {
                return uow.Cosplayers.GetCosplayerByName(name.Trim());
            }
        }

        public Image GetRandomImage()
        {
            using (var uow = _db.GetDbContext())
            {
                return uow.Images.GetRandomImage();
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
