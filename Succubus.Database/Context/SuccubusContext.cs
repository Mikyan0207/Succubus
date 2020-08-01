using Microsoft.EntityFrameworkCore;
using Succubus.Database.Models;
using Succubus.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

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

        public DbSet<Color> Colors { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Server> Servers { get; set; }
        public DbSet<Cosplayer> Cosplayers { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Set> Sets { get; set; }
        public DbSet<UserImage> UserImages { get; set; }
        public DbSet<DiscordChannel> DiscordChannels { get; set; }
        public DbSet<YoutubeChannel> YoutubeChannels { get; set; }

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

        public async void Initiliaze()
        {
            #region Cosplayers

            using var store = new NamedResourceStore<byte[]>(new DllResourceStore(new AssemblyName("Succubus.Resources")), @"Yabai");
            store.AddExtension(".json");

            List<string> cosplayers = store.GetResources().Select(x => Path.GetFileNameWithoutExtension(x)).ToList();

            foreach (string cosplayer in cosplayers)
            {
                CosplayerData cp = Utf8Json.JsonSerializer.Deserialize<CosplayerData>(store.Get(cosplayer));

                if (!await Cosplayers.AsQueryable().AnyAsync(x => x.Name == cp.Name).ConfigureAwait(false))
                {
                    await Cosplayers.AddAsync(new Cosplayer
                    {
                        Name = cp.Name,
                        Aliases = cp.Aliases,
                        Twitter = cp.Twitter,
                        Instagram = cp.Instagram,
                        Booth = cp.Booth,
                        ProfilePicture = $"{CloudUrl}{cp.ProfilePicture}"

                    }).ConfigureAwait(false);

                    await SaveChangesAsync().ConfigureAwait(false);
                }

                foreach (SetData set in cp.Sets)
                {
                    if (await Sets.AsQueryable().AnyAsync(x => x.Name == set.Name).ConfigureAwait(false))
                        continue;

                    await Sets.AddAsync(new Set
                    {
                        Name = set.Name,
                        Aliases = set.Aliases,
                        Cosplayer = Cosplayers.FirstOrDefault(y => y.Name == cp.Name),
                        Size = (uint)set.Size,
                        SetPreview = $@"{CloudUrl}{cp.Aliases}/{set.FolderName}/{set.FilePrefix ?? set.FolderName}_001.jpg",
                        YabaiLevel = (YabaiLevel)set.YabaiLevel

                    }).ConfigureAwait(false);

                    await SaveChangesAsync().ConfigureAwait(false);

                    for (int i = 0; i < set.Size - 1; i += 1)
                    {
                        await Images.AddAsync(new Image
                        {
                            Name = $"{set.Name} {String.Format("{0:000}", i + 1)}",
                            Cosplayer = Cosplayers.FirstOrDefault(y => y.Name == cp.Name),
                            Set = Sets.FirstOrDefault(y => y.Name == set.Name),
                            Url = $"{CloudUrl}{cp.Aliases}/{set.FolderName}/{set.FilePrefix ?? set.FolderName}_{String.Format("{0:000}", i + 1)}.jpg",
                            Number = i + 1

                        }).ConfigureAwait(false);
                    }

                    await SaveChangesAsync().ConfigureAwait(false);
                }
            }

            #endregion

            #region YouTube

            using var ytStore = new NamedResourceStore<byte[]>(new DllResourceStore(new AssemblyName("Succubus.Resources")), @"Youtube");
            store.AddExtension(".json");

            List<YoutubeModel> channels = Utf8Json.JsonSerializer.Deserialize<List<YoutubeModel>>(ytStore.Get("Channels"));

            foreach (var channel in channels)
            {
                if (await YoutubeChannels.AsQueryable().AnyAsync(x => x.ChannelId == channel.ChannelId).ConfigureAwait(false))
                    continue;

                await YoutubeChannels.AddAsync(new YoutubeChannel
                {
                    Name = channel.Name,
                    ChannelId = channel.ChannelId

                }).ConfigureAwait(false);
            }

            await SaveChangesAsync().ConfigureAwait(false);

            #endregion
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