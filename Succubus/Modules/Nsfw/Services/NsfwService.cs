using Microsoft.EntityFrameworkCore;
using Succubus.Database.Models;
using Succubus.Modules.Nsfw.Options;
using Succubus.Services;
using Succubus.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<Set> GetSetAsync(YabaiOptions options)
        {
            try
            {
                return await DbService
                    .GetContext()
                    .Sets
                    .Include(x => x.Cosplayer)
                    .AsAsyncEnumerable()
                    .ConditionalWhere(options.User != null, x => x.Keywords.Any(y => y.Equals(options.User)))
                    .ConditionalWhere(options.Set != null, x => x.Keywords.Any(y => y.Equals(options.Set)))
                    .OrderBy(x => new Random().Next(1, 100))
                    .Take(1)
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}