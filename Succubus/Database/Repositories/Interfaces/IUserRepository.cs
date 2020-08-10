using System.Threading.Tasks;
using Discord;
using Succubus.Database.Models;

namespace Succubus.Database.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetOrCreate(IUser usr);
    }
}