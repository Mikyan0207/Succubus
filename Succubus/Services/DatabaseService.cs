using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Succubus.Database;
using Succubus.Database.Models;
using Succubus.Services.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Succubus.Services
{
    public class DatabaseService : IService
    {
        public DatabaseService()
        {
            using var context = new SuccubusContext();
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
                context.SaveChanges();
            }

            context.Database.ExecuteSqlRaw("PRAGMA journal_mode=WAL; PRAGMA synchronous=OFF");
            context.SaveChanges();
            Initialize(context);
            context.SaveChanges();
        }

        public SuccubusContext GetContext()
        {
            var context = new SuccubusContext();
            context.Database.SetCommandTimeout(60);

            var conn = context.Database.GetDbConnection();
            conn.Open();

            using var com = conn.CreateCommand();
            com.CommandText = "PRAGMA journal_mode=WAL; PRAGMA synchronous=OFF";
            com.ExecuteNonQuery();

            return context;
        }

        private static void Initialize(SuccubusContext context)
        {
            var folder = Directory.GetParent(Assembly.GetCallingAssembly().Location)?.Parent?.Parent?.Parent?.FullName;
            var content = File.ReadAllText($"{folder}/Resources/Cosplayers.json");

            var cosplayers = JsonConvert.DeserializeObject<List<Cosplayer>>(content);

            foreach (var cp in cosplayers.Where(cp => !context.Cosplayers.Any(x => x.Name == cp.Keywords.FirstOrDefault())).ToList())
            {
                var c = context.Add(new Cosplayer
                {
                    Name = cp.Keywords[0],
                    Keywords = cp.Keywords,
                    Socials = cp.Socials
                });

                context.SaveChanges();

                foreach (var set in cp.Sets.Where(set => !context.Sets.Any(x => x.Name == set.Keywords.FirstOrDefault())).ToList())
                {
                    context.Add(new Set
                    {
                        Name = set.Keywords[0],
                        Keywords = set.Keywords,
                        Cosplayer = c.Entity,
                        Folder = set.Folder,
                        Prefix = set.Prefix,
                        Size = set.Size
                    });

                    context.SaveChanges();
                }
            }
        }
    }
}