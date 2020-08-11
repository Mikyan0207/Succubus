using System.Linq;
using Microsoft.EntityFrameworkCore;
using Succubus.Database;
using Succubus.Services.Interfaces;

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
    }
}