using Microsoft.EntityFrameworkCore;
using Succubus.Core.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Succubus.Application.Interfaces
{
    public interface IContext
    {
        DbSet<Cosplayer> Cosplayers { get; set; }

        DbSet<Image> Images { get; set; }

        DbSet<Set> Sets { get; set; }

        DbSet<Social> Socials { get; set; }

        DbSet<User> Users { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}