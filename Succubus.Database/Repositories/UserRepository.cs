using Discord;
using Microsoft.EntityFrameworkCore;
using NLog;
using Succubus.Database.Context;
using Succubus.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Succubus.Database.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private static readonly Logger _Logger = LogManager.GetCurrentClassLogger();

        public UserRepository(SuccubusContext context) : base(context)
        {
        }

        public async Task<User> GetOrCreate(Discord.IUser usr)
        {
            try
            {
                var e = await AnyAsync(x => x.UserId == usr.Id).ConfigureAwait(false);

                if (e)
                    return await Context.Users
                        .Include(x => x.Collection)
                        .FirstOrDefaultAsync(x => x.UserId == usr.Id)
                        .ConfigureAwait(false);

                var user = await Context.AddAsync(new User
                {
                    Username = usr.Username,
                    Discriminator = usr.Discriminator,
                    UserId = usr.Id,
                    Level = 0,
                    Experience = 0,
                    Collection = new List<UserImage>()
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

        public async Task<bool> AddImageToCollectionAsync(IUser discordUser, string setName, int number)
        {
            var user = await GetOrCreate(discordUser).ConfigureAwait(false);
            var image = await Context.Images
                .Include(x => x.Set)
                .FirstOrDefaultAsync(x => x.Set.Name == setName && x.Number == number)
                .ConfigureAwait(false);

            if (image == null)
                return false;

            if (Context.UserImages.Any(x => x.UserId == user.Id && x.ImageId == image.Id))
                return false;

            Context.Add(new UserImage
            {
                User = user,
                Image = image,
            });

            try
            {
                Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _Logger.Warn(ex.Message);
                return false;
            }

            return true;
        }

        public async Task<bool> RemoveImageFromCollectionAsync(IUser discordUser, string setName, int number)
        {
            var user = await GetOrCreate(discordUser).ConfigureAwait(false);
            var image = await Context.Images
                .Include(x => x.Set)
                .FirstOrDefaultAsync(x => x.Set.Name == setName && x.Number == number)
                .ConfigureAwait(false);

            if (image == null)
                return false;

            if (!Context.UserImages.Any(x => x.UserId == user.Id && x.ImageId == image.Id))
                return false;

            Context.UserImages.Remove(Context.UserImages.FirstOrDefault(x => x.UserId == user.Id && x.ImageId == image.Id));

            try
            {
                Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _Logger.Warn(ex.Message);
                return false;
            }

            return true;
        }
    }
}
