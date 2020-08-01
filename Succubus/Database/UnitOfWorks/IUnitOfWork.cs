using Succubus.Database.Context;
using Succubus.Database.Repositories;
using System;
using System.Threading.Tasks;
using Succubus.Database.Repositories.Interfaces;

namespace Succubus.Database.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {
        SuccubusContext Context { get; }

        public IUserRepository Users { get; }
        public IServerRepository Servers { get; }
        public IImageRepository Images { get; }
        public ICosplayerRepository Cosplayers { get; }
        public IYoutubeChannelRepository YoutubeChannels { get; }
        public IColorRepository Colors { get; }

        int SaveChanges();

        Task<int> SaveChangesAsync();
    }
}