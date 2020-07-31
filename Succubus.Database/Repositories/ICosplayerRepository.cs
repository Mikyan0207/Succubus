using Succubus.Database.Models;

namespace Succubus.Database.Repositories
{
    public interface ICosplayerRepository : IRepository<Cosplayer>
    {
        Cosplayer GetCosplayerByName(string name);
    }
}