using Discord;
using Succubus.Database.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Succubus.Database.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetOrCreate(Discord.IUser usr);

        Task<bool> AddImageToCollectionAsync(IUser discordUser, string setName, int number);

        Task<bool> RemoveImageFromCollectionAsync(IUser discordUser, string setName, int number);
    }
}
