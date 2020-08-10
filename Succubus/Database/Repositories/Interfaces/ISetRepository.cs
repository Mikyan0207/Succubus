using System.Threading.Tasks;
using Succubus.Commands.Nsfw.Options;
using Succubus.Database.Models;

namespace Succubus.Database.Repositories.Interfaces
{
    public interface ISetRepository : IRepository<Set>
    {
        Task<Set> GetSetAsync(YabaiOptions options);

        Task<Set> GetSetAsync(string name);
    }
}