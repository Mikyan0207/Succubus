using System.Threading.Tasks;
using Discord;
using Succubus.Database.Models;

namespace Succubus.Database.Repositories.Interfaces
{
    public interface IServerRepository : IRepository<Server>
    {
        Task<Server> GetOrCreate(IGuild server);
    }
}