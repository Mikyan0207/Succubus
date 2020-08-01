using Succubus.Database.Context;
using Succubus.Database.Repositories;
using System;
using System.Threading.Tasks;
using Succubus.Database.Repositories.Interfaces;

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

        private IImageRepository _imageRepository;
        public IImageRepository Images => _imageRepository ??= new ImageRepository(Context);

        private ICosplayerRepository _cosplayerRepository;
        public ICosplayerRepository Cosplayers => _cosplayerRepository ??= new CosplayerRepository(Context);

        private IColorRepository _colorRepository;
        public IColorRepository Colors => _colorRepository ??= new ColorRepository(Context);

        private IYoutubeChannelRepository _youtubeChannelRepository;
        public IYoutubeChannelRepository YoutubeChannels => _youtubeChannelRepository ??= new YoutubeChannelRepository(Context);

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