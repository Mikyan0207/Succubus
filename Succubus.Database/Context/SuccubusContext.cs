using Microsoft.EntityFrameworkCore;
using Succubus.Database.Models;
using Succubus.Store;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Succubus.Database.Context
{
    public class SuccubusContext : DbContext
    {
        private NamedResourceStore<byte[]> ConfigurationStore;

        private static string ConnectionString;
        private static string CloudUrl;

        public SuccubusContext()
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Server> Servers { get; set; }
        public DbSet<Cosplayer> Cosplayers { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Set> Sets { get; set; }
        public DbSet<UserImage> UserImages { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            ConfigurationStore = new NamedResourceStore<byte[]>(new DllResourceStore(new AssemblyName("Succubus.Resources")), @"Configuration");
            ConfigurationStore.AddExtension(".json");

            ConnectionString = Utf8Json.JsonSerializer.Deserialize<DbConfiguration>(ConfigurationStore.Get("Database")).ConnectionString;
            CloudUrl = Utf8Json.JsonSerializer.Deserialize<ApiConfiguration>(ConfigurationStore.Get("Cloud")).ApiUrl;

            optionsBuilder.UseSqlite($@"Data Source={ConnectionString}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Image>()
                .HasOne(x => x.Set)
                .WithMany(x => x.Images);

            modelBuilder.Entity<Set>()
                .HasOne(x => x.Cosplayer)
                .WithMany(x => x.Sets);

            modelBuilder.Entity<UserImage>()
                .HasOne(x => x.Image)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.ImageId);

            modelBuilder.Entity<UserImage>()
                .HasOne(x => x.User)
                .WithMany(x => x.Collection)
                .HasForeignKey(x => x.UserId);
        }

        public void Initiliaze()
        {
            using var store = new NamedResourceStore<byte[]>(new DllResourceStore(new AssemblyName("Succubus.Resources")), @"Yabai");
            store.AddExtension(".json");

            List<string> cosplayers = store.GetResources().Select(x => Path.GetFileNameWithoutExtension(x)).ToList();

            foreach (string cosplayer in cosplayers)
            {
                CosplayerData cp = Utf8Json.JsonSerializer.Deserialize<CosplayerData>(store.Get(cosplayer));

                if (!Cosplayers.Any(x => x.Name == cp.Name))
                {
                    Cosplayers.Add(new Cosplayer
                    {
                        Name = cp.Name,
                        Aliases = cp.Aliases,
                        Twitter = cp.Twitter,
                        Instagram = cp.Instagram,
                        Booth = cp.Booth,
                        ProfilePicture = $"{CloudUrl}{cp.ProfilePicture}"
                    });

                    SaveChanges();
                }

                foreach (SetData set in cp.Sets)
                {
                    if (Sets.Any(x => x.Name == set.Name))
                        continue;

                    Sets.Add(new Set
                    {
                        Name = set.Name,
                        Aliases = set.Aliases,
                        Cosplayer = Cosplayers.FirstOrDefault(y => y.Name == cp.Name),
                        Size = (uint)set.Size,
                        SetPreview = $@"{CloudUrl}{cp.Aliases}/{set.FolderName}/{set.FolderName}_001.jpg",
                        YabaiLevel = (YabaiLevel)set.YabaiLevel
                    });

                    SaveChanges();

                    for (int i = 0; i < set.Size - 1; i += 1)
                    {
                        Images.Add(new Image
                        {
                            Name = $"{set.Name} {String.Format("{0:000}", i + 1)}",
                            Cosplayer = Cosplayers.FirstOrDefault(y => y.Name == cp.Name),
                            Set = Sets.FirstOrDefault(y => y.Name == set.Name),
                            Url = $"{CloudUrl}{cp.Aliases}/{set.FolderName}/{set.FolderName}_{String.Format("{0:000}", i + 1)}.jpg",
                            Number = i + 1
                        });
                    }

                    SaveChanges();
                }
            }
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
