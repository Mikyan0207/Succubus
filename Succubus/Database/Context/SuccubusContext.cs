using Microsoft.EntityFrameworkCore;
using Mikyan.Framework.Stores;
using Succubus.Database.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Succubus.Database.JsonModels;

namespace Succubus.Database.Context
{
    public class SuccubusContext : DbContext
    {
        private static string _cloudUrl;

        public DbSet<Color> Colors { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Server> Servers { get; set; }

        public DbSet<Cosplayer> Cosplayers { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<Set> Sets { get; set; }

        public DbSet<UserImage> UserImages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            using var configurationStore = new NamedResourceStore<byte[]>(new DllResourceStore(new AssemblyName("Succubus.Resources")), @"Configuration");
            configurationStore.AddExtension(".json");

            var connectionString = Utf8Json.JsonSerializer.Deserialize<DbConfiguration>(configurationStore.Get("Database")).ConnectionString;
            _cloudUrl = Utf8Json.JsonSerializer.Deserialize<ApiConfiguration>(configurationStore.Get("Cloud")).ApiUrl;

            optionsBuilder.UseSqlite($@"Data Source={connectionString}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Set>()
                .HasOne(x => x.Cosplayer)
                .WithMany(x => x.Sets);

            modelBuilder.Entity<Set>()
                .Property(e => e.Aliases)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<List<string>>(v));

            modelBuilder.Entity<UserImage>()
                .HasOne(x => x.Image)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.ImageId);

            modelBuilder.Entity<UserImage>()
                .HasOne(x => x.User)
                .WithMany(x => x.Collection)
                .HasForeignKey(x => x.UserId);
        }

        public async void Initialize()
        {
            #region Cosplayers

            {
                using var store = new NamedResourceStore<byte[]>(new DllResourceStore(new AssemblyName("Succubus.Resources")), @"Yabai");
                store.AddExtension(".json");

                var cosplayers = store.GetResources().Select(x => Path.GetFileNameWithoutExtension(x) ?? "").ToList();

                foreach (var cp in cosplayers.Select(cosplayer => Utf8Json.JsonSerializer.Deserialize<CosplayerData>(store.Get(cosplayer))))
                {
                    if (!await Cosplayers.AsQueryable().AnyAsync(x => x.Name == cp.Name).ConfigureAwait(false))
                    {
                        await Cosplayers.AddAsync(new Cosplayer
                        {
                            Name = cp.Name,
                            Aliases = cp.Aliases,
                            Twitter = cp.Twitter,
                            Instagram = cp.Instagram,
                            Booth = cp.Booth,
                            ProfilePicture = $"{_cloudUrl}{cp.ProfilePicture}"
                        }).ConfigureAwait(false);

                        await SaveChangesAsync().ConfigureAwait(false);
                    }

                    foreach (var set in cp.Sets)
                    {
                        if (await Sets.AsQueryable().AnyAsync(x => x.Name == set.Name).ConfigureAwait(false))
                            continue;

                        await Sets.AddAsync(new Set
                        {
                            Name = set.Name,
                            Aliases = set.Aliases,
                            Cosplayer = Cosplayers.FirstOrDefault(y => y.Name == cp.Name),
                            Size = (uint) set.Size,
                            FolderName = set.FolderName,
                            FilePrefix = set.FilePrefix,
                            SetPreview = $@"{_cloudUrl}{cp.Aliases}/{set.FolderName}/{set.FilePrefix ?? set.FolderName}_001.jpg",
                            YabaiLevel = (YabaiLevel) set.YabaiLevel

                        }).ConfigureAwait(false);

                        await SaveChangesAsync().ConfigureAwait(false);
                    }
                }
            }

            #endregion Cosplayers
        }
    }

    public class DbConfiguration
    {
        public string ConnectionString { get; set; }
    }

    public class ApiConfiguration
    {
        public string ApiUrl { get; set; }
    }
}