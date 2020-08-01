using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

namespace Succubus.Services
{
    public interface IYoutubeService : IService
    {
        Task<List<SearchResultSnippet>> GetChannelLives(string id);
    }
}
