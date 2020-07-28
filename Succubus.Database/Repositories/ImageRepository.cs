using Microsoft.EntityFrameworkCore;
using Succubus.Database.Context;
using Succubus.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Succubus.Database.Repositories
{
    public class ImageRepository : Repository<Image>, IImageRepository
    {
        public ImageRepository(SuccubusContext context) : base(context)
        { }

        public async Task<Image> GetRandomImage()
        {
            int nb = new Random().Next(0, Context.Images.Count());

            return await Context.Images
                .Include(x => x.Set)
                .Include(x => x.Cosplayer)
                .FirstOrDefaultAsync(x => x.Number == nb + 1);
        }

        public Image GetImageFromCosplayer(string name)
        {
            var cosplayer = Context.Cosplayers
                .Include(x => x.Sets)
                .Where(x => x.Name == name || x.Aliases.Contains(name))
                .FirstOrDefault();

            if (cosplayer == null)
                return null;

            var sets = Context.Sets
                .Include(x => x.Cosplayer)
                .Include(x => x.Images)
                .Where(x => x.Cosplayer.Id == cosplayer.Id)
                .ToList();

            if (!sets.Any())
                return null;

            var selectedSet = sets[new Random().Next(0, sets.Count)];

            return selectedSet.Images[new Random().Next(0, (int)selectedSet.Size)];
        }
    }
}
