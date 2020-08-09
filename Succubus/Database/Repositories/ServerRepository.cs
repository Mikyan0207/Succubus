using Succubus.Database.Context;
using Succubus.Database.Models;
using Succubus.Database.Repositories.Interfaces;
using System;
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
                var e = await AnyAsync(x => x.ServerId == server.Id).ConfigureAwait(false);

                if (e)
                    return await FirstOrDefaultAsync(x => x.ServerId == server.Id).ConfigureAwait(false);

                var guild = await Context.AddAsync(new Server
                {
                    ServerId = server.Id,
                    Name = server.Name
                }).ConfigureAwait(false);

                await Context.SaveChangesAsync().ConfigureAwait(false);

                return guild.Entity;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}