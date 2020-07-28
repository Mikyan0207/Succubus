using Succubus.Database.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Succubus.Database.Repositories
{
    public interface IImageRepository : IRepository<Image>
    {
        Task<Image> GetRandomImage();

        Image GetImageFromCosplayer(string name);
    }
}
