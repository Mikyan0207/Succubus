using Google.Apis.Services;
using Google.Apis.Urlshortener.v1;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Succubus.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Succubus.Services
{
    public class YoutubeApiService : IYoutubeService
    {
        private readonly string ApiKey;
        private YouTubeService YoutubeService { get; set; }
        private UrlshortenerService UrlshortenerService { get; set; }

        public YoutubeApiService()
        {
            using var store = new NamedResourceStore<byte[]>(new DllResourceStore(new AssemblyName("Succubus.Resouces")), @"Configuration");
            store.AddExtension(".json");

            ApiKey = Utf8Json.JsonSerializer.Deserialize<YoutubeApiConfiguration>(store.Get("Youtube")).ApiKey;
            YoutubeService = new YouTubeService(new BaseClientService.Initializer
            {
                ApplicationName = "Succubus Bot",
                ApiKey = ApiKey
            });
            UrlshortenerService = new UrlshortenerService(new BaseClientService.Initializer
            {
                ApplicationName = "Succubus Bot",
                ApiKey = ApiKey
            });
        }

        public async Task<List<SearchResultSnippet>> GetChannelLives(string id)
        {
            var query = YoutubeService.Search.List("snippet");

            query.ChannelId = id;
            query.EventType = SearchResource.ListRequest.EventTypeEnum.Live;
            query.Type = "video";

            return (await query.ExecuteAsync().ConfigureAwait(false)).Items.Select(x => x.Snippet).ToList();
        }
    }

    public class YoutubeApiConfiguration
    {
        public string ApiKey { get; set; }
    }
}
