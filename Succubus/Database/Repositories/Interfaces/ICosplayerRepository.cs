using System.Threading.Tasks;
using Succubus.Database.Models;

namespace Succubus.Database.Repositories.Interfaces
{
    public interface ICosplayerRepository : IRepository<Cosplayer>
    {
        Task<Cosplayer> GetCosplayerAsync(string name);
    }
}