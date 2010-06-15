using Twiddler.Core.Models;

namespace Twiddler.Core.Services
{
    public interface ITweetStore : ITweetSink
    {
        Tweet GetTweet(string id);
    }
}