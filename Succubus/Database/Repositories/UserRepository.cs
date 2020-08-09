using Succubus.Database.Context;
using Succubus.Database.Models;
using Succubus.Database.Repositories.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Succubus.Database.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(SuccubusContext context) : base(context)
        {
        }

        public async Task<User> GetOrCreate(Discord.IUser usr)
        {
            try
            {
                if (await Context.Users.AnyAsync(x => x.UserId == usr.Id).ConfigureAwait(false))
                    return await Context.Users.FirstOrDefaultAsync(x => x.UserId == usr.Id).ConfigureAwait(false);

                var user = await Context.AddAsync(new User
                {
                    Username = usr.Username,
                    Discriminator = usr.Discriminator,
                    UserId = usr.Id,
                    Level = 0,
                    Experience = 0,
                }).ConfigureAwait(false);

                await Context.SaveChangesAsync().ConfigureAwait(false);

                return user.Entity;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}