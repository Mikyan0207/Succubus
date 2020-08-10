using Succubus.Database.Context;
using Succubus.Database.Models;
using Succubus.Database.Repositories.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Succubus.Database.Repositories
{
    public class ServerRepository : Repository<Server>, IServerRepository
    {
        public ServerRepository(SuccubusContext context) : base(context)
        {
        }

        public async Task<Server> GetOrCreate(Discord.IGuild server)
        {
            try
            {
                if (await Context.Servers.AnyAsync(x => x.ServerId == server.Id).ConfigureAwait(false))
                    return await Context.Servers.FirstOrDefaultAsync(x => x.ServerId == server.Id).ConfigureAwait(false);

                var guild = await Context.AddAsync(new Server
                {
                    ServerId = server.Id,
                    Name = server.Name,
                    Locale = "fr-FR"
                }).ConfigureAwait(false);

                await Context.SaveChangesAsync().ConfigureAwait(false);

                return guild.Entity;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}