﻿using Succubus.Database.Models;
using System.Threading.Tasks;

namespace Succubus.Database.Repositories
{
    public interface IServerRepository : IRepository<Server>
    {
        Task<Server> GetOrCreate(Discord.IGuild server);
    }
}