using Mikyan.Framework.Services;
using Mikyan.Framework.Stores;
using Succubus.Database.Context;
using System.Reflection;

namespace Succubus.Services
{
    public class BotService : IService
    {
        public string CloudUrl { get; }

        public BotService()
        {
            using var configurationStore = new NamedResourceStore<byte[]>(new DllResourceStore(new AssemblyName("Succubus.Resources")), @"Configuration");
            configurationStore.AddExtension(".json");

            CloudUrl = Utf8Json.JsonSerializer.Deserialize<ApiConfiguration>(configurationStore.Get("Cloud")).ApiUrl;
        }
    }
}