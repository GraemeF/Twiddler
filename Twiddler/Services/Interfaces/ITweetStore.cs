using Twiddler.Models;

namespace Twiddler.Services.Interfaces
{
    public interface ITweetStore : ITweetSink
    {
        Tweet GetTweet(TweetId id);
    }
}