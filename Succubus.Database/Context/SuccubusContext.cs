using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Succubus.Database.Models;
using Succubus.Store;
using System;
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            ConfigurationStore = new NamedResourceStore<byte[]>(new DllResourceStore(new AssemblyName("Succubus.Resources")), @"Configuration");
            ConfigurationStore.AddExtension(".json");

            ConnectionString = JsonConvert.DeserializeObject<DbConfiguration>(Encoding.UTF8.GetString(ConfigurationStore.Get("Database"))).ConnectionString;
            CloudUrl = JsonConvert.DeserializeObject<ApiConfiguration>(Encoding.UTF8.GetString(ConfigurationStore.Get("Cloud"))).ApiUrl;

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
        }

        public void Initiliaze()
        {

            if (!Cosplayers.Any(x => x.Name == @"ふれいあ"))
            {
                Cosplayers.Add(new Cosplayer
                {
                    Name = @"ふれいあ",
                    Aliases = @"fleia,flemani,ふれまに",
                    Twitter = "https://twitter.com/fleia0124",
                    Instagram = "https://instagram.com/fleia0124",
                    Booth = "https://flemani.booth.pm/",
                    ProfilePicture = @$"{CloudUrl}Fleia/ProfilePicture.jpg"
                });

                SaveChanges();
            }

            if (!Sets.Any(x => x.Name == @"ふれみこ"))
            {
                Sets.Add(new Set
                {
                    Name = @"ふれみこ",
                    Cosplayer = Cosplayers.FirstOrDefault(x => x.Name == @"ふれいあ"),
                    Size = 182,
                    SetPreview = $@"{CloudUrl}Fleia/FleiaMiko/FleiaMiko_001.jpg",
                });

                SaveChanges();

                for (int i = 0; i  < 181; i += 1)
                {
                    Images.Add(new Image
                    {
                        Name = @$"FleiaMiko {String.Format("{0:000}", i + 1)}",
                        Cosplayer = Cosplayers.FirstOrDefault(x => x.Name == @"ふれいあ"),
                        Set = Sets.FirstOrDefault(x => x.Name == @"ふれみこ"),
                        Url = $@"{CloudUrl}Fleia/FleiaMiko/FleiaMiko_{String.Format("{0:000}", i + 1)}.jpg",
                        Number = i + 1
                    });
                }

                SaveChanges();
            }

            if (!Sets.Any(x => x.Name == @"Honey Bunny"))
            {
                Sets.Add(new Set
                {
                    Name = @"Honey Bunny",
                    Cosplayer = Cosplayers.FirstOrDefault(x => x.Name == @"ふれいあ"),
                    Size = 349,
                    SetPreview = $@"{CloudUrl}Fleia/HoneyBunny/HoneyBunny_001.jpg",
                });

                SaveChanges();

                for (int i = 0; i < 348; i += 1)
                {
                    Images.Add(new Image
                    {
                        Name = @$"Honey Bunny {String.Format("{0:000}", i + 1)}",
                        Cosplayer = Cosplayers.FirstOrDefault(x => x.Name == @"ふれいあ"),
                        Set = Sets.FirstOrDefault(x => x.Name == @"Honey Bunny"),
                        Url = $@"{CloudUrl}Fleia/HoneyBunny/HoneyBunny_{String.Format("{0:000}", i + 1)}.jpg",
                        Number = i + 1
                    });
                }

                SaveChanges();
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
