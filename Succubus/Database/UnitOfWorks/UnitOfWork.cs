using Succubus.Database.Context;
using Succubus.Database.Repositories;
using Succubus.Database.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace Succubus.Database.UnitOfWorks
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        public SuccubusContext Context { get; }

        public UnitOfWork(SuccubusContext context)
        {
            Context = context;
        }

        #region Repositories

        private IUserRepository _userRepository;
        public IUserRepository Users => _userRepository ??= new UserRepository(Context);

        private IServerRepository _serverRepository;
        public IServerRepository Servers => _serverRepository ??= new ServerRepository(Context);

        private ISetRepository _imageRepository;
        public ISetRepository Sets => _imageRepository ??= new SetRepository(Context);

        private ICosplayerRepository _cosplayerRepository;
        public ICosplayerRepository Cosplayers => _cosplayerRepository ??= new CosplayerRepository(Context);

        private IColorRepository _colorRepository;
        public IColorRepository Colors => _colorRepository ??= new ColorRepository(Context);

        #endregion Repositories

        public int SaveChanges() => Context.SaveChanges();

        public async Task<int> SaveChangesAsync() => await Context.SaveChangesAsync().ConfigureAwait(false);

        public void Dispose()
        {
            Context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}