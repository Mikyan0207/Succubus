using Succubus.Database.Models;
using System.Threading.Tasks;

namespace Succubus.Database.Repositories.Interfaces
{
    public interface ICosplayerRepository : IRepository<Cosplayer>
    {
        Task<Cosplayer> GetCosplayerAsync(string name);
    }
}