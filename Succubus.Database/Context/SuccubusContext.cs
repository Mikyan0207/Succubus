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
                    Aliases = @"fleia",
                    Twitter = "https://twitter.com/fleia0124",
                    Instagram = "https://instagram.com/fleia0124",
                    Booth = "https://flemani.booth.pm/",
                    ProfilePicture = @$"{CloudUrl}Fleia/ProfilePicture.jpg"
                });

                SaveChanges();
            }

            if (!Cosplayers.Any(x => x.Name == "いくみ"))
            {
                Cosplayers.Add(new Cosplayer
                {
                    Name = @"いくみ",
                    Aliases = @"iKkyu",
                    Twitter = "https://twitter.com/193azs",
                    Instagram = "",
                    Booth = "https://ikkyu3.booth.pm/",
                    ProfilePicture = @$"{CloudUrl}iKkyu/ProfilePicture.jpg"
                });

                SaveChanges();
            }

            if (!Sets.Any(x => x.Name == @"ふれみこ"))
            {
                Sets.Add(new Set
                {
                    Name = @"ふれみこ",
                    Aliases = "furemiko",
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
                    Aliases = "",
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

            //if (!Sets.Any(x => x.Name == @"HeartBreak Morgan"))
            //{
            //    Sets.Add(new Set
            //    {
            //        Name = @"HeartBreak Morgan",
            //        Cosplayer = Cosplayers.FirstOrDefault(x => x.Name == @"ふれいあ"),
            //        Size = 243,
            //        SetPreview = $@"{CloudUrl}Fleia/HeartBreakMorgan/Morgan_001.jpg",
            //    });

            //    SaveChanges();

            //    for (int i = 0; i < 242; i += 1)
            //    {
            //        Images.Add(new Image
            //        {
            //            Name = @$"HeartBreak Morgan {String.Format("{0:000}", i + 1)}",
            //            Cosplayer = Cosplayers.FirstOrDefault(x => x.Name == @"ふれいあ"),
            //            Set = Sets.FirstOrDefault(x => x.Name == @"HeartBreak Morgan"),
            //            Url = $@"{CloudUrl}Fleia/HeartBreakMorgan/Morgan_{String.Format("{0:000}", i + 1)}.jpg",
            //            Number = i + 1
            //        });
            //    }

            //    SaveChanges();
            //}

            if (!Sets.Any(x => x.Name == "Immoral"))
            {
                Sets.Add(new Set
                {
                    Name = @"Immoral",
                    Aliases = "",
                    Cosplayer = Cosplayers.FirstOrDefault(x => x.Name == @"ふれいあ"),
                    Size = 215,
                    SetPreview = $@"{CloudUrl}Fleia/Immoral/Immoral_001.jpg"
                });

                SaveChanges();

                for (int i = 0; i < 214; i += 1)
                {
                    Images.Add(new Image
                    {
                        Name = $@"Immoral {String.Format("{0:000}", i + 1)}",
                        Cosplayer = Cosplayers.FirstOrDefault(x => x.Name == @"ふれいあ"),
                        Set = Sets.FirstOrDefault(x => x.Name == @"Immoral"),
                        Url = $@"{CloudUrl}Fleia/Immoral/Immoral_{String.Format("{0:000}", i + 1)}.jpg",
                        Number = i + 1
                    });
                }

                SaveChanges();
            }

            if (!Sets.Any(x => x.Name == "Black or White"))
            {
                Sets.Add(new Set
                {
                    Name = @"Black or White",
                    Aliases = "bow",
                    Cosplayer = Cosplayers.FirstOrDefault(x => x.Name == @"ふれいあ"),
                    Size = 322,
                    SetPreview = $@"{CloudUrl}Fleia/BoW/BoW_001.jpg"
                });

                SaveChanges();

                for (int i = 0; i < 321; i += 1)
                {
                    Images.Add(new Image
                    {
                        Name = $@"Black or White {String.Format("{0:000}", i + 1)}",
                        Cosplayer = Cosplayers.FirstOrDefault(x => x.Name == @"ふれいあ"),
                        Set = Sets.FirstOrDefault(x => x.Name == @"Black or White"),
                        Url = $@"{CloudUrl}Fleia/BoW/BoW_{String.Format("{0:000}", i + 1)}.jpg",
                        Number = i + 1
                    });
                }

                SaveChanges();
            }

            if (!Sets.Any(x => x.Name == "Rem"))
            {
                Sets.Add(new Set
                {
                    Name = @"Rem",
                    Aliases = "",
                    Cosplayer = Cosplayers.FirstOrDefault(x => x.Name == @"ふれいあ"),
                    Size = 166,
                    SetPreview = $@"{CloudUrl}Fleia/Rem/Rem_001.jpg"
                });

                SaveChanges();

                for (int i = 0; i < 165; i += 1)
                {
                    Images.Add(new Image
                    {
                        Name = $@"Rem {String.Format("{0:000}", i + 1)}",
                        Cosplayer = Cosplayers.FirstOrDefault(x => x.Name == @"ふれいあ"),
                        Set = Sets.FirstOrDefault(x => x.Name == @"Rem"),
                        Url = $@"{CloudUrl}Fleia/Rem/Rem_{String.Format("{0:000}", i + 1)}.jpg",
                        Number = i + 1
                    });
                }

                SaveChanges();
            }

            if (!Sets.Any(x => x.Name == "Danmachi"))
            {
                Sets.Add(new Set
                {
                    Name = @"Danmachi",
                    Aliases = "hestia",
                    Cosplayer = Cosplayers.FirstOrDefault(x => x.Name == @"ふれいあ"),
                    SetPreview = $@"{CloudUrl}Fleia/Danmachi/Danmachi_001.jpg",
                    Size = 264
                });

                SaveChanges();

                for (int i = 0; i < 263; i += 1)
                {
                    Images.Add(new Image
                    {
                        Name = $@"Danmachi {String.Format("{0:000}", i + 1)}",
                        Cosplayer = Cosplayers.FirstOrDefault(x => x.Name == @"ふれいあ"),
                        Set = Sets.FirstOrDefault(x => x.Name == @"Danmachi"),
                        Url = $@"{CloudUrl}Fleia/Danmachi/Danmachi_{String.Format("{0:000}", i + 1)}.jpg",
                        Number = i + 1
                    });
                }

                SaveChanges();
            }

            if (!Sets.Any(x => x.Name == "Ikumi Trip"))
            {
                Sets.Add(new Set
                {
                    Name = @"Ikumi Trip",
                    Aliases = "",
                    Cosplayer = Cosplayers.FirstOrDefault(x => x.Name == @"いくみ"),
                    SetPreview = $@"{CloudUrl}iKkyu/IkumiTrip/IkumiTrip_001.jpg",
                    Size = 189
                });

                SaveChanges();

                for (int i = 0; i < 188; i += 1)
                {
                    Images.Add(new Image
                    {
                        Name = $@"Ikumi Trip {String.Format("{0:000}", i + 1)}",
                        Cosplayer = Cosplayers.FirstOrDefault(x => x.Name == @"いくみ"),
                        Set = Sets.FirstOrDefault(x => x.Name == @"Ikumi Trip"),
                        Url = $@"{CloudUrl}iKkyu/IkumiTrip/IkumiTrip_{String.Format("{0:000}", i + 1)}.jpg",
                        Number = i + 1
                    });
                }

                SaveChanges();
            }

            if (!Sets.Any(x => x.Name == "Skinny"))
            {
                Sets.Add(new Set
                {
                    Name = @"Skinny",
                    Aliases = "",
                    Cosplayer = Cosplayers.FirstOrDefault(x => x.Name == @"いくみ"),
                    SetPreview = $@"{CloudUrl}iKkyu/Skinny/Skinny_01.jpg",
                    Size = 50
                });

                SaveChanges();

                for (int i = 0; i < 49; i += 1)
                {
                    Images.Add(new Image
                    {
                        Name = $@"Skinny {String.Format("{0:00}", i + 1)}",
                        Cosplayer = Cosplayers.FirstOrDefault(x => x.Name == @"いくみ"),
                        Set = Sets.FirstOrDefault(x => x.Name == @"Skinny"),
                        Url = $@"{CloudUrl}iKkyu/Skinny/skinny_{String.Format("{0:00}", i + 1)}.jpg",
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
