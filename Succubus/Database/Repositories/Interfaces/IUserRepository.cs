using Discord;
using Succubus.Database.Models;
using System.Threading.Tasks;

namespace Succubus.Database.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetOrCreate(Discord.IUser usr);
    }
}