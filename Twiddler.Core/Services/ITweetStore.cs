using Twiddler.Models;

namespace Twiddler.Core.Services
{
    public interface ITweetStore : ITweetSink
    {
        Tweet GetTweet(TweetId id);
    }
}