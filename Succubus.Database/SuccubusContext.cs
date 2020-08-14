using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Succubus.Database.Models;

namespace Succubus.Database
{
    public class SuccubusContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Guild> Guilds { get; set; }

        public DbSet<Cosplayer> Cosplayers { get; set; }

        public DbSet<Set> Sets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=Succubus.db", options =>
            {
                options
                    .CommandTimeout(60)
                    .MigrationsAssembly("Succubus.Database");
            });
        }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<Cosplayer>()
                .Property(p => p.Keywords)
                .HasConversion(
                    c => JsonSerializer.Serialize(c, null),
                    c => JsonSerializer.Deserialize<List<string>>(c, null));

            mb.Entity<Cosplayer>()
                .HasMany(c => c.Socials)
                .WithOne(s => s.Cosplayer);

            mb.Entity<Set>()
                .HasOne(s => s.Cosplayer)
                .WithMany(c => c.Sets);

            mb.Entity<Set>()
                .Property(p => p.Keywords)
                .HasConversion(
                    c => JsonSerializer.Serialize(c, null),
                    c => JsonSerializer.Deserialize<List<string>>(c, null));
        }
    }
}