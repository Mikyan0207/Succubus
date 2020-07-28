using Microsoft.EntityFrameworkCore;
using Succubus.Database.Context;
using Succubus.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using Microsoft.EntityFrameworkCore.Internal;
using Succubus.Database.Extensions;
using System.Security.Cryptography.X509Certificates;

namespace Succubus.Database.Repositories
{
    public class ImageRepository : Repository<Image>, IImageRepository
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public ImageRepository(SuccubusContext context) : base(context)
        { }

        public Image GetRandomImage()
        {
            int nb = new Random().Next(0, Context.Images.Count());

            try
            {
                Image img = Context.Images
                    .Include(x => x.Set)
                    .Include(x => x.Cosplayer)
                    .ToList()
                    .OrderBy(x => new Random().Next())
                    .Take(1).First();

                if (img == null)
                    Logger.Warn($"Failed to get Image from Database. Image N°{nb + 1}");
                else
                    Logger.Info($"Image: {img.Name} #{img.Number} | {img.Set.Name}");

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
                    .AsEnumerable()
                    .Select(x => (x, Distance: x.Name.ToLowerInvariant().LevenshteinDistance(name)))
                    .Where(x => x.Distance < 3 || x.x.Aliases.Split(',').Any(y => y.ToLowerInvariant().LevenshteinDistance(name) < 3))
                    .FirstOrDefault().x;

                if (cosplayer == null)
                {
                    Logger.Warn("No Cosplayer found. " + name);
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

                var selectedSet = sets[new Random().Next(0, sets.Count - 1)];

                return selectedSet.Images[new Random().Next(0, (int)selectedSet.Size - 1)];
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                return null;
            }
        }

        public Image GetImageFromSet(string setName)
        {
            try
            {
                var set = Context.Sets
                   .Include(x => x.Cosplayer)
                   .Include(x => x.Images)
                   .AsEnumerable()
                   .Select(y => (y, Distance: y.Name.ToLowerInvariant().LevenshteinDistance(setName)))
                   .OrderBy(y => y.Distance)
                   .Where(y => y.Distance < 3 || y.y.Aliases.Split(',').Any(x => x.ToLowerInvariant().LevenshteinDistance(setName) < 3))
                   .FirstOrDefault().y;

                if (set == null)
                {
                    Logger.Error("No Set found.");
                    return null;
                }

                return set.Images[new Random().Next(0, (int)set.Size - 1)];
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                return null;
            }
        }
    }
}
