using Succubus.Database.Models;
using Succubus.Database.Options;
using System.Threading.Tasks;

namespace Succubus.Database.Repositories.Interfaces
{
    public interface IImageRepository : IRepository<Image>
    {
        Task<Image> GetImageAsync(YabaiOptions options);

        Image GetImageFromCosplayer(string name);

        Image GetImageFromSet(string set);
    }
}