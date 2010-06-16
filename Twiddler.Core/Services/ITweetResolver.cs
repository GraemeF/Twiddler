using Twiddler.Core.Models;

namespace Twiddler.Core.Services
{
    public interface ITweetResolver : ITweetSink
    {
        Tweet GetTweet(string id);
    }
}