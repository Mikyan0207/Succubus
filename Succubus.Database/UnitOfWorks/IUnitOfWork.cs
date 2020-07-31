﻿using Succubus.Database.Context;
using Succubus.Database.Repositories;
using System;
using System.Threading.Tasks;

namespace Succubus.Database.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {
        SuccubusContext Context { get; }

        public IUserRepository Users { get; }
        public IServerRepository Servers { get; }
        public IImageRepository Images { get; }
        public ICosplayerRepository Cosplayers { get; }

        int SaveChanges();

        Task<int> SaveChangesAsync();
    }
}