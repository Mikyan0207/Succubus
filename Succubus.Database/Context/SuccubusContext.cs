using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Succubus.Database.Models;
using Succubus.Store;
using System;
using System.Collections.Generic;
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
            using (var store = new NamedResourceStore<byte[]>(new DllResourceStore(new AssemblyName("Succubus.Resources")), @"Yabai"))
            {
                store.AddExtension(".json");

                List<string> cosplayers = store.GetResources().ToList();

                foreach(string cosplayer in cosplayers)
                {
                    Cosplayer cp = JsonConvert.DeserializeObject<Cosplayer>(Encoding.UTF8.GetString(store.Get(cosplayer)));

                    if (!Cosplayers.Any(x => x.Name == cp.Name))
                    {
                        Cosplayers.Add(new Cosplayer
                        {
                            Name = cp.Name,
                            Aliases = cp.Aliases,
                            Twitter = cp.Twitter,
                            Instagram = cp.Instagram,
                            Booth = cp.Booth,
                            ProfilePicture = cp.ProfilePicture
                        });

                        SaveChanges();
                        Sets.AddRange(cp.Sets);
                        SaveChanges();
                    }
                }
            }

            //if (!Cosplayers.Any(x => x.Name == @"ふれいあ"))
            //{
            //    Cosplayers.Add(new Cosplayer
            //    {
            //        Name = @"ふれいあ",
            //        Aliases = @"fleia",
            //        Twitter = "https://twitter.com/fleia0124",
            //        Instagram = "https://instagram.com/fleia0124",
            //        Booth = "https://flemani.booth.pm/",
            //        ProfilePicture = @$"{CloudUrl}Fleia/ProfilePicture.jpg"
            //    });

            //    SaveChanges();
            //}

            //if (!Cosplayers.Any(x => x.Name == "いくみ"))
            //{
            //    Cosplayers.Add(new Cosplayer
            //    {
            //        Name = @"いくみ",
            //        Aliases = @"iKkyu",
            //        Twitter = "https://twitter.com/193azs",
            //        Instagram = "",
            //        Booth = "https://ikkyu3.booth.pm/",
            //        ProfilePicture = @$"{CloudUrl}iKkyu/ProfilePicture.jpg"
            //    });

            //    SaveChanges();
            //}

            //if (!Cosplayers.Any(x => x.Name == @"りずな"))
            //{
            //    Cosplayers.Add(new Cosplayer
            //    {
            //        Name = @"りずな",
            //        Aliases = @"rizuna",
            //        Twitter = @"https://mobile.twitter.com/rizunya",
            //        Instagram = @"https://www.instagram.com/rizuna1228/",
            //        Booth = @"https://rizunya.booth.pm/",
            //        ProfilePicture = @$"{CloudUrl}Rizuna/ProfilePicture.jpg"
            //    });

            //    SaveChanges();
            //}

            //if (!Cosplayers.Any(x => x.Name == @"ぽにょ"))
            //{
            //    Cosplayers.Add(new Cosplayer
            //    {
            //        Name = @"ぽにょ",
            //        Aliases = @"ponyo",
            //        Twitter = @"https://twitter.com/white613like",
            //        Instagram = @"https://www.instagram.com/ponyoouji/",
            //        Booth = @"https://ponyophoto.booth.pm/",
            //        ProfilePicture = @$"{CloudUrl}Ponyo/ProfilePicture.jpg"
            //    });

            //    SaveChanges();
            //}

            //#region Fleia

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

            //#endregion

            //#region Ikumi

            //if (!Sets.Any(x => x.Name == "Ikumi Trip"))
            //{
            //    Sets.Add(new Set
            //    {
            //        Name = @"Ikumi Trip",
            //        Aliases = "",
            //        Cosplayer = Cosplayers.FirstOrDefault(x => x.Name == @"いくみ"),
            //        SetPreview = $@"{CloudUrl}iKkyu/IkumiTrip/IkumiTrip_001.jpg",
            //        Size = 189
            //    });

            //    SaveChanges();

            //    for (int i = 0; i < 188; i += 1)
            //    {
            //        Images.Add(new Image
            //        {
            //            Name = $@"Ikumi Trip {String.Format("{0:000}", i + 1)}",
            //            Cosplayer = Cosplayers.FirstOrDefault(x => x.Name == @"いくみ"),
            //            Set = Sets.FirstOrDefault(x => x.Name == @"Ikumi Trip"),
            //            Url = $@"{CloudUrl}iKkyu/IkumiTrip/IkumiTrip_{String.Format("{0:000}", i + 1)}.jpg",
            //            Number = i + 1
            //        });
            //    }

            //    SaveChanges();
            //}

            //if (!Sets.Any(x => x.Name == "Skinny"))
            //{
            //    Sets.Add(new Set
            //    {
            //        Name = @"Skinny",
            //        Aliases = "",
            //        Cosplayer = Cosplayers.FirstOrDefault(x => x.Name == @"いくみ"),
            //        SetPreview = $@"{CloudUrl}iKkyu/Skinny/Skinny_01.jpg",
            //        Size = 50
            //    });

            //    SaveChanges();

            //    for (int i = 0; i < 49; i += 1)
            //    {
            //        Images.Add(new Image
            //        {
            //            Name = $@"Skinny {String.Format("{0:00}", i + 1)}",
            //            Cosplayer = Cosplayers.FirstOrDefault(x => x.Name == @"いくみ"),
            //            Set = Sets.FirstOrDefault(x => x.Name == @"Skinny"),
            //            Url = $@"{CloudUrl}iKkyu/Skinny/skinny_{String.Format("{0:00}", i + 1)}.jpg",
            //            Number = i + 1
            //        });
            //    }

            //    SaveChanges();
            //}

            //#endregion

            //#region Rizuna

            //if (!Sets.Any(x => x.Name == @"Great Ocean"))
            //{
            //    Sets.Add(new Set
            //    {
            //        Name = @"Great Ocean",
            //        Aliases = "go",
            //        Cosplayer = Cosplayers.FirstOrDefault(x => x.Name == @"りずな"),
            //        SetPreview = $@"{CloudUrl}Rizuna/GreatOcean/GreatOcean_001.jpg",
            //        Size = 101
            //    });

            //    SaveChanges();

            //    for (int i = 0; i < 100; i += 1)
            //    {
            //        Images.Add(new Image
            //        {
            //            Name = $@"Great Ocean {String.Format("{0:000}", i + 1)}",
            //            Cosplayer = Cosplayers.FirstOrDefault(x => x.Name == @"りずな"),
            //            Set = Sets.FirstOrDefault(x => x.Name == @"Great Ocean"),
            //            Url = $@"{CloudUrl}Rizuna/GreatOcean/GreatOcean_{String.Format("{0:000}", i + 1)}.jpg",
            //            Number = i + 1
            //        });
            //    }

            //    SaveChanges();
            //}

            //if (!Sets.Any(x => x.Name == @"冴えない彼女の裏のかお"))
            //{
            //    Sets.Add(new Set
            //    {
            //        Name = @"冴えない彼女の裏のかお",
            //        Aliases = "saenai kanojo",
            //        Cosplayer = Cosplayers.FirstOrDefault(x => x.Name == @"りずな"),
            //        SetPreview = $@"{CloudUrl}Rizuna/SaenaiKanojo/SaenaiKanojo_001.jpg",
            //        Size = 100,
            //        YabaiLevel = YabaiLevel.NotSafe
            //    });

            //    SaveChanges();

            //    for (int i = 0; i < 99; i += 1)
            //    {
            //        Images.Add(new Image
            //        {
            //            Name = $@"冴えない彼女の裏のかお {String.Format("{0:000}", i + 1)}",
            //            Cosplayer = Cosplayers.FirstOrDefault(x => x.Name == @"りずな"),
            //            Set = Sets.FirstOrDefault(x => x.Name == @"冴えない彼女の裏のかお"),
            //            Url = $@"{CloudUrl}Rizuna/SaenaiKanojo/SaenaiKanojo_{String.Format("{0:000}", i + 1)}.jpg",
            //            Number = i + 1
            //        });
            //    }

            //    SaveChanges();
            //}

            //if (!Sets.Any(x => x.Name == @"Hedonistic Labyrinth"))
            //{
            //    Sets.Add(new Set
            //    {
            //        Name = @"Hedonistic Labyrinth",
            //        Aliases = "labyrinth",
            //        Cosplayer = Cosplayers.FirstOrDefault(x => x.Name == @"りずな"),
            //        SetPreview = $@"{CloudUrl}Rizuna/HedonisticLabyrinth/HedonisticLabyrinth_001.jpg",
            //        Size = 100,
            //        YabaiLevel = YabaiLevel.NotSafe
            //    });

            //    SaveChanges();

            //    for (int i = 0; i < 99; i += 1)
            //    {
            //        Images.Add(new Image
            //        {
            //            Name = $@"Hedonistic Labyrinth {String.Format("{0:000}", i + 1)}",
            //            Cosplayer = Cosplayers.FirstOrDefault(x => x.Name == @"りずな"),
            //            Set = Sets.FirstOrDefault(x => x.Name == @"Hedonistic Labyrinth"),
            //            Url = $@"{CloudUrl}Rizuna/HedonisticLabyrinth/HedonisticLabyrinth_{String.Format("{0:000}", i + 1)}.jpg",
            //            Number = i + 1
            //        });
            //    }

            //    SaveChanges();
            //}

            //#endregion

            //#region Ponyo

            //if (!Sets.Any(x => x.Name == @"Succubus"))
            //{
            //    Sets.Add(new Set
            //    {
            //        Name = @"Succubus",
            //        Aliases = "",
            //        Cosplayer = Cosplayers.FirstOrDefault(x => x.Name == @"ぽにょ"),
            //        SetPreview = $@"{CloudUrl}Ponyo/Succubus/Succubus_001.jpg",
            //        Size = 218
            //    });

            //    SaveChanges();

            //    for (int i = 0; i < 217; i += 1)
            //    {
            //        Images.Add(new Image
            //        {
            //            Name = $@"Succubus {String.Format("{0:000}", i + 1)}",
            //            Cosplayer = Cosplayers.FirstOrDefault(x => x.Name == @"ぽにょ"),
            //            Set = Sets.FirstOrDefault(x => x.Name == @"Succubus"),
            //            Url = $@"{CloudUrl}Ponyo/Succubus/Succubus_{String.Format("{0:000}", i + 1)}.jpg",
            //            Number = i + 1
            //        });
            //    }

            //    SaveChanges();
            //}

            //#endregion
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
