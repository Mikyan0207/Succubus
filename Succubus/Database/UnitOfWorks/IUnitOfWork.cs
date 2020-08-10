using System;
using System.Threading.Tasks;
using Succubus.Database.Context;
using Succubus.Database.Repositories.Interfaces;

namespace Succubus.Database.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {
        SuccubusContext Context { get; }

        public IUserRepository Users { get; }
        public IServerRepository Servers { get; }
        public ISetRepository Sets { get; }
        public ICosplayerRepository Cosplayers { get; }
        public IColorRepository Colors { get; }

        int SaveChanges();

        Task<int> SaveChangesAsync();
    }
}