using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Succubus.Core.Entities;

namespace Succubus.Application.Interfaces
{
    public interface IContext
    {
        DbSet<Cosplayer> Cosplayers { get; }

        DbSet<Set> Sets { get; }

        DbSet<Social> Socials { get; }

        DbSet<User> Users { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}