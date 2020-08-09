using Succubus.Commands.Nsfw.Options;
using Succubus.Database.Models;
using System.Threading.Tasks;

namespace Succubus.Database.Repositories.Interfaces
{
    public interface ISetRepository : IRepository<Set>
    {
        Task<Set> GetSetAsync(YabaiOptions options);
    }
}