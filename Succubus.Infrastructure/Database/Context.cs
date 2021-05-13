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
        private readonly ICurrentUserService CurrentUserService;

        public Context(DbContextOptions<Context> options, ICurrentUserService currentUserService) : base(options)
        {
            CurrentUserService = currentUserService;

            Cosplayers = Set<Cosplayer>();
            Images = Set<Image>();
            Sets = Set<Set>();
            Socials = Set<Social>();
            Users = Set<User>();
        }

        public DbSet<Cosplayer> Cosplayers { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<Set> Sets { get; set; }

        public DbSet<Social> Socials { get; set; }

        public DbSet<User> Users { get; set; }

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
