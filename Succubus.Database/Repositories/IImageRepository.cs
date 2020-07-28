using Succubus.Database.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Succubus.Database.Repositories
{
    public interface IImageRepository : IRepository<Image>
    {
        Image GetRandomImage();

        Image GetImageFromCosplayer(string name);

        Image GetImageFromSet(string set);
    }
}
