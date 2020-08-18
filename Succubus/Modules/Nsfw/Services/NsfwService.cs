using Microsoft.EntityFrameworkCore;
using Succubus.Database.Models;
using Succubus.Modules.Nsfw.Options;
using Succubus.Services;
using Succubus.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Succubus.Exceptions;
using Succubus.Extensions;

namespace Succubus.Modules.Nsfw.Services
{
    public class NsfwService : IService
    {
        private ConfigurationService Configuration { get; }

        private DatabaseService DbService { get; }

        public string CloudUrl => Configuration.Configuration.CloudUrl;

        public NsfwService(ConfigurationService cs, DatabaseService ds)
        {
            Configuration = cs;
            DbService = ds;
        }

        public async Task<IEnumerable<Set>> GetSetsAsync()
        {
            return await DbService
                .GetContext()
                .Sets
                .AsQueryable()
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<Set> GetSetAsync(YabaiOptions options)
        {
            try
            {
                return await DbService
                    .GetContext()
                    .Sets
                    .Include(x => x.Cosplayer)
                    .AsNoTracking()
                    .AsQueryable()
                    .ConditionalWhere(options.User != null, x => x.Keywords.Any(y => y.Equals(options.User)))
                    .ConditionalWhere(options.Set != null, x => x.Keywords.Any(y => y.Equals(options.Set)))
                    .OrderBy(x => new Random().Next(1, 100))
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new SetException("", ex);
            }
        }

        public async Task<Set> GetSetAsync(string name)
        {
            try
            {
                return await DbService
                    .GetContext()
                    .Sets
                    .AsQueryable()
                    .FirstOrDefaultAsync(x => x.Keywords.Any(y => y.Equals(name)))
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new SetException("", ex);
            }
        }

        public async Task<IEnumerable<Cosplayer>> GetCosplayersAsync()
        {
            return await DbService
                .GetContext()
                .Cosplayers
                .AsQueryable()
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<Cosplayer> GetCosplayerAsync(string name)
        {
            try
            {
                return await DbService
                    .GetContext()
                    .Cosplayers
                    .AsQueryable()
                    .FirstOrDefaultAsync(x => x.Keywords.Any(y => y.Equals(name)))
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new CosplayerException("", ex);
            }
        }

        public async Task<(Set, bool)> CreateSetAliasAsync(string setName, string alias)
        {
            var set = await GetSetAsync(setName).ConfigureAwait(false);

            if (set == null)
                return (null, false);

            if (set.Keywords.Contains(alias))
                return (set, false);

            set.Keywords.Add(alias);

            try
            {
                await using var ctx = DbService.GetContext();

                ctx.Update(set);
                await ctx.SaveChangesAsync().ConfigureAwait(false);

                return (set, true);
            }
            catch (DbUpdateConcurrencyException)
            {
                return (set, false);
            }
        }

        public async Task<(Set, bool)> RemoveSetAliasAsync(string setName, string alias)
        {
            var set = await GetSetAsync(setName).ConfigureAwait(false);

            if (set == null)
                return (null, false);

            if (!set.Keywords.Contains(alias))
                return (set, false);

            set.Keywords.Remove(alias);

            try
            {
                await using var ctx = DbService.GetContext();

                ctx.Update(set);
                await ctx.SaveChangesAsync().ConfigureAwait(false);

                return (set, true);
            }
            catch (DbUpdateConcurrencyException)
            {
                return (set, false);
            }
        }

        public async Task<(Cosplayer, bool)> CreateCosplayerAliasAsync(string cosplayerName, string alias)
        {
            var cosplayer = await GetCosplayerAsync(cosplayerName).ConfigureAwait(false);

            if (cosplayer == null)
                return (null, false);

            if (cosplayer.Keywords.Contains(alias))
                return (cosplayer, false);

            cosplayer.Keywords.Add(alias);

            try
            {
                await using var ctx = DbService.GetContext();

                ctx.Update(cosplayer);
                await ctx.SaveChangesAsync().ConfigureAwait(false);

                return (cosplayer, true);
            }
            catch (DbUpdateConcurrencyException e)
            {
                return (cosplayer, false);
            }
        }

        public async Task<(Cosplayer, bool)> RemoveCosplayerAliasAsync(string cosplayerName, string alias)
        {
            var cosplayer = await GetCosplayerAsync(cosplayerName).ConfigureAwait(false);

            if (cosplayer == null)
                return (null, false);

            if (!cosplayer.Keywords.Contains(alias))
                return (cosplayer, false);

            cosplayer.Keywords.Remove(alias);

            try
            {
                await using var ctx = DbService.GetContext();

                ctx.Update(cosplayer);
                await ctx.SaveChangesAsync().ConfigureAwait(false);

                return (cosplayer, true);
            }
            catch (DbUpdateConcurrencyException e)
            {
                return (cosplayer, false);
            }
        }
    }
}