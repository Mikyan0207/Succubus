using System.Reflection;
using Mikyan.Framework.Services;
using Mikyan.Framework.Stores;
using Succubus.Database.Context;
using Utf8Json;

namespace Succubus.Services
{
    public class BotService : IService
    {
        public BotService()
        {
            using var configurationStore =
                new NamedResourceStore<byte[]>(new DllResourceStore(new AssemblyName("Succubus.Resources")),
                    @"Configuration");
            configurationStore.AddExtension(".json");

            CloudUrl = JsonSerializer.Deserialize<ApiConfiguration>(configurationStore.Get("Cloud")).ApiUrl;
        }

        public string CloudUrl { get; }
    }
}