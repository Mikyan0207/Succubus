using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Succubus.Store;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Succubus.Database.Context
{
    public class SuccubusContext : DbContext
    {
        private readonly NamedResourceStore<byte[]> ConfigurationStore;

        public SuccubusContext()
        {
            ConfigurationStore = new NamedResourceStore<byte[]>(new DllResourceStore(new AssemblyName("Succubus.Resources")), @"Configuration");

            ConfigurationStore.AddExtension(".json");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($@"Data Source={GetConnectionString()}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        private string GetConnectionString() => JsonConvert.DeserializeObject<string>(Encoding.UTF8.GetString(ConfigurationStore.Get("Database")));
    }
}
