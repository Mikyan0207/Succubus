using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using NLog;
using Succubus.Database.Context;
using Succubus.Database.Extensions;
using Succubus.Database.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Succubus.Commands.Nsfw.Options;
using Succubus.Database.Repositories.Interfaces;

namespace Succubus.Database.Repositories
{
    public class ImageRepository : Repository<Image>, IImageRepository
    {
        private static readonly NLog.Logger Logger = LogManager.GetCurrentClassLogger();

        public ImageRepository(SuccubusContext context) : base(context)
        { }

        public async Task<Image> GetImageAsync(YabaiOptions options)
        {
            try
            {
                if (options.FromCollection)
                {
                    return Context.Users
                        .Include(x => x.Collection)
                            .ThenInclude(y => y.Image)
                                .ThenInclude(z => z.Set)
                                .ThenInclude(z => z.Cosplayer)
                        .Select(x => x.Collection)
                        .ToList()
                        .Where(x => x.Any(y => options.SafeMode ? y.Image.Set.YabaiLevel == YabaiLevel.Safe : y.Image.Set.YabaiLevel >= YabaiLevel.Safe))
                        .ConditionalWhere(options.Set != null, x => x.Any(y => y.Image.Set.Name.ToLowerInvariant().LevenshteinDistance(options.Set.ToLowerInvariant()) < 2 || y.Image.Set.Aliases.ToLowerInvariant().LevenshteinDistance(options.Set.ToLowerInvariant()) < 2))
                        .ConditionalWhere(options.User != null, x => x.Any(y => y.Image.Cosplayer.Name.ToLowerInvariant().LevenshteinDistance(options.User) < 2 || y.Image.Cosplayer.Aliases.ToLowerInvariant().LevenshteinDistance(options.User) < 2))
                        .OrderBy(x => new Random().Next())
                        .Take(1)
                        .FirstOrDefault()
                        .Select(x => x.Image)
                        .FirstOrDefault();
                }

                return Context.Images
                    .Include(x => x.Set)
                    .Include(x => x.Cosplayer)
                    .ToList()
                    .Where(x => options.SafeMode ? x.Set.YabaiLevel == YabaiLevel.Safe : x.Set.YabaiLevel >= YabaiLevel.Safe)
                    .ConditionalWhere(options.Set != null, x => x.Set.Name.ToLowerInvariant().LevenshteinDistance(options.Set.ToLowerInvariant()) < 2 || x.Set.Aliases.ToLowerInvariant().LevenshteinDistance(options.Set.ToLowerInvariant()) < 2)
                    .ConditionalWhere(options.User != null, x => x.Cosplayer.Name.ToLowerInvariant().LevenshteinDistance(options.User) < 2 || x.Cosplayer.Aliases.ToLowerInvariant().LevenshteinDistance(options.User) < 2)
                    .OrderBy(x => new Random().Next())
                    .Take(1)
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logger.Warn($"Error during SQL Request. {ex.Message}");

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
                    .Select(x => (x, Distance: x.Name.ToLowerInvariant().LevenshteinDistance(name), AliasDistance: x.Aliases.ToLowerInvariant().LevenshteinDistance(name)))
                    .Where(x => x.Distance < 3 || x.AliasDistance < 3)
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
                   .Select(y => (y, Distance: y.Name.ToLowerInvariant().LevenshteinDistance(setName), AliasDistance: y.Aliases.ToLowerInvariant().LevenshteinDistance(setName)))
                   .OrderBy(y => y.Distance)
                   .Where(y => y.Distance < 3 || y.AliasDistance < 3)
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