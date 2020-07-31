using Discord;
using Succubus.Database.Context;
using Succubus.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
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

        private IUserRepository userRepository;
        public IUserRepository Users => userRepository ??= new UserRepository(Context);

        private IServerRepository serverRepository;
        public IServerRepository Servers => serverRepository ??= new ServerRepository(Context);

        private IImageRepository imageRepository;
        public IImageRepository Images => imageRepository ??= new ImageRepository(Context);

        private ICosplayerRepository cosplayerRepository;
        public ICosplayerRepository Cosplayers => cosplayerRepository ??= new CosplayerRepository(Context);

        private IColorRepository colorRepository;
        public IColorRepository Colors => colorRepository ??= new ColorRepository(Context);

        #endregion

        public int SaveChanges() => Context.SaveChanges();

        public async Task<int> SaveChangesAsync() => await Context.SaveChangesAsync().ConfigureAwait(false);

        public void Dispose()
        {
            Context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
