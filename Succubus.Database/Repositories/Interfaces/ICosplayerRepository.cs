using Succubus.Database.Models;

namespace Succubus.Database.Repositories.Interfaces
{
    public interface ICosplayerRepository : IRepository<Cosplayer>
    {
        Cosplayer GetCosplayerByName(string name);
    }
}