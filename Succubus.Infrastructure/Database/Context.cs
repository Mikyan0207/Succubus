using Microsoft.EntityFrameworkCore;
using Succubus.Application.Interfaces;
using Succubus.Core.Common;
using Succubus.Core.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Succubus.Infrastructure.Database
{
    public class Context : DbContext, IContext
    {
        private ICurrentUserService CurrentUserService { get; }

        public Context(DbContextOptions<Context> options, ICurrentUserService currentUserService) : base(options)
        {
            CurrentUserService = currentUserService;
        }

        public DbSet<Cosplayer> Cosplayers => Set<Cosplayer>();

        public DbSet<Set> Sets => Set<Set>();

        public DbSet<Social> Socials => Set<Social>();

        public DbSet<User> Users => Set<User>();

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = CurrentUserService.UserId;
                        entry.Entity.Created = DateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedBy = CurrentUserService.UserId;
                        entry.Entity.Updated = DateTime.Now;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
