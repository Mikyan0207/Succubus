using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Apis.YouTube.v3.Data;

namespace Succubus.Services
{
    public interface IYoutubeService : IService
    {
        Task<List<LiveResult>> GetChannelLives(string id);
    }
}