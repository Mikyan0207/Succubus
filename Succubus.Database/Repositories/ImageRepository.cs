using Microsoft.EntityFrameworkCore;
using Succubus.Database.Context;
using Succubus.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace Succubus.Database.Repositories
{
    public class ImageRepository : Repository<Image>, IImageRepository
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public ImageRepository(SuccubusContext context) : base(context)
        { }

        public async Task<Image> GetRandomImage()
        {
            int nb = new Random().Next(0, Context.Images.Count());

            try
            {
                Image img = Context.Images
                    .Include(x => x.Set)
                    .Include(x => x.Cosplayer)
                    .ToList()[nb];

                if (img == null)
                    Logger.Warn($"Failed to get Image from Database. Image N°{nb+1}");

                return img;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);

                return null;
            }
        }

        public Image GetImageFromCosplayer(string name)
        {
            try
            {
                var cosplayer = Context.Cosplayers
                    .Include(x => x.Sets)
                    .Where(x => x.Name == name || x.Aliases.Contains(name))
                    .FirstOrDefault();

                if (cosplayer == null)
                {
                    Logger.Warn("No Cosplayer found.");
                    return null;
                }

                var sets = Context.Sets
                    .Include(x => x.Cosplayer)
                    .Include(x => x.Images)
                    .Where(x => x.Cosplayer.Id == cosplayer.Id)
                    .ToList();

                if (!sets.Any())
                {
                    Logger.Error("No Set found.");
                    return null;
                }

                var selectedSet = sets[new Random().Next(0, sets.Count)];

                return selectedSet.Images[new Random().Next(0, (int)selectedSet.Size)];
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                return null;
            }



        }
    }
}
