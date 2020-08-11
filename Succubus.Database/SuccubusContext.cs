using Microsoft.EntityFrameworkCore;
using Succubus.Database.Models;

namespace Succubus.Database
{
    public class SuccubusContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Guild> Guilds { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("File=Succubus.db", options =>
            {
                options.CommandTimeout(60);
                options.MigrationsAssembly("Succubus.Database");
            });
        }
    }
}