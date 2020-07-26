using Microsoft.EntityFrameworkCore;
using Succubus.Database.Context;
using Succubus.Database.Models;
using System;
using System.Collections.Generic;
using System.Text;
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
                var e = await AnyAsync(x => x.UserId == usr.Id).ConfigureAwait(false);

                if (e)
                    return await FirstOrDefaultAsync(x => x.UserId == usr.Id).ConfigureAwait(false);

                var user = await Context.AddAsync(new User
                {
                    Username = usr.Username,
                    Discriminator = usr.Discriminator,
                    UserId = usr.Id,
                    Level = 0,
                    Experience = 0
                }).ConfigureAwait(false);

                await Context.SaveChangesAsync().ConfigureAwait(false);

                return user.Entity;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
