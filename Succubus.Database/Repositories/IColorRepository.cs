using Succubus.Database.Models;

namespace Succubus.Database.Repositories
{
    public interface IColorRepository : IRepository<Color>
    {
        Discord.Color GetDiscordColor(string name);
    }
}