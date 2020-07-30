using Succubus.Database.Models;
using Succubus.Database.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Succubus.Database.Repositories
{
    public interface IImageRepository : IRepository<Image>
    {
        Task<Image> GetImageAsync(YabaiOptions options);

        Image GetImageFromCosplayer(string name);

        Image GetImageFromSet(string set);
    }
}
