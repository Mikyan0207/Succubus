﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Succubus.Database.Context;
using Succubus.Database.UnitOfWorks;

namespace Succubus.Services
{
    public class DbService : IService
    {
        public DbService()
        {
            using (var context = new SuccubusContext())
            {
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                    context.SaveChanges();
                }

                context.Database.ExecuteSqlRaw("PRAGMA journal_mode=WAL");
                context.Initiliaze();
                context.SaveChanges();
            }
        }

        private SuccubusContext GetDbContextInternal()
        {
            var context = new SuccubusContext();
            context.Database.SetCommandTimeout(60);

            var conn = context.Database.GetDbConnection();
            conn.Open();

            using (var com = conn.CreateCommand())
            {
                com.CommandText = "PRAGMA journal_mode=WAL; PRAGMA synchronous=OFF";
                com.ExecuteNonQuery();
            }

            return context;
        }

        public IUnitOfWork GetDbContext() => new UnitOfWork(GetDbContextInternal());
    }
}