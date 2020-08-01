using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Google.Apis.Services;
using Google.Apis.Urlshortener.v1;
using Google.Apis.Util;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Succubus.Store;
using Utf8Json;

namespace Succubus.Services
{
    public class YoutubeApiService : IYoutubeService
    {
        public YoutubeApiService()
        {
            using var store = new NamedResourceStore<byte[]>(new DllResourceStore(new AssemblyName("Succubus.Resources")), @"Configuration");
            store.AddExtension(".json");

            var apiKey = JsonSerializer.Deserialize<YoutubeApiConfiguration>(store.Get("Youtube")).ApiKey;
            YoutubeService = new YouTubeService(new BaseClientService.Initializer
            {
                ApplicationName = "Succubus Bot",
                ApiKey = apiKey
            });
            UrlshortenerService = new UrlshortenerService(new BaseClientService.Initializer
            {
                ApplicationName = "Succubus Bot",
                ApiKey = apiKey
            });
        }

        private YouTubeService YoutubeService { get; }
        private UrlshortenerService UrlshortenerService { get; }

        public async Task<List<LiveResult>> GetChannelLives(string id)
        {
            var query = YoutubeService.Search.List("snippet");

            query.ChannelId = id;
            query.EventType = SearchResource.ListRequest.EventTypeEnum.Live;
            query.Type = "video";

            var result = (await query.ExecuteAsync().ConfigureAwait(false));

            try
            {
                return result?.Items?.Select(x => new LiveResult
                {
                    Title = x.Snippet.Title,
                    ChannelName = x.Snippet.ChannelTitle,
                    ThumbnailUrl = x.Snippet.Thumbnails?.High?.Url ?? x.Snippet.Thumbnails?.Default__?.Url,
                    LiveUrl = $"https://www.youtube.com/watch?v={x.Id.VideoId}",
                    Description = x.Snippet.Description,
                    PublishedAt = DateTime.Parse(x.Snippet.PublishedAt)
                }).ToList();
            }
            catch (NullReferenceException e)
            {
                return null;
            }

           
        }
    }

    public class YoutubeApiConfiguration
    {
        public string ApiKey { get; set; }
    }

    public class LiveResult
    {
        public string Title { get; set; }
        public string ChannelName { get; set; }
        public string ThumbnailUrl { get; set; }
        public string LiveUrl { get; set; }
        public string Description { get; set; }
        public DateTime PublishedAt { get; set; }
    }
}