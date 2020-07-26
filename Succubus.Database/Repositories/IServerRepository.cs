using Succubus.Database.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Succubus.Database.Repositories
{
    public interface IServerRepository : IRepository<Server>
    {
        Task<Server> GetOrCreate(Discord.IGuild server);
    }
}
